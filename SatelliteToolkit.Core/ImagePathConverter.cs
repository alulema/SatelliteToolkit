using System;
using System.Globalization;
using Xamarin.Forms;

namespace SatelliteToolkit.Core
{
    public class ImagePathConverter : IValueConverter
    {
        public static string GetImageSource(string resourceName)
        {
            string[] strArray = resourceName.Split('.');
            if (strArray.Length == 0)
                throw new ArgumentException("The provided resource name is invalid. Example, SampleBrowser.SfChart.Button.png");
            string str1 = Device.RuntimePlatform == "WPF" ? ".WPF" : ".UWP";
            string str2 = strArray[0] + "." + strArray[1] + str1;
            string str3 = strArray[2] + "." + strArray[3];
            string str4 = "pack://application:,,,/" + str2 + ";component/" + str3;
            return str3;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter != null ? (object)ImagePathConverter.GetImageSource(parameter.ToString()) : (object)ImagePathConverter.GetImageSource(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
