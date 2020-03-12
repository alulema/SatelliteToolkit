using System;
using System.Windows.Input;
using SatelliteToolkit.Pages;
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

        private void BtnTool_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Tool1Page1());
        }

        public ICommand Tool1Cmd { get; set; }
    }
}