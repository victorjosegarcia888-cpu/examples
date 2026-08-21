// Physics_Cooling_Active_v07.cs
//
// Active cooling physics with dynamic channel control and thermal bypass.
// Theory: Heat flux modulated by channel geometry, bypass valve for startup/shutdown.

using System;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Physics
{
    public struct CoolingPoint
    {
        public double Z;
        public double Tw;
        public double Qw;
        public double Qnorm;
        public double CoolantTemp;
        public double BypassFraction;
    }

    public class CoolingMap
    {
        public CoolingPoint[] Points;
    }

    public static class Physics_Cooling_Active_v07
    {
        public static CoolingMap Run(EngineParams p, ThermoMap thermo, double bypassFraction = 0.0)
        {
            int Nz = thermo.Points.Length;
            CoolingPoint[] pts = new CoolingPoint[Nz];

            double mdotCool = p.CoolantMassFlow;
            double cpCool = 5350.0;
            double TwInlet = p.CoolantInletTemp_C + 273.15;
            double TwMax = p.CoolantOutletTemp_C + 273.15;

            for (int i = 0; i < Nz; i++)
            {
                var t = thermo.Points[i];
                double fraction = i / (double)(Nz - 1);

                // Bypass reduces effective coolant flow
                double effectiveFlow = mdotCool * (1.0 - bypassFraction * 0.5);
                double Tw = TwInlet + (TwMax - TwInlet) * fraction;
                double Qw = t.Hg * (t.Tg - Tw);
                double deltaT = Qw / (effectiveFlow * cpCool);

                pts[i] = new CoolingPoint
                {
                    Z = t.Z,
                    Tw = Tw + deltaT,
                    Qw = Qw,
                    Qnorm = t.Qnorm,
                    CoolantTemp = TwInlet + deltaT * (i + 1),
                    BypassFraction = bypassFraction
                };
            }

            return new CoolingMap { Points = pts };
        }

        public static double ComputeBypassFraction(double throttleSetting)
        {
            // Bypass opens during low throttle to prevent overcooling
            return Math.Clamp(1.0 - throttleSetting, 0.0, 0.5);
        }
    }
}
