using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Entities
{
    public class Autor
    {
        public int Id { get; set; }
       
        public string Nombre { get; set; }

        public List<Libro> Libros { get; set; }
    }
}
