using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Mp3.Service
{
    class FileServiceImp : FileService
    {
        private StorageFile photo;
        public async Task<string> GetUploadFile(String url, String paramName, String contentType)
        {
            string imgUrl="";
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";

            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            // write file.
            Stream fileStream = await this.photo.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                //imgUrl = reader2.ReadToEnd();
                //Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                //Debug.WriteLine(imgUrl);
                //ImageUrl.Text = u.AbsoluteUri;
                //MyAvatar.Source = new BitmapImage(u);
                //Debug.WriteLine(reader2.ReadToEnd());
                imgUrl = reader2.ReadToEnd();
                Debug.WriteLine("Link anh: "+imgUrl);
                //Avatar.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                //AvatarUrl.Text = imageUrl;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex.StackTrace);
                Debug.WriteLine("Error uploading file", ex.InnerException);
                if (wresp != null)
                {
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
            return imgUrl;
        }

        public string GetUploadUrl(String uploadToken)
        {
            HttpClient client = new HttpClient();
            var uploadUrl = client.GetAsync(uploadToken).Result.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(uploadUrl);
            return uploadUrl;
        }
        
    }
}
