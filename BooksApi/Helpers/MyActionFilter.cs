using BooksApi.Context;
using BooksApi.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Helpers
{
    public class MyActionFilter : IActionFilter
    {
        private readonly ApplicationDBContext dBContext;

        public MyActionFilter(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async void OnActionExecuted(ActionExecutedContext context)
        {
            var book = await dBContext.Autores.FirstOrDefaultAsync(x => x.Id == 1);
            var controller = (AutoresController)context.Controller;
            if (book == null)
            {
                context.Result = controller.Redirect("autores/NoAutor");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
