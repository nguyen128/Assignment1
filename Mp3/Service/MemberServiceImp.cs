using Mp3.Constant;
using Mp3.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Service
{
    class MemberServiceImp : MemberService
    {
        public Member GetInformation(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + token);
                var responseContent = client.GetAsync(ApiUrl.GET_INFORMATION_URL).Result.Content.ReadAsStringAsync().Result;
                Member mem = Newtonsoft.Json.JsonConvert.DeserializeObject<Member>(responseContent);
                return mem;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
        public string Login(string username, string password)
        {
            try
            {
                var memberLogin = new MemberLogin()
                {
                    email = username,
                    password = password
                };
                //Validate login:
                if (!ValidateMemberLogin(memberLogin))
                {
                    throw new Exception("Login fails!");
                }
                var token = GetTokenFromApi(memberLogin);
                Debug.WriteLine("Token lay trong login: "+token);

                SaveTokenToLocalStorage(token);
                return token;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
        private bool ValidateMemberLogin(MemberLogin memberLogin)
        {
            return true;
        }
        private void SaveTokenToLocalStorage(string token)
        {
            Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile tokenFile =
                 storageFolder.CreateFileAsync("token.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();
            Windows.Storage.FileIO.WriteTextAsync(tokenFile, token).GetAwaiter().GetResult();
        }
        public string ReadTokenFromLocalStorage()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile tokenFile = storageFolder.GetFileAsync("token.txt").GetAwaiter().GetResult();
                //Debug.WriteLine(tokenFile.Path);
                var token = Windows.Storage.FileIO.ReadTextAsync(tokenFile).GetAwaiter().GetResult();
                return token;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
        public Member Register(Member member)
        {
            try
            {
                var httpClient = new HttpClient();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(member), Encoding.UTF8,
                    "application/json");
                Task<HttpResponseMessage> httpRequestMessage = httpClient.PostAsync(ApiUrl.MEMBER_URL, content);
                var responseContent = httpRequestMessage.Result.Content.ReadAsStringAsync().Result;
                //Convert json to ofject C#:
                Member resMember = JsonConvert.DeserializeObject<Member>(responseContent);
                return resMember;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
        private string GetTokenFromApi(MemberLogin memberLogin)
        {
            //Thuc hien request len api lay token ve:
            var dataContent = new StringContent(JsonConvert.SerializeObject(memberLogin),
                Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var responseContent = client.PostAsync(ApiUrl.LOGIN_URL, dataContent).Result.Content.ReadAsStringAsync().Result;
            JObject jsonJObject = JObject.Parse(responseContent);
            return jsonJObject["token"].ToString();
        }
    }
}
