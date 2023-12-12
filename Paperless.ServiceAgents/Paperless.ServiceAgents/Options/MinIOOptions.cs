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
        public string AccessKey { get; } = "1bAUBEXIiR4APOLEQ1ET";
        public string SecretKey { get; } = "xLR0O1LDh8fTJgUEyB1R2vsShIq2ghLAyLbLXhuZ";
        public string Api { get; } = "s3v4";
        public string Path { get; } = "auto";
    }
}
