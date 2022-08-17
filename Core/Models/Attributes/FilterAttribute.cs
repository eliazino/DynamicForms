using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Attributes {
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class FilterAttribute : Attribute {
        public bool _ignore { get; private set; }

        public FilterAttribute(bool _ignore = true) {
            this._ignore = _ignore;
        }
    }
}
