using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SatelliteToolkit.Pages;
using Xamarin.Forms;

namespace SatelliteToolkit.ViewModel
{
    public class OrbitalToolViewModel
    {
        private enum OrbitOptionCommand
        {
            KeplerLaws,
            ApogeePerigee,
            Perturbations,
            OrbitalPlane
        }

        public OrbitalToolViewModel()
        {
            KeplerLawsCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync(OrbitOptionCommand.KeplerLaws));
            ApogeePerigeeCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync(OrbitOptionCommand.ApogeePerigee));
            PerturbationsCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync(OrbitOptionCommand.Perturbations));
            OrbitalPlaneCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync(OrbitOptionCommand.OrbitalPlane));
        }

        public ICommand KeplerLawsCommand { set; get; }
        public ICommand ApogeePerigeeCommand { set; get; }
        public ICommand PerturbationsCommand { set; get; }
        public ICommand TimeConvertCommand { set; get; }
        public ICommand OrbitalPlaneCommand { set; get; }

        private async Task NewCommandFunctionAsync(OrbitOptionCommand cmd)
        {
            switch (cmd)
            {
                case OrbitOptionCommand.KeplerLaws:
                    await Application.Current.MainPage.Navigation.PushAsync(new OrbitalToolKeplerLaws());
                    break;
                case OrbitOptionCommand.ApogeePerigee:
                    await Application.Current.MainPage.Navigation.PushAsync(new OrbitalToolApogeePerigee());
                    break;
                case OrbitOptionCommand.Perturbations:
                    await Application.Current.MainPage.Navigation.PushAsync(new OrbitalToolPerturbations());
                    break;
                case OrbitOptionCommand.OrbitalPlane:
                    await Application.Current.MainPage.Navigation.PushAsync(new OrbitalToolOrbitalPlane());
                    break;
            }
        }
    }
}
