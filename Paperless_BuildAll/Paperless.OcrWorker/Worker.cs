using Paperless.BusinessLogic.Interfaces;
using Paperless.ServiceAgents.Interfaces;

namespace Paperless.OcrWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMinIOServiceAgent _minIOServiceAgent;
    private readonly IOcrServiceAgent _ocrServiceAgent;
    private readonly IElasticSearchServiceAgent _elasticSearchServiceAgent;
    private readonly IRabbitMQService _rabbitMQService;

    public Worker(ILogger<Worker> logger, IMinIOServiceAgent minIOServiceAgent, IOcrServiceAgent ocrServiceAgent, IElasticSearchServiceAgent elasticSearchServiceAgent, IRabbitMQService rabbitMQService)
    {
        _logger = logger;
        _minIOServiceAgent = minIOServiceAgent;
        _ocrServiceAgent = ocrServiceAgent;
        _elasticSearchServiceAgent = elasticSearchServiceAgent;
        _rabbitMQService = rabbitMQService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var ocrJob = _rabbitMQService.RetrieveOCRJob(); // Retrieve current OcrJob

                if(ocrJob != null)
                {
                    var documentStream = await _minIOServiceAgent.GetDocument(Path.GetFileName(ocrJob.Path));
                    if(documentStream != null)
                    {
                        try
                        {
                            var ocrResult = _ocrServiceAgent.PerformOcrPdf(documentStream);
                            var documentToIndex = new
                            {
                                Name = ocrJob.Title,
                                Content = ocrResult
                            };

                            await _elasticSearchServiceAgent.IndexDocumentAsync("paperless-index", documentToIndex);

                            _logger.LogInformation("Processed and indexed document {DocumentName} at: {Time}", ocrJob.Title, DateTimeOffset.Now);

                        }
                        finally
                        {
                            documentStream.Dispose();
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Document {DocumentName} not found or could not be opened.", ocrJob.Title);

                    }
                }
                else
                {
                    // No OCR job found, wait before checking again
                    _logger.LogInformation("No OCR jobs found in queue. Waiting for next check...");
                    await Task.Delay(10000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing OCR job");
                // Delay after an error before the next attempt
                await Task.Delay(5000, stoppingToken);
            }

        }
    }
}
