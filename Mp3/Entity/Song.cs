using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Entity
{
    class Song
    {
        public string name { get; set; }
        public string description { get; set; }
        public string singer { get; set; }
        public string author { get; set; }
        public string thumbnail { get; set; }
        public string link { get; set; }

        public Dictionary<string, string> errors = new Dictionary<string, string>();
        public Dictionary<string, string> Validate()
        {
            if (string.IsNullOrEmpty(name))
            {
                errors.Add("name", "Name is required!");
            }
            else if(name.Length > 50)
            {
                errors.Add("name", "Max length is 50!");
            }

            if (string.IsNullOrEmpty(description))
            {
                errors.Add("description", "Description is required!");
            }
            else if (description.Length > 255)
            {
                errors.Add("description", "Max length is 255!");
            }

            if (string.IsNullOrEmpty(singer))
            {
                errors.Add("singer", "Singer is required!");
            }
            else if (singer.Length > 50)
            {
                errors.Add("singer", "Max length is 50!");
            }

            if (string.IsNullOrEmpty(author))
            {
                errors.Add("author", "Author is required!");
            }
            else if (author.Length > 50)
            {
                errors.Add("author", "Max length is 50!");
            }
            //Thumbnail:
            if (string.IsNullOrEmpty(thumbnail))
            {
                errors.Add("thumbnail", "Thumbnail is required!");
            }
            else if (!Uri.IsWellFormedUriString(thumbnail, UriKind.RelativeOrAbsolute))
            {
                errors.Add("thumbnail", "Thumbnail is not valid!");
            }
            //Link:
            if (string.IsNullOrEmpty(link))
            {
                errors.Add("link", "Link is required!");
            }
            else if (!Uri.IsWellFormedUriString(link, UriKind.RelativeOrAbsolute))
            {
                errors.Add("link", "Link is not valid!");
            }

            return errors;
        }


    }
}
