using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Services
{
    public interface IWriterFile
    {
        void WriteToFile(string file, string message, bool append = true);
    }

    public class WriterFile : IWriterFile
    {
        private readonly IHostEnvironment environment;

        public WriterFile(IHostEnvironment environment)
        {
            this.environment = environment;
        }
        public async void WriteToFile(string file, string message, bool append = true)
        {
            var path = $@"{environment.ContentRootPath}\Logs\{file}.txt";
            using StreamWriter writer = new StreamWriter(path, append: append);
            await writer.WriteLineAsync(DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + ": "+ message);
        }
    }
}
