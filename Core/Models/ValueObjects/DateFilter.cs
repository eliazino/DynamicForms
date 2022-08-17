using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.ValueObjects {
    public class DateFilter {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
    }
}
