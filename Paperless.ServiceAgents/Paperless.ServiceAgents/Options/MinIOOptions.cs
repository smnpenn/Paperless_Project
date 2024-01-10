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
        public string AccessKey { get; } = "pdhePR6mc6WVMyLkSYvb";
        public string SecretKey { get; } = "KHFmMesygUzyhpeR2Az6M2G6gTqvLQ735bsP4I4P";
        public string Api { get; } = "s3v4";
        public string Path { get; } = "auto";
    }
}
