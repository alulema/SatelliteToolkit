using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SatelliteToolkit.Annotations;
using SatelliteToolkit.Pages;
using Xamarin.Forms;

namespace SatelliteToolkit.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private enum ToolOption
        {
            OrbitalTool,
            NotImplemented
        }

        public HomeViewModel()
        {
            OrbitalToolCommand = new Command<Type>(async (Type pageType) => await HomeViewCommandAsync(ToolOption.OrbitalTool));
            NotImplementedCommand = new Command<Type>(async (Type pageType) => await HomeViewCommandAsync(ToolOption.NotImplemented));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand OrbitalToolCommand { get; set; }
        public ICommand NotImplementedCommand { get; set; }

        private async Task HomeViewCommandAsync(ToolOption cmd)
        {
            switch (cmd)
            {
                case ToolOption.OrbitalTool:
                    await Application.Current.MainPage.Navigation.PushAsync(new OrbitalTool());
                    break;
                case ToolOption.NotImplemented:
                    await Application.Current.MainPage.DisplayAlert("Sorry!", "Not implemented yet.", "Ok");
                    break;
            }
        }

    }
}
