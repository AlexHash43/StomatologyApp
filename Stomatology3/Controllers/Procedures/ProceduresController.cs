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
        public async Task<IActionResult> GetProceduresAsync()
        {
            var proceduresDbList = await _context.ProcedureTypes.ToListAsync();
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
        [HttpGet("procedure")]
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
        [HttpPost]
        public async Task<IActionResult> CreateProcedure(string name)//, CancellationToken cancellationToken)
        {
            if (name == string.Empty) return BadRequest(AppResources.NullTask);
            var procedure = await _context.ProcedureTypes.FirstOrDefaultAsync(proc => proc.ProcedureName.ToLower() == name.ToLower());//, cancellationToken);
            if (procedure != null) return BadRequest(AppResources.TaskAlreadyExists);
            var newProcedure = new ProcedureType
            {
               // ProcId = procedure.ProcId,
                ProcedureName = name.ToLower(),
            };
            var addAsync = await _context.ProcedureTypes.AddAsync(newProcedure);//, cancellationToken);
            var saveAsync = await _context.SaveChangesAsync();//cancellationToken);
            if (saveAsync == 0) return BadRequest(AppResources.TaskCreationNotSaved);
            return Ok(GetProceduresAsync());
        }

        // PUT api/<ProceduresController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateProcedureAsync(CreatedProcedure procedure)//, CancellationToken cancellationToken)
        {
            if(procedure==null) return BadRequest(AppResources.NullTask);
            var originalProcedure = await _context.ProcedureTypes.FindAsync(procedure.ProcId);
            originalProcedure.ProcedureName = procedure.ProcedureName;
            var updater = _context.ProcedureTypes.Update(originalProcedure);
            var saver = await _context.SaveChangesAsync();
            if (saver == 0) //|| saver==0)
                return BadRequest();

            return Ok(GetProceduresAsync());
        }

        // DELETE api/<ProceduresController>/5
        [HttpDelete]
        public async Task<IActionResult> DeleteProcedureAsync(CreatedProcedure procedure)//, CancellationToken cancellationToken)
        {
            if (procedure == null) return BadRequest(AppResources.NullRole);
            var procToDelete = await _context.ProcedureTypes.FirstOrDefaultAsync(proc => proc.ProcId == procedure.ProcId);
            if (procToDelete == null) return BadRequest(AppResources.RoleDoesNotExist);
            _context.ProcedureTypes.Remove(procToDelete);
            var result = await _context.SaveChangesAsync();
            if (result == 0) return BadRequest(AppResources.RoleDeletionImpossible);
            return Ok(GetProceduresAsync());
        }
    }
}
