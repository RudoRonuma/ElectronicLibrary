using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.Utils.Validation
{
    public static class EmailValidation
    {
        private const string EMAIL_PATTERN = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
        public static bool IsEmailValid(string email) =>
            Regex.Match(email, EMAIL_PATTERN).Success;
    }
}
