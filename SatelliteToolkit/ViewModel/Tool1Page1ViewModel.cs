using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SatelliteToolkit.Annotations;
using Xamarin.Forms;

namespace SatelliteToolkit.ViewModel
{
    public class Tool1Page1ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Tool1Page1ViewModel()
        {
            KeplerLawsCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync("KeplerLaws"));
            ApogeePerigeeCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync("ApogeePerigee"));
            PerturbationsCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync("Perturbations"));
            TimeConvertCommand = new Command<Type>(async (Type pageType) => await NewCommandFunctionAsync("TimeConvert"));
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

        private async Task NewCommandFunctionAsync(string cmd)
        {
            await Application.Current.MainPage.DisplayAlert("ok", cmd, "ok");
        }
    }
}
