using BooksApi.Context;
using BooksApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BooksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsuariosController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost("AsignaRolUsuario")]
        public async Task<ActionResult> AsignaRolUsuario(EditarRoleDTO editarRoleDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRoleDTO.UserId);
            if (usuario == null) { return NotFound(); }
            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRoleDTO.RoleName));
            await userManager.AddToRoleAsync(usuario, editarRoleDTO.RoleName);
            return Ok();
        }

        [HttpPost("RemoveRolUsuario")]
        public async Task<ActionResult> RemoveRolUsuario(EditarRoleDTO editarRoleDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRoleDTO.UserId);
            if (usuario == null) { return NotFound(); }
            await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRoleDTO.RoleName));
            await userManager.RemoveFromRoleAsync(usuario, editarRoleDTO.RoleName);
            return Ok();
        }
    }
}
