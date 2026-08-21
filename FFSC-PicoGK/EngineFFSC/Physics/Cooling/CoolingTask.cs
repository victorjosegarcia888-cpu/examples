// CoolingTask.cs
//
// Tarea de refrigeracion regenerativa del motor FFSC.

using System;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;

namespace FFSC_PicoGK.Physics.Cooling
{
    public struct CoolingPoint
    {
        public double Z;
        public double Tw;
        public double Qw;
        public double Qnorm;
        public double CoolantTemp;
    }

    public class CoolingMap
    {
        public required CoolingPoint[] Points;
    }

    public static class CoolingTask
    {
        public static CoolingMap Run(EngineParams p, ThermoMap thermo)
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

                double Tw = TwInlet + (TwMax - TwInlet) * fraction;
                double Qw = t.Hg * (t.Tg - Tw);
                double deltaT = Qw / (mdotCool * cpCool);

                pts[i] = new CoolingPoint
                {
                    Z = t.Z,
                    Tw = Tw + deltaT,
                    Qw = Qw,
                    Qnorm = t.Qnorm,
                    CoolantTemp = TwInlet + deltaT * (i + 1)
                };
            }

            return new CoolingMap { Points = pts };
        }
    }
}
