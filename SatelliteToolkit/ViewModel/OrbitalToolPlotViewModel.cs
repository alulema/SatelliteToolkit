using System.Collections.ObjectModel;

namespace SatelliteToolkit.ViewModel
{
    public class OrbitalToolPlotViewModel
    {
        public class Model
        {
            public double E { get; set; }

            public double Y { get; set; }

            public Model(double xValue, double yValue)
            {
                E = xValue;
                Y = yValue;
            }
        }

        public ObservableCollection<Model> Data { get; set; }

        public OrbitalToolPlotViewModel()
        {
            var incomingE = OrbitalToolOrbitalPlaneViewModel.E.ToArray<double>();
            var incomingY = OrbitalToolOrbitalPlaneViewModel.Y.ToArray<double>();

            Data = new ObservableCollection<Model>();

            for (int i = 0; i < incomingE.Length; i++)
                Data.Add(new Model(incomingE[i], incomingY[i]));
        }
    }
}
