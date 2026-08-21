// ComputeThicknessTask.cs
//
// Calculo de espesor estructural.

using System;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;

namespace FFSC_PicoGK.Physics.Structural
{
    public struct ThicknessPoint
    {
        public double Z;
        public double Radius;
        public double Thickness;
    }

    public class ThicknessMap
    {
        public ThicknessPoint[] Points;
    }

    public static class ComputeThicknessTask
    {
        public static ThicknessMap Run(EngineParams p, ThermoMap thermo)
        {
            double FS = p.SafetyFactor;
            double sigmaAllow = p.Material.YieldStrengthPa / FS;

            ThicknessPoint[] pts = new ThicknessPoint[thermo.Points.Length];

            for (int i = 0; i < thermo.Points.Length; i++)
            {
                var t = thermo.Points[i];
                double radius = LocalRadius(t.Z, p);

                double tBarlow = (p.Pc * radius) / sigmaAllow;
                double thermalFactor = 1.0 + 2.0 * t.Qnorm;
                double tempDegradation = 1.0 + 0.5 * Math.Max(0, (t.Tw - 600.0) / 600.0);
                double thickness = tBarlow * thermalFactor * tempDegradation;

                pts[i] = new ThicknessPoint
                {
                    Z = t.Z,
                    Radius = radius,
                    Thickness = thickness
                };
            }

            return new ThicknessMap { Points = pts };
        }

        private static double LocalRadius(double z, EngineParams p)
        {
            double At = p.At;
            double Ae = p.Ae;

            if (z < p.ChamberLength)
            {
                return Math.Sqrt(At * p.ContractionRatio / Math.PI);
            }
            else
            {
                double zNozzle = z - p.ChamberLength;
                double Lnozzle = p.Lstar * 0.6;
                double t = Math.Min(1.0, zNozzle / Lnozzle);
                double A = At + (Ae - At) * Math.Pow(t, 1.5);
                return Math.Sqrt(A / Math.PI);
            }
        }
    }
}
