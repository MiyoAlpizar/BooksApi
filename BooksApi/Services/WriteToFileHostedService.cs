using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BooksApi.Services
{
    public class WriteToFileHostedService : IHostedService
    {
        private readonly IWriterFile writer;

        public WriteToFileHostedService(IWriterFile writer)
        {
            this.writer = writer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            writer.WriteToFile("server", "Iniciando servidor");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            writer.WriteToFile("server", "terminando servidor");
            return Task.CompletedTask;
        }

        private void WriteToFile(string message)
        {

        }
    }
}
