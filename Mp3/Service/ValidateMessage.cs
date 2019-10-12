using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Mp3.Service
{
    class ValidateMessage
    {
        public void ErrorMessage(Dictionary<string, string> errors, string field, TextBlock tb)
        {
            if (errors.ContainsKey(field))
            {
                tb.Visibility = Visibility.Visible;
                tb.Text = errors[field];
            }
            else
            {
                tb.Visibility = Visibility.Collapsed;
            }
        }
    }
}
