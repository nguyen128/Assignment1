
using Mp3.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Entity
{
    class Member
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string avatar { get; set; }

        public string phone { get; set; }

        public string address { get; set; }

        public string introduction { get; set; }

        public int gender { get; set; }

        public string birthday { get; set; }

        public string email { get; set; }

        public string password { get; set; }
        public string error { get; set; }
        public Dictionary<string, string> errors = new Dictionary<string, string>();
        public Dictionary<string, string> Validate()
        {
            RegexCheck regexCheck = new RegexCheck();
            //Firstname:
            if (string.IsNullOrEmpty(firstName))
            {
                errors.Add("firstName","Firstname is required!");
            }
            else if(firstName.Length > 50)
            {
                errors.Add("firstName", "Max length is 50!");
            }
            //Lastname:
            if (string.IsNullOrEmpty(lastName))
            {
                errors.Add("lastName", "Lastname is required!");
            }
            else if (lastName.Length > 50)
            {
                errors.Add("lastName", "Max length is 50!");
            }
            //Phone:
            if (string.IsNullOrEmpty(phone))
            {
                errors.Add("phone", "Phone is required!");
            }else if (!regexCheck.CheckPattern(phone, RegexCheck.phonePattern))
            {
                errors.Add("phone", "Phone is not valid!");
            }
            //Address
            if (string.IsNullOrEmpty(address))
            {
                errors.Add("address", "Address is required!");
            }
            else if (address.Length > 255)
            {
                errors.Add("address", "Max length is 255!");
            }
            //Introduction
            if (string.IsNullOrEmpty(introduction))
            {
                errors.Add("introduction", "Introduction is required!");
            }
            else if (introduction.Length > 255)
            {
                errors.Add("introduction", "Max length is 255!");
            }
            //Gender:
            if (gender != 0 && gender != 1)
            {
                errors.Add("gender", "Please choose!");
            }
            //Birthday:
            if (birthday.Equals("1601-01-01"))
            {
                errors.Add("birthday", "Please choose!");
            }
            //Password:
            if (string.IsNullOrEmpty(password))
            {
                errors.Add("password", "Password is required!");
            }
            //Email:
            if (string.IsNullOrEmpty(email))
            {
                errors.Add("email", "Email is required!");
            }else if (!regexCheck.MailCheck(email))
            {
                errors.Add("email", "Email is not valid!");
            }
            return errors;
        }
    }
}
