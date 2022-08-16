using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core.Application.DTOs.Filter {
    public class Filter {
        protected Dictionary<string, object> reference = new Dictionary<string, object>();
        protected string GetCallerName([CallerMemberName] string name = null) {
            return name;
        }

        protected object getField(string field) {
            try {
                return reference[field];
            } catch {
                throw new Exception("Field was not set");
            }
        }

        public bool fieldIsSet(string field) {
            object value;
            if (reference.TryGetValue(field, out value)) {
                return true;
            }
            return false;
        }
    }
}
