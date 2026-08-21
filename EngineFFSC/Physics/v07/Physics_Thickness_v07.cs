// Physics_Thickness_v07.cs
//
// Advanced wall thickness with thermal cycling and creep-fatigue interaction.
// Theory: Barlow-based thickness with Qnorm thermal amplification and cycle count degradation.

using System;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Physics
{
    public struct ThicknessPoint
    {
        public double Z;
        public double Radius;
        public double Thickness;
        public double CreepFactor;
        public double FatigueFactor;
    }

    public class ThicknessMap
    {
        public ThicknessPoint[] Points;
    }

    public static class Physics_Thickness_v07
    {
        public static ThicknessMap Run(EngineParams p, ThermoMap thermo, int cycleCount = 1000)
        {
            double FS = p.SafetyFactor;
            double sigmaAllow = p.Material.YieldStrengthPa / FS;

            ThicknessPoint[] pts = new ThicknessPoint[thermo.Points.Length];

            for (int i = 0; i < thermo.Points.Length; i++)
            {
                var t = thermo.Points[i];
                double radius = LocalRadius(t.Z, p);

                // Base Barlow thickness
                double tBarlow = (p.Pc * radius) / sigmaAllow;

                // Thermal factor from Qnorm
                double thermalFactor = 1.0 + 2.0 * t.Qnorm;

                // Temperature degradation
                double tempDegradation = 1.0 + 0.5 * Math.Max(0, (t.Tw - 600.0) / 600.0);

                // Creep factor (time at temperature)
                double creepFactor = 1.0 + 0.001 * Math.Max(0, t.Tw - p.Material.MaxServiceTemp_C + 273.15);

                // Fatigue factor from thermal cycling
                double fatigueFactor = 1.0 + 0.0001 * cycleCount * Math.Max(0, t.Qnorm - 0.3);

                double thickness = tBarlow * thermalFactor * tempDegradation * creepFactor * fatigueFactor;

                pts[i] = new ThicknessPoint
                {
                    Z = t.Z,
                    Radius = radius,
                    Thickness = thickness,
                    CreepFactor = creepFactor,
                    FatigueFactor = fatigueFactor
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
