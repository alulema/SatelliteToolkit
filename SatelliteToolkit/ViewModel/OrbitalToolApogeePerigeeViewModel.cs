using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SatelliteToolkit.Annotations;
using SatelliteToolkit.Model;
using Xamarin.Forms;

namespace SatelliteToolkit.ViewModel
{
    public class OrbitalToolApogeePerigeeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Numeric Properties

        private double? semiMajorA;
        private double? meanMotion;
        private double? apogeeHeight;
        private double? perigeeHeight;
        private double? radiusApogee;
        private double? radiusPerigee;
        private double? eccentricity;

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

        public double? MeanMotion
        {
            get => meanMotion;
            set
            {
                if (!meanMotion.HasValue || meanMotion.Value != value)
                {
                    meanMotion = value;
                    OnPropertyChanged("MeanMotion");
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

        #endregion

        public OrbitalToolApogeePerigeeViewModel()
        {
            CalculateCommand = new Command<Type>((Type pageType) => Calculate());
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Calculate()
        {
            CheckMeanMotion();

            if (Eccentricity.HasValue && SemiMajorA.HasValue)
            {
                RadiusApogee = SemiMajorA.Value * (1 + Eccentricity.Value);
                ApogeeHeight = RadiusApogee.Value - Constants.EarthRadius;

                RadiusPerigee = SemiMajorA.Value * (1 - Eccentricity.Value);
                PerigeeHeight = RadiusPerigee.Value - Constants.EarthRadius;
            }
            else if (ApogeeHeight.HasValue && PerigeeHeight.HasValue)
            {
                RadiusApogee = ApogeeHeight.Value + Constants.EarthRadius;
                RadiusPerigee = PerigeeHeight.Value + Constants.EarthRadius;

                Eccentricity = (RadiusApogee.Value - RadiusPerigee.Value) / (RadiusApogee.Value + RadiusPerigee.Value);
                SemiMajorA = RadiusApogee / (1 + Eccentricity.Value);
            }
            else if (RadiusApogee.HasValue && RadiusPerigee.HasValue)
            {
                ApogeeHeight = RadiusApogee.Value - Constants.EarthRadius;
                PerigeeHeight = RadiusPerigee.Value - Constants.EarthRadius;

                Eccentricity = (RadiusApogee.Value - RadiusPerigee.Value) / (RadiusApogee.Value + RadiusPerigee.Value);
                SemiMajorA = RadiusApogee / (1 + Eccentricity.Value);
            }

            CheckMeanMotion();
        }

        private void CheckMeanMotion()
        {
            if (MeanMotion.HasValue && !SemiMajorA.HasValue)
                SemiMajorA = Math.Pow(Constants.EarthGravitationalConstant / Math.Pow(MeanMotion.Value, 2), 1.0 / 3);
            else if (SemiMajorA.HasValue && !MeanMotion.HasValue)
            {
                var test = Math.Sqrt(Constants.EarthGravitationalConstant / Math.Pow(SemiMajorA.Value * 1000, 3));
                MeanMotion = Math.Sqrt(Constants.EarthGravitationalConstant / Math.Pow(SemiMajorA.Value * 1000, 3));
            }
        }
    }
}
