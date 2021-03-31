using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Models
{
    public class LibroDTO
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "El título del libro debe ser entre {2} y {1} caracteres")]
        public string Titulo { get; set; }

        [Required]
        public int AutorId { get; set; }

        public AutorDTO Autor { get; set; }

        public string Clave { get; set; }
    }
}
