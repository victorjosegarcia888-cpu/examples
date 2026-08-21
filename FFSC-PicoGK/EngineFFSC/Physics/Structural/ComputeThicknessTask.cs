// ComputeThicknessTask.cs
//
// Calculo de espesor estructural para el motor FFSC.
//
// Basado en:
// - Tension circunferencial (Barlow): t = P*r/sigma
// - Pared gruesa (Lame): sigma_hoop = P*(ri^2+ro^2)/(ro^2-ri^2)
// - Margen termico segun Qnorm(z)
//
// Cita PDF:
// "Por debajo de cierta temperatura desaparecen las llamas,
//  fenomeno denominado extincion. El espesor debe compensar
//  la degradacion termica del material."

using System;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Physics.Structural
{
    /// <summary>
    /// Punto del mapa de espesores.
    /// </summary>
    public struct ThicknessPoint
    {
        public double Z;
        public double Radius;
        public double Thickness;
    }

    /// <summary>
    /// Mapa de espesores.
    /// </summary>
    public class ThicknessMap
    {
        public ThicknessPoint[] Points;
    }

    /// <summary>
    /// Tarea de calculo de espesor estructural.
    /// </summary>
    public static class ComputeThicknessTask
    {
        /// <summary>
        /// Ejecuta el calculo de espesor.
        /// </summary>
        public static ThicknessMap Run(EngineParams p, ThermoMap thermo)
        {
            double FS = p.SafetyFactor;
            double sigmaAllow = p.Material.YieldStrengthPa / FS;

            ThicknessPoint[] pts = new ThicknessPoint[thermo.Points.Length];

            for (int i = 0; i < thermo.Points.Length; i++)
            {
                var t = thermo.Points[i];
                double radius = LocalRadius(t.Z, p);

                // Barlow: t = Pc * r / sigma_allow
                double tBarlow = (p.Pc * radius) / sigmaAllow;

                // Margen termico segun Qnorm
                double thermalFactor = 1.0 + 2.0 * t.Qnorm;

                // Degradacion del material con temperatura
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

        /// <summary>
        /// Radio local en funcion de z.
        /// </summary>
        private static double LocalRadius(double z, EngineParams p)
        {
            double At = p.At;
            double Ae = p.Ae;
            double Lstar = p.Lstar;

            if (z < p.ChamberLength)
            {
                return Math.Sqrt(At * p.ContractionRatio / Math.PI);
            }
            else
            {
                double zNozzle = z - p.ChamberLength;
                double Lnozzle = Lstar * 0.6;
                double t = Math.Min(1.0, zNozzle / Lnozzle);
                double A = At + (Ae - At) * Math.Pow(t, 1.5);
                return Math.Sqrt(A / Math.PI);
            }
        }
    }
}
