using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SatelliteToolkit.Annotations;
using SatelliteToolkit.Model;
using Xamarin.Forms;

namespace SatelliteToolkit.ViewModel
{
    public class OrbitalToolKeplerLawsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public OrbitalToolKeplerLawsViewModel()
        {
            AngleUnit = "rad";
            CalculateCommand = new Command<Type>( (Type pageType) => Calculate());
            ToggleRadiansDegreesCommand = new Command<Type>((Type pageType) => ToggleRadiansDegrees());
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string angleUnit;

        public string AngleUnit
        {
            get => angleUnit;
            set
            {
                if (angleUnit != value)
                {
                    angleUnit = value;
                    OnPropertyChanged("AngleUnit");
                }
            }
        }

        private double? semiMajorA;
        private double? semiMinorB;
        private double? eccentricity;
        private double? trueAnomaly;
        private double? positionVector;
        private double? fociDistance;
        private double? meanMotion;
        private double? period;

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

        public double? SemiMinorB
        {
            get => semiMinorB;
            set
            {
                if (!semiMinorB.HasValue || semiMinorB.Value != value)
                {
                    semiMinorB = value;
                    OnPropertyChanged("SemiMinorB");
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

        public double? PositionVector
        {
            get => positionVector;
            set
            {
                if (!positionVector.HasValue || positionVector.Value != value)
                {
                    positionVector = value;
                    OnPropertyChanged("PositionVector");
                }
            }
        }

        public double? FociDistance
        {
            get => fociDistance;
            set
            {
                if (!fociDistance.HasValue || fociDistance.Value != value)
                {
                    fociDistance = value;
                    OnPropertyChanged("FociDistance");
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

        public double? Period
        {
            get => period;
            set
            {
                if (!period.HasValue || period.Value != value)
                {
                    period = value;
                    OnPropertyChanged("Period");
                }
            }
        }

        public ICommand CalculateCommand { get; set; }
        public ICommand ToggleRadiansDegreesCommand { get; set; }

        private void Calculate()
        {
            CheckAxis();
            CheckFociDistance();

            CheckAxis();
            CheckTrueAnomaly();

            if (Period.HasValue)
                MeanMotion = 2 * Math.PI / Period.Value;
            else if (MeanMotion.HasValue)
                Period = 2 * Math.PI / MeanMotion.Value;

            if (MeanMotion.HasValue)
                SemiMajorA = Math.Pow(Constants.EarthGravitationalConstant / Math.Pow(MeanMotion.Value, 2), 1.0 / 3);
            else if (SemiMajorA.HasValue)
            {
                MeanMotion = Math.Sqrt(Constants.EarthGravitationalConstant / Math.Pow(SemiMajorA.Value, 3));
                Period = 2 * Math.PI / MeanMotion.Value;
            }
        }

        private void ToggleRadiansDegrees()
        {

            if (TrueAnomaly.HasValue)
                TrueAnomaly = AngleUnit == "deg" ? TrueAnomaly * Math.PI / 180 : TrueAnomaly * 180 / Math.PI;

            AngleUnit = AngleUnit == "rad" ? "deg" : "rad";
        }

        private void CheckAxis()
        {
            if (SemiMinorB.HasValue && Eccentricity.HasValue && !SemiMajorA.HasValue)
                SemiMajorA = SemiMinorB / Math.Sqrt(1 - Math.Pow(Eccentricity.Value, 2));
            else if (SemiMinorB.HasValue && SemiMajorA.HasValue && !Eccentricity.HasValue)
                Eccentricity = Math.Sqrt(Math.Pow(SemiMajorA.Value, 2) - Math.Pow(SemiMinorB.Value, 2)) / SemiMajorA.Value;
        }

        private void CheckFociDistance()
        {
            if (SemiMajorA.HasValue && Eccentricity.HasValue)
            {
                if (!SemiMinorB.HasValue)
                    SemiMinorB = SemiMajorA.Value * Math.Sqrt(1 - Math.Pow(Eccentricity.Value, 2));
                FociDistance = 2 * SemiMajorA.Value * Eccentricity.Value;
            }
            else if (FociDistance.HasValue && SemiMajorA.HasValue)
            {
                Eccentricity = FociDistance.Value / (2 * SemiMajorA.Value);
                if (!SemiMinorB.HasValue)
                    SemiMinorB = SemiMajorA.Value * Math.Sqrt(1 - Math.Pow(Eccentricity.Value, 2));
            }
            else if (FociDistance.HasValue && Eccentricity.HasValue)
            {
                SemiMajorA = FociDistance.Value / (2 * Eccentricity.Value);
                if (!SemiMinorB.HasValue)
                    SemiMinorB = SemiMajorA.Value * Math.Sqrt(1 - Math.Pow(Eccentricity.Value, 2));
            }
        }

        private void CheckTrueAnomaly()
        {
            if (SemiMajorA.HasValue && Eccentricity.HasValue && TrueAnomaly.HasValue)
            {
                var angle = AngleUnit == "deg" ? TrueAnomaly.Value * Math.PI / 180 : TrueAnomaly.Value;
                PositionVector = SemiMajorA.Value * (1 - Math.Pow(Eccentricity.Value, 2)) /
                                 (1 + Eccentricity.Value * Math.Cos(angle));
            }
            else if (SemiMajorA.HasValue && Eccentricity.HasValue && PositionVector.HasValue)
            {
                var numerator = (SemiMajorA.Value * (1 - Math.Pow(Eccentricity.Value, 2))) - PositionVector.Value;
                var denominator = PositionVector.Value * Eccentricity.Value;
                var angle = Math.Acos(numerator / denominator);
                TrueAnomaly = AngleUnit == "deg" ? angle * 180 / Math.PI : angle;
            }
            else if (TrueAnomaly.HasValue && Eccentricity.HasValue && PositionVector.HasValue)
            {
                var angle = AngleUnit == "deg" ? TrueAnomaly.Value * Math.PI / 180 : TrueAnomaly.Value;
                SemiMajorA = PositionVector.Value * (1 + Eccentricity.Value * Math.Cos(angle)) /
                             (1 - Math.Pow(Eccentricity.Value, 2));
            }
        }
    }
}
