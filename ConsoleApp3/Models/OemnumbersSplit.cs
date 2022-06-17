using System;
using System.Collections.Generic;

namespace ConsoleApp3.Models
{
    public partial class OemnumbersSplit
    {
        public string? ArticleNumber { get; set; }
        public long? MfrId { get; set; }
        public long? AssemblyGroupNodeId { get; set; }
        public long? LegacyArticleId { get; set; }
        public string? Lang { get; set; }
        public string? ReferenceTypeKey { get; set; }
        public string? ReferenceTypeDescription { get; set; }
    }
}
