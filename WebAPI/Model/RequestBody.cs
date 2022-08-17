using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model {
    public class RequestBody<T> {
        public T data { get; set; }
    }
}
