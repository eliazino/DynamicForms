using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces {
    public interface ITermii {
        Task<bool> sendMailOTP(string otp, string email);
    }
}
