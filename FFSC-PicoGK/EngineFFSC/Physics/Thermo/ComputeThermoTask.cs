// ComputeThermoTask.cs (Physics/Thermo)
//
// Tarea termoquimica para el motor FFSC.

using System;
using System.Linq;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Physics.Thermo
{
    public struct ThermoPoint
    {
        public double Z;
        public double Tg;
        public double Hg;
        public double Qnorm;
        public double Tw;
    }

    public class ThermoMap
    {
        public ThermoPoint[] Points;
    }

    public static class ComputeThermoTask
    {
        public static ThermoMap Run(EngineParams p)
        {
            double Tad = SolveTad(p);
            int Nz = p.Nz;
            ThermoPoint[] pts = new ThermoPoint[Nz];

            for (int i = 0; i < Nz; i++)
            {
                double z = p.Lstar * (i / (double)(Nz - 1));
                double A_local = LocalArea(z, p);
                double Dt_local = Math.Sqrt(4.0 * A_local / Math.PI);
                double hg = BartzCalculator.Evaluate(p, Tad, Dt_local, A_local);

                double Tw = (p.CoolantInletTemp_C + 273.15) + 200.0 * (z / p.Lstar);

                pts[i] = new ThermoPoint
                {
                    Z = z,
                    Tg = Tad,
                    Hg = hg,
                    Qnorm = 0.0,
                    Tw = Tw
                };
            }

            double hgMin = pts.Min(t => t.Hg);
            double hgMax = pts.Max(t => t.Hg);

            for (int i = 0; i < Nz; i++)
            {
                pts[i].Qnorm = (pts[i].Hg - hgMin) / Math.Max(1e-12, hgMax - hgMin);
            }

            return new ThermoMap { Points = pts };
        }

        private static double SolveTad(EngineParams p)
        {
            double Tlow = 1500.0;
            double Thigh = 4500.0;

            for (int i = 0; i < 80; i++)
            {
                double Tmid = 0.5 * (Tlow + Thigh);
                double resid = EnergyResidual(Tmid, p);

                if (Math.Abs(resid) < 1e-8)
                    return Tmid;

                double rlow = EnergyResidual(Tlow, p);
                if (rlow * resid <= 0)
                    Thigh = Tmid;
                else
                    Tlow = Tmid;
            }

            return 0.5 * (Tlow + Thigh);
        }

        private static double EnergyResidual(double T, EngineParams p)
        {
            double hReact = -100000.0;
            double cpMean = ThermoTables.Cp_Mixture(p.MixtureRatio, T);
            double hProd = hReact + cpMean * (T - 298.15);
            return hProd - hReact + cpMean * (298.15 - p.TadInitialGuess);
        }

        private static double LocalArea(double z, EngineParams p)
        {
            double At = p.At;
            double Ae = p.Ae;

            if (z < p.ChamberLength)
            {
                return At * p.ContractionRatio;
            }
            else
            {
                double zNozzle = z - p.ChamberLength;
                double Lnozzle = p.Lstar * 0.6;
                double t = Math.Min(1.0, zNozzle / Lnozzle);
                return At + (Ae - At) * Math.Pow(t, 1.5);
            }
        }
    }
}
