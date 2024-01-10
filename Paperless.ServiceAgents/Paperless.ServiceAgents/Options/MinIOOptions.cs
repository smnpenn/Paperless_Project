using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Paperless.ServiceAgents.Options
{
    public class MinIOOptions
    {
        public const string MinIO = "MinIO";
        public string Url { get; } = "http://localhost:9001";
        public string AccessKey { get; } = "y6Um1dFTvixLhiLnq1B1";
        public string SecretKey { get; } = "hERDACpOtYH0ba8MCQjQGARExz2Nl6CZNADa0pFI";
        public string Api { get; } = "s3v4";
        public string Path { get; } = "auto";
    }
}
