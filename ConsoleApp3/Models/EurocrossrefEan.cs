using System;
using System.Collections.Generic;

namespace ConsoleApp3.Models
{
    public partial class EurocrossrefEan
    {
        public int? Id { get; set; }
        public string? RefEuro { get; set; }
        public string? RefFrn { get; set; }
        public string? ArticleNumber { get; set; }
        public int? MfrId { get; set; }
        public int? AssemblyGroupNodeId { get; set; }
        public int? LegacyArticleId { get; set; }
        public string? Lang { get; set; }
        public string? ReferenceTypeKey { get; set; }
        public string? ReferenceTypeDescription { get; set; }
        public int? LegacyArticleId2 { get; set; }
        public double? Eancode { get; set; }
    }
}
