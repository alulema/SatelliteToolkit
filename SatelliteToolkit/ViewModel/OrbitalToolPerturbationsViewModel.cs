using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SatelliteToolkit.Annotations;
using SatelliteToolkit.Model;
using Xamarin.Forms;

namespace SatelliteToolkit.ViewModel
{
    public class OrbitalToolPerturbationsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Unit Button Properties

        private string inclinationUnit;
        private string rateNodesUnit;
        private string rateRotationUnit;

        public string InclinationUnit
        {
            get => inclinationUnit;
            set
            {
                if (inclinationUnit != value)
                {
                    inclinationUnit = value;
                    OnPropertyChanged("InclinationUnit");
                }
            }
        }

        public string RateNodesUnit
        {
            get => rateNodesUnit;
            set
            {
                if (rateNodesUnit != value)
                {
                    rateNodesUnit = value;
                    OnPropertyChanged("RateNodesUnit");
                }
            }
        }

        public string RateRotationUnit
        {
            get => rateRotationUnit;
            set
            {
                if (rateRotationUnit != value)
                {
                    rateRotationUnit = value;
                    OnPropertyChanged("RateRotationUnit");
                }
            }
        }

        #endregion

        #region Numeric Properties

        private double? nominalMeanMotion;
        private double? semiMajorA;
        private double? inclination;
        private double? rateRegressionNodes;
        private double? rateRotationLineApsides;
        private double? apogeeHeight;
        private double? perigeeHeight;
        private double? radiusApogee;
        private double? radiusPerigee;
        private double? eccentricity;

        public double? NominalMeanMotion
        {
            get => nominalMeanMotion;
            set
            {
                if (!nominalMeanMotion.HasValue || nominalMeanMotion.Value != value)
                {
                    nominalMeanMotion = value;
                    OnPropertyChanged("NominalMeanMotion");
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

        public double? Inclination
        {
            get => inclination;
            set
            {
                if (!inclination.HasValue || inclination.Value != value)
                {
                    inclination = value;
                    OnPropertyChanged("Inclination");
                }
            }
        }

        public double? RateRegressionNodes
        {
            get => rateRegressionNodes;
            set
            {
                if (!rateRegressionNodes.HasValue || rateRegressionNodes.Value != value)
                {
                    rateRegressionNodes = value;
                    OnPropertyChanged("RateRegressionNodes");
                }
            }
        }

        public double? RateRotationLineApsides
        {
            get => rateRotationLineApsides;
            set
            {
                if (!rateRotationLineApsides.HasValue || rateRotationLineApsides.Value != value)
                {
                    rateRotationLineApsides = value;
                    OnPropertyChanged("RateRotationLineApsides");
                }
            }
        }

        public double? ApogeeHeight
        {
            get => apogeeHeight;
            set
            {
                if (!apogeeHeight.HasValue || apogeeHeight.Value != value)
                {
                    apogeeHeight = value;
                    OnPropertyChanged("ApogeeHeight");
                }
            }
        }

        public double? PerigeeHeight
        {
            get => perigeeHeight;
            set
            {
                if (!perigeeHeight.HasValue || perigeeHeight.Value != value)
                {
                    perigeeHeight = value;
                    OnPropertyChanged("PerigeeHeight");
                }
            }
        }

        public double? RadiusApogee
        {
            get => radiusApogee;
            set
            {
                if (!radiusApogee.HasValue || radiusApogee.Value != value)
                {
                    radiusApogee = value;
                    OnPropertyChanged("RadiusApogee");
                }
            }
        }

        public double? RadiusPerigee
        {
            get => radiusPerigee;
            set
            {
                if (!radiusPerigee.HasValue || radiusPerigee.Value != value)
                {
                    radiusPerigee = value;
                    OnPropertyChanged("RadiusPerigee");
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

        #endregion

        #region Commands

        public ICommand CalculateCommand { get; set; }
        public ICommand ToggleInclinationUnitCommand { get; set; }
        public ICommand ToggleRateNodesUnitCommand { get; set; }
        public ICommand ToggleRateRotationUnitCommand { get; set; }

        #endregion

        public OrbitalToolPerturbationsViewModel()
        {
            InclinationUnit = "rad";
            RateNodesUnit = "rd/dy";
            RateRotationUnit = "rd/dy";
            CalculateCommand = new Command<Type>((Type pageType) => Calculate());
            ToggleInclinationUnitCommand = new Command<Type>((Type pageType) => ToggleInclination());
            ToggleRateNodesUnitCommand = new Command<Type>((Type pageType) => ToggleRateNodes());
            ToggleRateRotationUnitCommand = new Command<Type>((Type pageType) => ToggleRateRotation());
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Calculate()
        {
            if (ApogeeHeight.HasValue && PerigeeHeight.HasValue)
            {
                RadiusApogee = ApogeeHeight.Value + Constants.EarthRadius;
                RadiusPerigee = PerigeeHeight.Value + Constants.EarthRadius;

                Eccentricity = (RadiusApogee.Value - RadiusPerigee.Value) / (RadiusApogee.Value + RadiusPerigee.Value);
                SemiMajorA = RadiusApogee / (1 + eccentricity);
            }
            else if (RadiusApogee.HasValue && RadiusPerigee.HasValue)
            {
                ApogeeHeight = RadiusApogee.Value - Constants.EarthRadius;
                PerigeeHeight = RadiusPerigee.Value - Constants.EarthRadius;

                Eccentricity = (RadiusApogee.Value - RadiusPerigee.Value) / (RadiusApogee.Value + RadiusPerigee.Value);
                SemiMajorA = RadiusApogee / (1 + eccentricity);
            }

            if (ApogeeHeight.HasValue && PerigeeHeight.HasValue && Inclination.HasValue)
            {
                SemiMajorA = (RadiusApogee - RadiusPerigee) / 2;
                NominalMeanMotion = Math.Sqrt(Constants.EarthGravitationalConstant / Math.Pow(SemiMajorA.Value * 1000, 3));

                var K = NominalMeanMotion * Constants.K1 / Math.Pow(SemiMajorA.Value * (1 - Math.Pow(Eccentricity.Value, 2)), 2);
                RateRegressionNodes = K * (-1) * Math.Cos(Inclination.Value);
                RateRotationLineApsides = K * (2 - 2.5 * Math.Pow(Math.Sin(Inclination.Value), 2));
            }
        }

        private void ToggleRateRotation()
        {
            if (RateRotationLineApsides.HasValue)
                RateRotationLineApsides = RateRotationUnit == "dg/dy" ? RateRotationLineApsides * Math.PI / 180 : RateRotationLineApsides * 180 / Math.PI;

            RateRotationUnit = RateRotationUnit == "rd/dy" ? "dg/dy" : "rd/dy";
        }

        private void ToggleRateNodes()
        {
            if (RateRegressionNodes.HasValue)
                RateRegressionNodes = RateNodesUnit == "dg/dy" ? RateRegressionNodes * Math.PI / 180 : RateRegressionNodes * 180 / Math.PI;

            RateNodesUnit = RateNodesUnit == "rd/dy" ? "dg/dy" : "rd/dy";
        }

        private void ToggleInclination()
        {
            if (Inclination.HasValue)
                Inclination = InclinationUnit == "deg" ? Inclination * Math.PI / 180 : Inclination * 180 / Math.PI;

            InclinationUnit = InclinationUnit == "rad" ? "deg" : "rad";
        }
    }
}
