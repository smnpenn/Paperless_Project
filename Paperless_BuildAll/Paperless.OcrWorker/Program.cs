using Paperless.OcrWorker;
using Paperless.ServiceAgents;
using Paperless.ServiceAgents.Interfaces;
using Paperless.ServiceAgents.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // register services
        services.Configure<MinIOOptions>(hostContext.Configuration.GetSection("MinIOOptions"));
        services.AddSingleton<IMinIOServiceAgent, MinIOServiceAgent>();


        services.Configure<OcrOptions>(hostContext.Configuration.GetSection("OcrOptions"));
        services.AddSingleton<IOcrServiceAgent, OcrServiceAgent>();

        services.AddSingleton<IElasticSearchServiceAgent, ElasticSearchServiceAgent>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();

