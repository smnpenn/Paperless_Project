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

namespace Paperless.ServiceAgents
{
    public class MinIOServiceAgent : IMinIOServiceAgent
    {
        IMinioClient client;
        IOptions<MinIOOptions> options;
        string bucketName = "paperless.documents";

        public MinIOServiceAgent(IOptions<MinIOOptions> options) 
        { 
            this.options = options;
            try
            {
                client = new MinioClient()
                                  .WithEndpoint("localhost", 9000)
                                  .WithCredentials(options.Value.AccessKey, options.Value.SecretKey)
                                  .WithSSL(false)
                                  .Build();
            }
            catch (Exception)
            {
                Console.WriteLine("Error while creating MinIoClient");
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
                Console.WriteLine("Successfully uploaded " + filePath);
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }
        }

        //Get Document by Objectname
        public async Task<Stream> GetDocument(string objectName)
        {
            /*
            try
            {
                var args = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFile(objectName);
                var stat = await client.GetObjectAsync(args).ConfigureAwait(false);
                Console.WriteLine($"Downloaded the file {objectName} in bucket {bucketName}");
                Console.WriteLine($"Stat details of object {objectName} in bucket {bucketName}\n" + stat);
                Console.WriteLine();
                return stat;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
                return null;
            }
            */

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
                    return memoryStream;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
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
                Console.WriteLine($"Removed object {objectName} from bucket {bucketName} successfully");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket-Object]  Exception: {e}");
            }
        }
    }
}
