using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Service
{
    interface FileService
    {
        String GetUploadUrl(String uploadToken);
        Task<string> GetUploadFile(String url, String paramName, String contentType);
    }
}
