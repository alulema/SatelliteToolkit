using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SatelliteToolkit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            Console.Write("OK");
        }
    }
}