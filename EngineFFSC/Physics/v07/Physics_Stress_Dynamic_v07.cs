// Physics_Stress_Dynamic_v07.cs
//
// Dynamic stress analysis including fatigue, vibration, and transient loads.
// Theory: Rainflow counting for fatigue damage, modal analysis for vibration.

using System;
using System.Linq;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Physics
{
    public struct StressPoint
    {
        public double Z;
        public double StaticStress;
        public double DynamicStress;
        public double FatigueDamage;
        public double NaturalFrequency;
    }

    public class StressMap
    {
        public StressPoint[] Points;
    }

    public static class Physics_Stress_Dynamic_v07
    {
        public static StressMap Run(EngineParams p, ThermoMap thermo, ThicknessMap thickness, double vibrationAmplitude = 0.15)
        {
            int Nz = thermo.Points.Length;
            StressPoint[] pts = new StressPoint[Nz];

            for (int i = 0; i < Nz; i++)
            {
                var t = thermo.Points[i];
                var th = thickness.Points[i];

                // Static stress (pressure loading)
                double staticStress = (p.Pc * th.Radius) / th.Thickness;

                // Dynamic stress from vibration
                double dynamicStress = staticStress * vibrationAmplitude * Math.Sin(t.Z * 40.0);

                // Fatigue damage via simplified rainflow approximation
                double stressRange = Math.Abs(staticStress - dynamicStress);
                double fatigueDamage = Math.Pow(stressRange / p.Material.YieldStrengthPa, 2.0) * 0.01;

                // Natural frequency approximation for shell modes
                double naturalFreq = 120.0 * Math.Sqrt(p.Material.YoungsModulus / p.Material.Density) / (2.0 * Math.PI * th.Radius);

                pts[i] = new StressPoint
                {
                    Z = t.Z,
                    StaticStress = staticStress,
                    DynamicStress = dynamicStress,
                    FatigueDamage = fatigueDamage,
                    NaturalFrequency = naturalFreq
                };
            }

            return new StressMap { Points = pts };
        }

        public static double ComputeFatigueLife(StressMap stress)
        {
            double totalDamage = stress.Points.Sum(p => p.FatigueDamage);
            return totalDamage > 0 ? 1.0 / totalDamage : double.PositiveInfinity;
        }
    }
}
