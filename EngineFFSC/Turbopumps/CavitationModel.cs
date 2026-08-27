// CavitationModel.cs
//
// Modelo de cavitacion para turbobomba FFSC.
// Evalua el NPSH disponible vs requerido.

using System;

namespace EngineFFSC.Turbopumps
{
    public class CavitationModel
    {
        public double NPSH_Available { get; set; }
        public double NPSH_Required { get; set; }
        public double InletPressure_Pa { get; set; }
        public double VaporPressure_Pa { get; set; }
        public double FluidDensity { get; set; }
        public double InletVelocity { get; set; }
        public double SuctionSpecificSpeed { get; set; }

        public bool IsCavitating => NPSH_Available < NPSH_Required * 1.1;

        public double Margin => NPSH_Available - NPSH_Required;

        public static CavitationModel Evaluate(
            double pInlet,
            double pVapor,
            double rho,
            double velocity,
            double head,
            double rpm)
        {
            double npshAvailable = (pInlet - pVapor) / (rho * 9.81);
            double npshRequired = 3.0 + 0.002 * head / 9.81;

            double s = rpm * Math.Pow(head / 9.81, 0.75) / Math.Pow(npshAvailable, 0.75);

            return new CavitationModel
            {
                InletPressure_Pa = pInlet,
                VaporPressure_Pa = pVapor,
                FluidDensity = rho,
                InletVelocity = velocity,
                NPSH_Available = npshAvailable,
                NPSH_Required = npshRequired,
                SuctionSpecificSpeed = s
            };
        }
    }
}
