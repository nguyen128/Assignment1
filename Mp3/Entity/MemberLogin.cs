using Mp3.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Entity
{
    class MemberLogin
    {
        public string email { get; set; }
        public string password { get; set; }

        public Dictionary<string, string> errors = new Dictionary<string, string>();
        public Dictionary<string, string> Validate()
        {
            RegexCheck regexCheck = new RegexCheck();
            //Email:
            if (string.IsNullOrEmpty(email))
            {
                errors.Add("email", "Email is required!");
            }
            else if (!regexCheck.MailCheck(email))
            {
                errors.Add("email", "Email is not valid!");
            }
            //Password:
            if (string.IsNullOrEmpty(password))
            {
                errors.Add("password", "Password is required!");
            }
            return errors;
        }
    }
}
