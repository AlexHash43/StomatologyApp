﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Context;
using Stomatology3.Controllers.Users.RoleManagementModels;
using Stomatology3.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stomatology3.Controllers.Roles
{
    //[Authorize(Policy = "admin" )]
    [ApiController]
    [Route("api/[controller]")]

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
        [HttpPut("updaterole")]
        public async Task<IActionResult> EditRole(RoleModel role)//, CancellationToken cancellationToken)
        {
            if (role == null) return NotFound("Role model is Empty");
            var originalRole = await _roleManager.Roles.FirstOrDefaultAsync(a => a.Name.ToLower() == role.Name.ToLower());
            if (originalRole == null) return NotFound("Original role not found");
            originalRole.Id = role.Id;
            originalRole.Name = role.Name;
            //_context.Roles.Update(originalRole);
            var updater = await _roleManager.UpdateAsync(originalRole);
            var saver = await _context.SaveChangesAsync();
            if (updater.Succeeded && saver!=0)
            return Ok(new RoleModel 
            {
                Id = originalRole.Id,
                Name=originalRole.Name,
            });
            return BadRequest();
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(RoleModel roleModel)
        {
            if (roleModel == null)  return BadRequest(AppResources.NullRole);
            var role = await _context.Roles.FirstOrDefaultAsync(role => role.Id == roleModel.Id);
            if (role == null) return BadRequest(AppResources.RoleDoesNotExist);
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) return BadRequest(AppResources.RoleDeletionImpossible);
            return Ok(AppResources.RoleDeleted);    

        }
    }
}
