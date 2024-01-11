using Paperless.OcrWorker;
using Paperless.ServiceAgents;
using Paperless.ServiceAgents.Interfaces;
using Paperless.ServiceAgents.Options;
using Microsoft.Extensions.Configuration;

using dotenv.net;
using Paperless.BusinessLogic.Interfaces;
using Paperless.BusinessLogic;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;
using Paperless.DAL.Sql;
using Paperless.DAL.Interfaces;

DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { "../../.env" }));

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) => {
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {


        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddEnvironmentVariables();

        IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

        var repo = new Repository(configuration, "TestDbContext");

        services.AddSingleton<IDocumentRepository>(repo);


        // Rabbit MQ Settings

        var rabbitMQConfig = hostContext.Configuration.GetSection("RabbitMQ");

        var connectionFactory = new ConnectionFactory()
        {
            HostName = rabbitMQConfig["HostName"],
            Port = 5672,
            UserName = rabbitMQConfig["UserName"],
            Password = rabbitMQConfig["Password"],
        };

        services.AddSingleton<ConnectionFactory>(connectionFactory);

        services.AddSingleton<IRabbitMQService>(sp =>
            new RabbitMQService(
                sp.GetRequiredService<ConnectionFactory>(),
                rabbitMQConfig["QueueName"] ?? "TestQueue"
            )
        );



        Console.WriteLine("Started OCR Worker");

        services.AddSingleton<IConfiguration>(configuration);


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

