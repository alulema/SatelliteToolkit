using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using NumSharp;
using SatelliteToolkit.Annotations;
using SatelliteToolkit.Model;
using SatelliteToolkit.Pages;
using Xamarin.Forms;

namespace SatelliteToolkit.ViewModel
{
    public class OrbitalToolOrbitalPlaneViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static NDArray E;
        public static NDArray Y;

        #region Button Properties

        private string meanAnomalyUnit;
        private string eccentricAnomalyUnit;
        private string trueAnomalyUnit;
        private bool enablePlotButton;

        public string MeanAnomalyUnit
        {
            get => meanAnomalyUnit;
            set
            {
                if (meanAnomalyUnit != value)
                {
                    meanAnomalyUnit = value;
                    OnPropertyChanged("MeanAnomalyUnit");
                }
            }
        }

        public string EccentricAnomalyUnit
        {
            get => eccentricAnomalyUnit;
            set
            {
                if (eccentricAnomalyUnit != value)
                {
                    eccentricAnomalyUnit = value;
                    OnPropertyChanged("EccentricAnomalyUnit");
                }
            }
        }

        public bool EnablePlotButton
        {
            get => enablePlotButton;
            set
            {
                if (enablePlotButton != value)
                {
                    enablePlotButton = value;
                    OnPropertyChanged("EnablePlotButton");
                }
            }
        }

        public string TrueAnomalyUnit
        {
            get => trueAnomalyUnit;
            set
            {
                if (trueAnomalyUnit != value)
                {
                    trueAnomalyUnit = value;
                    OnPropertyChanged("TrueAnomalyUnit");
                }
            }
        }

        #endregion

        #region Numeric Properties

        private double? meanAnomaly;
        private double? initialTime;
        private double? elapsedTime;
        private double? eccentricAnomaly;
        private double? eccentricity;
        private double? trueAnomaly;
        private double? radiusVector;
        private double? semiMajorA;

        public double? MeanAnomaly
        {
            get => meanAnomaly;
            set
            {
                if (!meanAnomaly.HasValue || meanAnomaly.Value != value)
                {
                    meanAnomaly = value;
                    OnPropertyChanged("MeanAnomaly");
                }
            }
        }

        public double? InitialTime
        {
            get => initialTime;
            set
            {
                if (!initialTime.HasValue || initialTime.Value != value)
                {
                    initialTime = value;
                    OnPropertyChanged("InitialTime");
                }
            }
        }

        public double? ElapsedTime
        {
            get => elapsedTime;
            set
            {
                if (!elapsedTime.HasValue || elapsedTime.Value != value)
                {
                    elapsedTime = value;
                    OnPropertyChanged("ElapsedTime");
                }
            }
        }

        public double? EccentricAnomaly
        {
            get => eccentricAnomaly;
            set
            {
                if (!eccentricAnomaly.HasValue || eccentricAnomaly.Value != value)
                {
                    eccentricAnomaly = value;
                    OnPropertyChanged("EccentricAnomaly");
                }
            }
        }

        public double? Eccentricity
        {
            get => eccentricity;
            set
            {
                if (!eccentricity.HasValue || eccentricity.Value != value)
                {
                    eccentricity = value;
                    OnPropertyChanged("Eccentricity");
                }
            }
        }

        public double? TrueAnomaly
        {
            get => trueAnomaly;
            set
            {
                if (!trueAnomaly.HasValue || trueAnomaly.Value != value)
                {
                    trueAnomaly = value;
                    OnPropertyChanged("TrueAnomaly");
                }
            }
        }

        public double? RadiusVector
        {
            get => radiusVector;
            set
            {
                if (!radiusVector.HasValue || radiusVector.Value != value)
                {
                    radiusVector = value;
                    OnPropertyChanged("RadiusVector");
                }
            }
        }

        public double? SemiMajorA
        {
            get => semiMajorA;
            set
            {
                if (!semiMajorA.HasValue || semiMajorA.Value != value)
                {
                    semiMajorA = value;
                    OnPropertyChanged("SemiMajorA");
                }
            }
        }

        #endregion

        #region Commands

        public ICommand CalculateCommand { get; set; }
        public ICommand ShowPlotCommand { get; set; }
        public ICommand ToggleEccentricAnomalyUnitCommand { get; set; }
        public ICommand ToggleTrueAnomalyCommand { get; set; }
        public ICommand ToggleMeanAnomalyUnitCommand { get; set; }

        #endregion

        public OrbitalToolOrbitalPlaneViewModel()
        {
            EnablePlotButton = false;
            EccentricAnomalyUnit = "rad";
            MeanAnomalyUnit = "rad";
            TrueAnomalyUnit = "rad";
            CalculateCommand = new Command<Type>((Type pageType) => Calculate());
            ShowPlotCommand = new Command<Type>(async (Type pageType) => await ShowPlot());
            ToggleEccentricAnomalyUnitCommand = new Command<Type>((Type pageType) => ToggleEccentricAnomaly());
            ToggleMeanAnomalyUnitCommand = new Command<Type>((Type pageType) => ToggleMeanAnomaly());
            ToggleTrueAnomalyCommand = new Command<Type>((Type pageType) => ToggleTrueAnomaly());
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Calculate()
        {
            if (Eccentricity.HasValue && SemiMajorA.HasValue && ElapsedTime.HasValue && InitialTime.HasValue && MeanAnomaly.HasValue)
            {
                var rp = SemiMajorA.Value * (1 - Eccentricity.Value);
                var n = Math.Sqrt(Constants.EarthGravitationalConstant / Math.Pow(SemiMajorA.Value, 3));
                var M = MeanAnomaly.Value + n * (ElapsedTime.Value - InitialTime.Value);


                var solution = SolveKeplerEquation(M);

                EnablePlotButton = true;

                EccentricAnomaly = solution.EccentricAnomaly;
                E = solution.E;
                Y = solution.y;
            }
        }

        private (double EccentricAnomaly, NDArray E, NDArray y) SolveKeplerEquation(double M)
        {
            var E = np.linspace(0, 2 * np.pi, 2000);
            var y = M - (E - (Eccentricity.Value * np.sin(E)));

            var minValue = 1000.0;
            var selIndex = 0;
            var items = y.ToArray<double>();

            for (int i = 0; i < items.Length; i++)
            {
                var itemAbs = Math.Abs(items[i]);
                if (itemAbs < minValue)
                {
                    minValue = itemAbs;
                    selIndex = i;
                }
            }

            return (E.GetAtIndex<double>(selIndex), E, y);
        }

        private void ToggleEccentricAnomaly()
        {
            if (EccentricAnomaly.HasValue)
                EccentricAnomaly = EccentricAnomalyUnit == "deg" ? EccentricAnomaly * Math.PI / 180 : EccentricAnomaly * 180 / Math.PI;

            EccentricAnomalyUnit = EccentricAnomalyUnit == "rad" ? "deg" : "rad";
        }

        private void ToggleMeanAnomaly()
        {
            if (MeanAnomaly.HasValue)
                MeanAnomaly = MeanAnomalyUnit == "deg" ? MeanAnomaly * Math.PI / 180 : MeanAnomaly * 180 / Math.PI;

            MeanAnomalyUnit = MeanAnomalyUnit == "rad" ? "deg" : "rad";
        }

        private void ToggleTrueAnomaly()
        {
            if (TrueAnomaly.HasValue)
                TrueAnomaly = TrueAnomalyUnit == "deg" ? TrueAnomaly * Math.PI / 180 : TrueAnomaly * 180 / Math.PI;

            TrueAnomalyUnit = TrueAnomalyUnit == "rad" ? "deg" : "rad";
        }

        private async Task ShowPlot()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new OrbitalToolPlot());
        }
    }
}
