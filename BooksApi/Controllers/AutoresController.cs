using AutoMapper;
using BooksApi.Context;
using BooksApi.Entities;
using BooksApi.Helpers;
using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IEmailService emalService;
        private readonly ILogger<AutoresController> logger;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(ApplicationDBContext context, IEmailService emalService, ILogger<AutoresController> logger, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.emalService = emalService;
            this.logger = logger;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        [ServiceFilter(typeof(MyActionFilter))]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            logger.LogInformation("Getting authors");
            var messge = emalService.SendEmail();
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id}", Name = "ObtenerAutor")]
        public async Task<ActionResult<AutorDTO>> Get(int id)
        {
            var autor = await context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return mapper.Map<AutorDTO>(autor);
        }

        [HttpGet("Name", Name = "ObtenerAutorName")]
        public  ActionResult<string> GetName([BindRequired] string nombre) // Son QueryStrings donde nombre es requerido a webo
        {
            return configuration["MiNombre"].ToString() + " " + nombre;
        }

        [HttpGet("time")]//Sets end point https://localhost:44383/api/autores/time
        [ResponseCache(Duration =15)]
        public ActionResult<string> GetTime()
        {
            return DateTime.Now.ToString();
        }

        [HttpGet("NoAutor")]//Sets end point https://localhost:44383/api/autores/NoAutor
        public ActionResult<string> GetNoAutor()
        {
            return "No existe el autor";
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreationDTO autorCreator)
        {
            var autor = mapper.Map<Autor>(autorCreator);
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreationDTO autorActualizacion)
        {
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;

            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Autor> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                return BadRequest();
            }
            var autorDeLaDB = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autorDeLaDB == null)
            {
                return NotFound();
            }
            jsonPatchDocument.ApplyTo(autorDeLaDB, ModelState);
            var isValid = TryValidateModel(autorDeLaDB);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            //Solo retorna el campo ID, lo que hace el query más rápido
            var autorId = await context.Autores.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);

            if (autorId == default)
            {
                return NotFound();
            }
            context.Autores.Remove(new Autor { Id = autorId });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
