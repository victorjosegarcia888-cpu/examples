// ComputeThermoTask.cs
//
// Tarea termoquimica para el motor FFSC.
//
// Calcula:
// - Temperatura adiabatica de llama (Tad)
// - Mapa termico axial Tg(z)
// - Coeficiente de pelicula Bartz hg(z)
// - Campo normalizado Qnorm(z)
//
// Basado en:
// - Termoquimica UC3M
// - Ecuaciones y referencias para diseno de motores de cohete
//
// Cita:
// "La combustion ocurre en superficies delgadas llamadas llamas,
//  separando reactivos de productos."

using System;
using System.Linq;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Physics.Thermo
{
    /// <summary>
    /// Punto del mapa termico axial.
    /// </summary>
    public struct ThermoPoint
    {
        public double Z;         // Posicion axial [m]
        public double Tg;        // Temperatura del gas [K]
        public double Hg;        // Coeficiente de pelicula [W/m^2-K]
        public double Qnorm;     // Flujo normalizado [0-1]
        public double Tw;        // Temperatura de pared [K]
    }

    /// <summary>
    /// Mapa termico completo.
    /// </summary>
    public class ThermoMap
    {
        public ThermoPoint[] Points;
    }

    /// <summary>
    /// Tarea de computacion termoquimica.
    /// </summary>
    public static class ComputeThermoTask
    {
        /// <summary>
        /// Ejecuta el calculo termoquimico completo.
        /// </summary>
        public static ThermoMap Run(EngineParams p)
        {
            // 1. Resolver Tad
            double Tad = SolveTad(p);

            // 2. Construir discretizacion axial
            int Nz = p.Nz;
            ThermoPoint[] pts = new ThermoPoint[Nz];

            for (int i = 0; i < Nz; i++)
            {
                double z = p.Lstar * (i / (double)(Nz - 1));
                double A_local = LocalArea(z, p);
                double Dt_local = Math.Sqrt(4.0 * A_local / Math.PI);

                // 3. Evaluar Bartz
                double hg = BartzCalculator.Evaluate(p, Tad, Dt_local, A_local);

                // Temperatura de pared estimada
                double Tw = p.CoolantInletTemp_C + 273.15 + 200.0 * (z / p.Lstar);

                pts[i] = new ThermoPoint
                {
                    Z = z,
                    Tg = Tad,
                    Hg = hg,
                    Qnorm = 0.0,
                    Tw = Tw
                };
            }

            // 4. Normalizar Qnorm
            double hgMin = pts.Min(t => t.Hg);
            double hgMax = pts.Max(t => t.Hg);

            foreach (ref var t in pts.AsSpan())
            {
                t.Qnorm = (t.Hg - hgMin) / Math.Max(1e-12, hgMax - hgMin);
            }

            return new ThermoMap { Points = pts };
        }

        // === Rutina iterativa para Tad ===
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

        // === Residual energetico simplificado ===
        private static double EnergyResidual(double T, EngineParams p)
        {
            double hReact = -100000.0;
            double cpMean = ThermoTables.Cp_Mixture(p.MixtureRatio, T);
            double hProd = hReact + cpMean * (T - 298.15);
            return hProd - hReact - cpMean * (p.TadInitialGuess - 298.15);
        }

        // === Perfil de area local ===
        private static double LocalArea(double z, EngineParams p)
        {
            double At = p.At;
            double Ae = p.Ae;
            double Lstar = p.Lstar;

            // Distribucion de area a lo largo de la camara+tobera
            if (z < p.ChamberLength)
            {
                // Seccion cilindrica de la camara
                return At * p.ContractionRatio;
            }
            else
            {
                // Seccion convergente-divergente
                double zNozzle = z - p.ChamberLength;
                double Lnozzle = Lstar * 0.6;
                double t = Math.Min(1.0, zNozzle / Lnozzle);
                return At + (Ae - At) * Math.Pow(t, 1.5);
            }
        }
    }
}
