// CoolingTask.cs
//
// Tarea de refrigeracion regenerativa del motor FFSC.
//
// Calcula:
// - Perfil de temperatura de pared Tw(z)
// - Coeficiente de pelicula hg(z)
// - Flujo de calor Qw(z)
// - Qnorm(z) normalizado
//
// Basado en:
// - Correlacion de Bartz
// - Ecuaciones de conveccion interna
//
// Cita PDF:
// "La refrigeracion regenerativa usa el propio combustible
//  como refrigerante. CH4 absorbe calor antes de la combustion."

using System;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Physics.Cooling
{
    /// <summary>
    /// Tarea de refrigeracion regenerativa.
    /// </summary>
    public static class CoolingTask
    {
        /// <summary>
        /// Ejecuta el calculo de refrigeracion.
        /// </summary>
        /// <param name="p">Parametros del motor</param>
        /// <param name="thermo">Mapa termico</param>
        /// <returns>Mapa de enfriamiento</returns>
        public static CoolingMap Run(EngineParams p, ThermoMap thermo)
        {
            int Nz = thermo.Points.Length;
            CoolingPoint[] pts = new CoolingPoint[Nz];

            double mdotCool = p.CoolantMassFlow;
            double cpCool = 5350.0; // J/kg-K CH4
            double TwInlet = p.CoolantInletTemp_C + 273.15;
            double TwMax = p.CoolantOutletTemp_C + 273.15;

            for (int i = 0; i < Nz; i++)
            {
                var t = thermo.Points[i];
                double fraction = i / (double)(Nz - 1);

                // Temperatura de pared
                double Tw = TwInlet + (TwMax - TwInlet) * fraction;

                // Flujo de calor
                double Qw = t.Hg * (t.Tg - Tw);

                // Incremento de temperatura del refrigerante
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

    /// <summary>
    /// Punto del mapa de enfriamiento.
    /// </summary>
    public struct CoolingPoint
    {
        public double Z;
        public double Tw;
        public double Qw;
        public double Qnorm;
        public double CoolantTemp;
    }

    /// <summary>
    /// Mapa de enfriamiento completo.
    /// </summary>
    public class CoolingMap
    {
        public CoolingPoint[] Points;
    }
}
