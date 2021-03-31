using AutoMapper;
using BooksApi.Context;
using BooksApi.Entities;
using BooksApi.Helpers;
using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IEmailService emalService;
        private readonly ILogger<AutoresController> logger;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDBContext context, IEmailService emalService, ILogger<AutoresController> logger, IMapper mapper)
        {
            this.context = context;
            this.emalService = emalService;
            this.logger = logger;
            this.mapper = mapper;
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
        public async Task<ActionResult<AutorDTO>> Get(int id, [BindRequired] string nombre, bool incluyeDireccion = false) // Son QueryStrings donde nombre es requerido a webo
        {
            var autor = await context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return mapper.Map<AutorDTO>(autor);
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
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest();
            }
            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }
            context.Autores.Remove(autor);
            await context.SaveChangesAsync();
            return autor;
        }
    }
}
