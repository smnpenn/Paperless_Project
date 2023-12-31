﻿using Microsoft.Extensions.Primitives;
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
        public string AccessKey { get; } = "M7GLRFiFh6TwP0N71gGj";
        public string SecretKey { get; } = "3mkdSof4u8UzBg8uQR3JAJIiToCTHos8aWzy9Kg5";
        public string Api { get; } = "s3v4";
        public string Path { get; } = "auto";
    }
}
