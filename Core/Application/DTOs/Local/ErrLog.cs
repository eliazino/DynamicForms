using Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.DTOs.Local {
    public class ErrLogDTO {
        public string details { get; set; }
        public long transDate { get; } = Utilities.getTodayDate().unixTimestamp;
        public string route { get; set; }
        public string username { get; set; }
        public string request { get; set; }
    }
}
