// Physics_Turbopump_Triple_v07.cs
//
// Triple turbopump physics: LOX, CH4, and preburner turbine performance.
// Theory: Euler turbomachinery equation with slip factor and Reynolds corrections.

using System;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Physics
{
    public struct TurbopumpPoint
    {
        public string Name;
        public double Head;
        public double Power;
        public double Efficiency;
        public double RPM;
        public double FlowRate;
    }

    public static class Physics_Turbopump_Triple_v07
    {
        public static TurbopumpPoint[] Run(EngineParams p)
        {
            double rho_LOX = 1140.0;
            double rho_CH4 = 422.0;
            double rpm = p.TurbopumpRPM;

            // LOX pump
            double qLOX = p.MassFlowOxidizer / rho_LOX;
            double headLOX = EulerHead(rpm, 0.16, 0.05, 10);
            double powerLOX = rho_LOX * g * qLOX * headLOX / 0.75;
            double effLOX = 0.75 - 0.05 * Math.Log10(rpm / 30000.0);

            // CH4 pump
            double qCH4 = p.MassFlowFuel / rho_CH4;
            double headCH4 = EulerHead(rpm, 0.16, 0.05, 10) * 1.1;
            double powerCH4 = rho_CH4 * g * qCH4 * headCH4 / 0.72;
            double effCH4 = 0.72 - 0.04 * Math.Log10(rpm / 30000.0);

            // Preburner turbine
            double headTurbine = EulerHead(rpm, 0.18, 0.05, 12) * 0.9;
            double powerTurbine = 850000.0;
            double effTurbine = 0.85 - 0.03 * Math.Log10(rpm / 30000.0);

            return new[]
            {
                new TurbopumpPoint { Name = "LOX_Pump", Head = headLOX, Power = powerLOX, Efficiency = effLOX, RPM = rpm, FlowRate = qLOX },
                new TurbopumpPoint { Name = "CH4_Pump", Head = headCH4, Power = powerCH4, Efficiency = effCH4, RPM = rpm, FlowRate = qCH4 },
                new TurbopumpPoint { Name = "Preburner_Turbine", Head = headTurbine, Power = powerTurbine, Efficiency = effTurbine, RPM = rpm * 1.05, FlowRate = 0.0 }
            };
        }

        private static double EulerHead(double rpm, double rotorRadius, double hubRadius, int bladeCount)
        {
            double u = 2.0 * Math.PI * rpm / 60.0 * rotorRadius;
            double slip = 1.0 - 0.6 / bladeCount;
            return u * u * slip / (2.0 * g);
        }

        private const double g = 9.80665;
    }
}
