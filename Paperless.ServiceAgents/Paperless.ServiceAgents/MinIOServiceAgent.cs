using Microsoft.Extensions.Options;
using Paperless.ServiceAgents.Interfaces;
using Paperless.ServiceAgents.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Globalization;
using Minio.DataModel;
using Microsoft.Extensions.Logging;
using Paperless.ServiceAgents.Exceptions;

namespace Paperless.ServiceAgents
{
    public class MinIOServiceAgent : IMinIOServiceAgent
    {
        IMinioClient client;
        IOptions<MinIOOptions> options;
        string bucketName = "paperless.documents";

        private readonly ILogger<MinIOServiceAgent> _logger;

        public MinIOServiceAgent(IOptions<MinIOOptions> options, ILogger<MinIOServiceAgent> logger) 
        { 
            this.options = options;
            _logger = logger;
            try
            {
                client = new MinioClient()
                                  .WithEndpoint("localhost", 9000)
                                  .WithCredentials(options.Value.AccessKey, options.Value.SecretKey)
                                  .WithSSL(false)
                                  .Build();
                _logger.LogInformation("Successfully created MinIOClient");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while creating MinIoClient");
                throw new MinIOServiceAgentException("Error while creating MinIOClient", ex);
            }
           
        }

        //Uploads document in bucket paperless.documents
        //Updates document if fileName already exists in minio
        public async Task UploadDocument(string filePath, string fileName)
        {
            string contentType = "application/pdf";
            try
            {
                //filePath = "C:/Users/Simon/source/repos/SWKOM/Paperless_Project/Swagger.RestService/src/IO.Swagger/" + filePath;
                // Make a bucket on the server, if not already present.
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await client.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await client.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                // Upload a file to bucket.
                var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                    .WithFileName(filePath)
                    .WithObject(fileName + ".pdf")
                    .WithContentType(contentType);
                await client.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                _logger.LogInformation("Successfully uploaded " + filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError("File Upload Error");
                throw new MinIOServiceAgentException("File Upload Error", ex);
            }
        }

        //Get Document by Objectname
        public async Task<Stream> GetDocument(string objectName)
        {

            try
            {
                var statArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);
                var objectStat = await client.StatObjectAsync(statArgs).ConfigureAwait(false);

                if (objectStat != null)
                {
                    var memoryStream = new MemoryStream();
                    var getArgs = new GetObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        .WithCallbackStream(s => s.CopyTo(memoryStream));

                    await client.GetObjectAsync(getArgs).ConfigureAwait(false);

                    memoryStream.Position = 0;
                    _logger.LogInformation("Fetching document was successful");
                    return memoryStream;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[Bucket]  Exception: {e}");
                throw new MinIOServiceAgentException("Getting document failed", e);
            }

            return null;
        }

        //Deletes document in minIO
        public async Task DeleteDocument(string objectName)
        {
            try
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);

                await client.RemoveObjectAsync(args).ConfigureAwait(false);
                _logger.LogInformation($"Removed object {objectName} from bucket {bucketName} successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"[Bucket-Object]  Exception: {e}");
                throw new MinIOServiceAgentException("Deleting document failed", e);

            }
        }
    }
}
