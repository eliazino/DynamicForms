using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.DTOs.Request {
    public class DataFilterDTO {
        private List<FilterElement> _filterObj;
        public string schemaID { get; set; }
        public List<FilterElement> filterBodyObj { get; set; }
        public List<KeyValuePair<string, dynamic>> filterBody { 
            get {
                if (filterBodyObj == null)
                    return new List<KeyValuePair<string, dynamic>>();
                if (this.filterBodyObj.Count < 1)
                    return new List<KeyValuePair<string, dynamic>>();
                var t = new List<KeyValuePair<string, dynamic>>();
                foreach(FilterElement filter in this.filterBodyObj) {
                    t.Add(new KeyValuePair<string, dynamic>(filter.fieldID, filter.value));
                }
                return t;
            }  
        }
    }

    public class FilterElement {
        public string fieldID { get; set; }
        public string value { get; set; } 
    }
}
