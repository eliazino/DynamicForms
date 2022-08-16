using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions {
    public class InputError : Exception {
        public InputError(string message) : base(message) { }
    }
}
