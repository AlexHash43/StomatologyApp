using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Context;
using Stomatology3.Controllers.Procedures.ProcedureModels;
using Stomatology3.Models;
using Stomatology3.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stomatology3.Controllers.Procedures
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProceduresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProceduresController(ApplicationDbContext context)
        {
            _context = context;

        }
        // GET: api/<ProceduresController>
        [HttpGet]
        public IActionResult GetProcedures()
        {
            var proceduresDbList = _context.ProcedureTypes.ToList();
            //if (!proceduresDbList.Any())
                 //return BadRequest(AppResources.RolesDoNotExist);
            var proceduresList = proceduresDbList.Select(procedure => new CreatedProcedure
            {
                ProcId = procedure.ProcId,
                ProcedureName = procedure.ProcedureName
            });

            return Ok(proceduresList);
        }

        // GET api/<ProceduresController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcedureAsync(int id)
        {
            var procedure = await _context.ProcedureTypes.FirstOrDefaultAsync(proc => proc.ProcId == id);
            if (procedure is null) return BadRequest(AppResources.NoTaskId);

            return Ok(new
            {
                Procedure = new CreatedProcedure
                {
                    ProcId = procedure.ProcId,
                    ProcedureName = procedure.ProcedureName

                }
            });
        }

        // POST api/<ProceduresController>
        [HttpPost ("createprocedure")]
        public async Task<IActionResult> CreateProcedure(string name, CancellationToken cancellationToken)
        {
            if (name == string.Empty) return BadRequest(AppResources.NullTask);
            var procedure = await _context.ProcedureTypes.FirstOrDefaultAsync(proc => proc.ProcedureName.ToLower() == name.ToLower(), cancellationToken);
            if (procedure != null) return BadRequest(AppResources.TaskAlreadyExists);
            var newProcedure = new ProcedureType
            {
                ProcId = procedure.ProcId,
                ProcedureName = procedure.ProcedureName,
            };
            var addAsync = await _context.ProcedureTypes.AddAsync(newProcedure, cancellationToken);
            var saveAsync = await  _context.SaveChangesAsync(cancellationToken);
            if (saveAsync == 0) return BadRequest(AppResources.TaskCreationNotSaved);
            return Ok(newProcedure);
        }

        // PUT api/<ProceduresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProceduresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
