using Paperless.OcrWorker;
using Paperless.ServiceAgents;
using Paperless.ServiceAgents.Interfaces;
using Paperless.ServiceAgents.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {

        //Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", "/usr/local/Cellar/leptonica/1.83.1/lib");

        Console.WriteLine("Started OCR Worker");
        // register services
        services.Configure<MinIOOptions>(hostContext.Configuration.GetSection("MinIOOptions"));
        services.AddSingleton<IMinIOServiceAgent, MinIOServiceAgent>();


        services.Configure<OcrOptions>(hostContext.Configuration.GetSection("OcrOptions"));
        services.AddSingleton<IOcrServiceAgent, OcrServiceAgent>();


        services.Configure<ElasticSearchOptions>(hostContext.Configuration.GetSection("ElasticSearchOptions"));
        services.AddSingleton<IElasticSearchServiceAgent, ElasticSearchServiceAgent>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();

