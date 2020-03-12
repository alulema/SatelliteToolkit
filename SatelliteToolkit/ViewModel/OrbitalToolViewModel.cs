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
    public class OrbitalToolViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private enum OrbitOptionCommand
        {
            KeplerLaws,
            ApogeePerigee,
            Perturbations,
            TimeConvert
        }

        public OrbitalToolViewModel()
        {
            KeplerLawsCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync(OrbitOptionCommand.KeplerLaws));
            ApogeePerigeeCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync(OrbitOptionCommand.ApogeePerigee));
            PerturbationsCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync(OrbitOptionCommand.Perturbations));
            TimeConvertCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync(OrbitOptionCommand.TimeConvert));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand KeplerLawsCommand { set; get; }
        public ICommand ApogeePerigeeCommand { set; get; }
        public ICommand PerturbationsCommand { set; get; }
        public ICommand TimeConvertCommand { set; get; }

        private async Task NewCommandFunctionAsync(OrbitOptionCommand cmd)
        {
            //await Application.Current.MainPage.DisplayAlert("ok", cmd, "ok");

            switch (cmd)
            {
                case OrbitOptionCommand.KeplerLaws:
                    await Application.Current.MainPage.Navigation.PushAsync(new OrbitalToolKeplerLaws());
                    break;
            }
        }
    }
}
