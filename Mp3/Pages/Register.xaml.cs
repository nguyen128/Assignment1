using Mp3.Constant;
using Mp3.Entity;
using Mp3.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Mp3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        private StorageFile photo;
        private string imgUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTO6b8UmHNot4Ra90A75-m6yRyuI03Q9SgvHgyiwsxHJIXXxJcL";
        MemberService memberService;
        public Register()
        {
            this.InitializeComponent();
            this.memberService = new MemberServiceImp();

            //Combobox gender:
            Dictionary<String, int> genders = new Dictionary<string, int>();
            genders.Add("------", -1);
            genders.Add("Female", 0);
            genders.Add("Male", 1);
            this.gender.ItemsSource = genders;
            this.gender.SelectedValuePath = "Value";
            this.gender.DisplayMemberPath = "Key";
            this.gender.SelectedValue = -1;
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var errors = new Dictionary<string, string>();
            var member = new Member
            {
                firstName = this.firstname.Text,
                lastName = this.lastname.Text,
                password = this.password.Password,
                address = this.address.Text,
                avatar = imgUrl,
                birthday = this.birthday.Date.ToString("yyyy-MM-dd"),
                email = this.email.Text,
                gender = (int)this.gender.SelectedValue,
                introduction = this.introduction.Text,
                phone = this.phone.Text
            };
            Debug.WriteLine("Birthday: "+member.birthday);

            errors = member.Validate();
            if (errors.Count == 0)
            {
                var memberRes = memberService.Register(member);
                if (memberRes == null)
                {
                    //Show error
                }
                else
                {
                    //Show success
                    var token = memberService.Login(this.email.Text, this.password.Password);
                    Frame.Navigate(typeof(ListSong));
                }
            }
            else
            {
                ShowError(errors);
            }
        }
        private void ShowError(Dictionary<string, string> errors)
        {
            ValidateMessage mes = new ValidateMessage();
            mes.ErrorMessage(errors, "firstName", firstname_er);
            mes.ErrorMessage(errors, "lastName", lastname_er);
            mes.ErrorMessage(errors, "phone", phone_er);
            mes.ErrorMessage(errors, "adrress", address_er);
            mes.ErrorMessage(errors, "introduction", introduction_er);
            mes.ErrorMessage(errors, "gender", gender_er);
            mes.ErrorMessage(errors, "birthday", birthday_er);
            mes.ErrorMessage(errors, "email", email_er);
            mes.ErrorMessage(errors, "password", password_er);
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            
        }
        private async void Button_Photo(object sender, RoutedEventArgs e)
        {
            //Get upload url:
            FileServiceImp fileService = new FileServiceImp();
            var uploadUrl = fileService.GetUploadUrl(ApiUrl.GET_UPLOAD_TOKEN);

            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            this.photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }
            HttpUploadFile(uploadUrl, "myFile", "image/png");
        }
        public async void HttpUploadFile(string url, string paramName, string contentType)
        {
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
                //string imgUrl = reader2.ReadToEnd();
                //Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                //Debug.WriteLine(u.AbsoluteUri);
                //ImageUrl.Text = u.AbsoluteUri;
                //MyAvatar.Source = new BitmapImage(u);
                //Debug.WriteLine(reader2.ReadToEnd());
                string imageUrl = reader2.ReadToEnd();
                Debug.WriteLine(imageUrl);
                Avatar.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                imgUrl = imageUrl;
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
        }
    }
}
