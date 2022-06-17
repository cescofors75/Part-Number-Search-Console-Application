using System;
using System.Collections.Generic;

namespace ConsoleApp3.Models
{
    public partial class StoreStocked
    {
        public string? RefEuro { get; set; }
        public string? StoreCountry { get; set; }
        public string? StoreName { get; set; }
        public double? Storeprice { get; set; }
        public DateTime? DateStoked { get; set; }
        public DateTime? DateUptade { get; set; }
        public int IdNew { get; set; }
    }
}
