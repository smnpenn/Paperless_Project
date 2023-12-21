using Paperless.ServiceAgents.Interfaces;

namespace Paperless.OcrWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMinIOServiceAgent _minIOServiceAgent;
    private readonly IOcrServiceAgent _ocrServiceAgent;
    private readonly IElasticSearchServiceAgent _elasticSearchServiceAgent;

    public Worker(ILogger<Worker> logger, IMinIOServiceAgent minIOServiceAgent, IOcrServiceAgent ocrServiceAgent, IElasticSearchServiceAgent elasticSearchServiceAgent)
    {
        _logger = logger;
        _minIOServiceAgent = minIOServiceAgent;
        _ocrServiceAgent = ocrServiceAgent;
        _elasticSearchServiceAgent = elasticSearchServiceAgent;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                string documentName = "example.pdf";
                using (var documentStream = await _minIOServiceAgent.GetDocument(documentName))
                {
                    if(documentStream != null)
                    {
                        var ocrResult = _ocrServiceAgent.PerformOcrPdf(documentStream);
                        var documentToIndex = new
                        {
                            Name = documentName,
                            Content = ocrResult
                        };

                        await _elasticSearchServiceAgent.IndexDocumentAsync("my-index-001", documentToIndex);

                        _logger.LogInformation("Processed and indexed document {DocumentName} at: {Time}", documentName, DateTimeOffset.Now);

                    }
                }

                await Task.Delay(10000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing document");
            }

        }
    }
}
