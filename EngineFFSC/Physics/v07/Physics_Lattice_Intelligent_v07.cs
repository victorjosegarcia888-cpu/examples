// Physics_Lattice_Intelligent_v07.cs
//
// Intelligent lattice mechanics with adaptive density and multiscale modeling.
// Theory: Voronoi-TPMS hybrid stiffness with Qnorm-driven topology optimization.

using System;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Physics
{
    public struct LatticePoint
    {
        public double X;
        public double Y;
        public double Z;
        public double Density;
        public double Stiffness;
        public double ThermalConductivity;
    }

    public class LatticeMap
    {
        public required LatticePoint[] Points;
    }

    public static class Physics_Lattice_Intelligent_v07
    {
        public static LatticeMap Run(EngineParams p, ThermoMap thermo, int pointCount = 60)
        {
            LatticePoint[] pts = new LatticePoint[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                double x = (i / 20 - 1.0) * 0.3;
                double y = (i % 10 - 5) * 0.06;
                double z = (i % 4 - 2) * 0.15;

                // Find nearest thermo point for Qnorm
                int nearestIdx = 0;
                double minDist = double.MaxValue;
                for (int j = 0; j < thermo.Points.Length; j++)
                {
                    double dist = Math.Abs(thermo.Points[j].Z - (z + 0.3));
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestIdx = j;
                    }
                }

                double qnorm = thermo.Points[nearestIdx].Qnorm;

                // Adaptive density: higher in high-stress regions
                double density = 0.3 + 0.7 * qnorm;

                // Stiffness scales with density squared (power law)
                double stiffness = p.Material.YoungsModulus * Math.Pow(density, 2.0);

                // Thermal conductivity through TPMS solid fraction
                double thermalCond = p.Material.ThermalConductivity * density * 0.5;

                pts[i] = new LatticePoint
                {
                    X = x,
                    Y = y,
                    Z = z,
                    Density = density,
                    Stiffness = stiffness,
                    ThermalConductivity = thermalCond
                };
            }

            return new LatticeMap { Points = pts };
        }
    }
}
