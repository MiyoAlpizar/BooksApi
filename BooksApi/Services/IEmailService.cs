using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Services
{
    public interface IEmailService
    {
        string SendEmail();
    }

    public class EmalService : IEmailService
    {
        private readonly ILogger<EmalService> logger;

        public EmalService(ILogger<EmalService> logger)
        {
            this.logger = logger;
        }
        public string SendEmail()
        {
            logger.LogInformation("Enviando email");
            return "Email was sended successfully";
        }
    }
}
