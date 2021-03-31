using BooksApi.Entities;
using BooksApi.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Models
{
    public class AutorDTO
    {
        public int Id { get; set; }

        [Required]
        [PrimeraMayuscula]
        public string Nombre { get; set; }

        public List<LibroDTO> Libros { get; set; }

        public string Clave { get; set; }
    }
}
