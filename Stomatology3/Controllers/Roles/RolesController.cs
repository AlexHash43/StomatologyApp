﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stomatology3.Context;
using Stomatology3.Controllers.Users.RoleManagementModels;
using Stomatology3.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stomatology3.Controllers.Roles
{
    //[Authorize(Policy = "admin" )]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/<RolesController>
        [HttpGet]
        public IActionResult GetRoles()
        {
            var roleDbList = _roleManager.Roles.ToList();
            if (!roleDbList.Any()) return BadRequest(AppResources.RolesDoNotExist);
            var roleList = roleDbList.Select(role => new RoleModel
            {
                Id = role.Id,
                Name = role.Name
            });

            return Ok(roleList);
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public IActionResult GetRoles(string id)
        {
            var userRoles = _roleManager.Roles.Where(role => role.Id == id).ToList();
            if (!userRoles.Any()) return BadRequest(AppResources.RolesDoNotExist);
            var roleList = userRoles.Select(role => new RoleModel
            {
                Id = role.Id,
                Name = role.Name
            });

            return Ok(roleList);
        }

        // POST api/<RolesController>
        [HttpPost("{name}")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if(roleName == string.Empty) return BadRequest(AppResources.NullRole);
            
            var role = _context.Roles.FirstOrDefault(role => role.Name.ToLower() == roleName.ToLower());
            if (role is not null) return BadRequest(AppResources.RoleAlreadyExists);

            var newRole = new IdentityRole
            {
                Name = roleName
            };
            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                var roleDbList = _roleManager.Roles.ToList();
                var roleList = roleDbList.Select(role => new RoleModel
                {
                    Id = role.Id,
                    Name = role.Name
                });

                return Ok(new { Roles = roleList, Message = AppResources.RoleCreated });
            }
            return BadRequest(AppResources.RoleCreationImpossible);
        }

        // PUT api/<RolesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
