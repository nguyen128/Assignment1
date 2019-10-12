using Mp3.Entity;
using Mp3.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Mp3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profile : Page
    {
        MemberServiceImp memberService;
        private string loginToken;
        private Member memLogged;
        public Profile()
        {
            memberService = new MemberServiceImp();
            loginToken = memberService.ReadTokenFromLocalStorage();
            if (loginToken == null)
            {
            }
            else
            {
                this.InitializeComponent();
                memLogged = memberService.GetInformation(loginToken);
                if (memLogged.gender == 0)
                {
                    this.gender.Text = "Female";
                }else if (memLogged.gender == 1)
                {
                    this.gender.Text = "Male";
                }
                DateTime dt = DateTime.Parse(memLogged.birthday);
                this.birthday.Text = String.Format("{0:ddd, MMM d, yyyy}", dt);
            }
        }
    }
}
