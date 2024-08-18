using MetaDataParser.Services;
using Microsoft.Extensions.DependencyInjection;
namespace MetaDataParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Compiler data refresh.");
            if (args.Length == 0)
            {
                Console.WriteLine("Missing parameter [path] e.g. \"D:\\\\devl\\\\zx\\\\proj\\\\ZXGame\\\\2.source\\\\data\"");
                Environment.ExitCode = -1;
                return;
            }
            if (!Path.Exists(args[0]))
            {
                Console.WriteLine("Path is invalid or Missing. " + args[0]);
                Environment.ExitCode = -2;
                return;
            }
            Console.WriteLine("path " + args[0]);
            var services = new ServiceCollection()
            .AddSingleton<IScanProject, ScanProject>()
            .AddSingleton<IServiceMetaData, ServiceMetaData>()
            .BuildServiceProvider();
            var scan = services.GetRequiredService<IScanProject>();
            scan.Scan(args[0]);
            Console.WriteLine("Finished refresh");
            Environment.ExitCode = 0;
        }
    }
}
