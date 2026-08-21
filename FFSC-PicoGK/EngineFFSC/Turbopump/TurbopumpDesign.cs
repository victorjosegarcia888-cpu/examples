// TurbopumpDesign.cs
//
// Diseno parametrico de turbobomba FFSC.
//
// Basado en:
// - Ecuacion de Euler: DeltaH = U2*Cu2 - U1*Cu1
// - Continuidad: Q = 2*pi*rm*h*Cm
// - Triangulos de velocidad
// - Relacion de flujo especifico
//
// Cita PDF:
// "La ecuacion de Euler para turbomaquinas relaciona el trabajo
//  especifico con las velocidades perifericas y tangenciales."

using System.Collections.Generic;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.EngineFFSC.Turbopump
{
    /// <summary>
    /// Especificacion de diseno de turbobomba.
    /// </summary>
    public class TurbopumpDesign
    {
        public double MassFlow { get; set; }
        public double Head { get; set; }
        public double Omega { get; set; }
        public double U2 { get; set; }
        public double R1 { get; set; }
        public double R2 { get; set; }
        public double BladeHeight { get; set; }
        public double InletAngle_deg { get; set; }
        public double OutletAngle_deg { get; set; }
        public int BladeCount { get; set; }
        public double BladeChord { get; set; }
        public Dictionary<string, double> ShapeParams { get; set; } = new();
    }

    /// <summary>
    /// Clase de diseno de turbobomba.
    /// </summary>
    public static class TurbopumpDesigner
    {
        /// <summary>
        /// Disena una turbobomba para los parametros dados.
        /// </summary>
        public static TurbopumpDesign Run(EngineParams p, double mdotOxidizer)
        {
            double rho = 1141.0; // LOX
            double Q = mdotOxidizer / rho;

            double deltaP = 30e6; // 30 MPa
            double deltaH = deltaP / rho;

            double N = p.TurbopumpRPM;
            double omega = 2.0 * Math.PI * N / 60.0;

            // Radio de salida del rotor (Euler)
            double r2 = 0.06;
            double U2 = omega * r2;
            double Cu2 = deltaH / U2;

            // Radio de entrada
            double r1 = 0.03;
            double rm = (r1 + r2) / 2.0;
            double Cm = 10.0;

            // Altura de aspa
            double h = Q / (2.0 * Math.PI * rm * Cm);

            return new TurbopumpDesign
            {
                MassFlow = mdotOxidizer,
                Head = deltaP,
                Omega = omega,
                U2 = U2,
                R1 = r1,
                R2 = r2,
                BladeHeight = h,
                InletAngle_deg = 25.0,
                OutletAngle_deg = 30.0,
                BladeCount = 10,
                BladeChord = 0.04,
                ShapeParams = new Dictionary<string, double>
                {
                    { "r1", r1 },
                    { "r2", r2 },
                    { "h", h },
                    { "omega", omega },
                    { "Cu2", Cu2 },
                    { "Cm", Cm },
                    { "deltaH", deltaH }
                }
            };
        }
    }
}
