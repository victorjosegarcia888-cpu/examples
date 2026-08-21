// BartzCalculator.cs
//
// Implementacion de la ecuacion de Bartz para transferencia de calor
// en toberas de cohete.
//
// Ecuacion:
// hg = 0.026 * (mu^0.2) * (cp^0.6) * (Pc/C*)^0.8 *
//      (Dt^0.2) * (Rc^-0.1) * (At/A)^0.9 * Pr^-0.6
//
// Donde:
// - hg: coeficiente de pelicula [W/m^2-K]
// - mu: viscosidad dinamica del gas [Pa-s]
// - cp: calor especifico a presion constante [J/kg-K]
// - Pc: presion de camara [Pa]
// - C*: velocidad caracteristica [m/s]
// - Dt: diametro de garganta [m]
// - Rc: radio de curvatura local [m]
// - At: area de garganta [m^2]
// - A: area local [m^2]
// - Pr: numero de Prandtl [-]
//
// Cita del PDF:
// "El coeficiente de pelicula Bartz predice el flujo de calor
//  en la pared de la tobera con precision razonable para
//  regimenes subsónicos y supersónicos."

using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Models
{
    /// <summary>
    /// Calculadora de transferencia de calor usando ecuacion de Bartz.
    /// </summary>
    public static class BartzCalculator
    {
        private const double C_FACTOR = 0.026;
        private const double MU_REF = 3.5e-5;
        private const double CP_REF = 2000.0;
        private const double PR_REF = 0.7;
        private const double CSTAR_REF = 1500.0;

        /// <summary>
        /// Evalua el coeficiente de pelicula Bartz.
        /// </summary>
        /// <param name="p">Parametros del motor</param>
        /// <param name="Tg">Temperatura del gas [K]</param>
        /// <param name="Dt_local">Diametro de garganta local [m]</param>
        /// <param name="A_local">Area de seccion local [m^2]</param>
        /// <returns>Coeficiente de pelicula hg [W/m^2-K]</returns>
        public static double Evaluate(
            EngineParams p,
            double Tg,
            double Dt_local,
            double A_local)
        {
            // Propiedades del gas (simplificadas, temperatura-dependientes)
            double mu = MU_REF * (1.0 + 0.0005 * (Tg - 3000.0)); // viscosidad
            double cp = CP_REF + 0.1 * (Tg - 3000.0);            // cp
            double Pr = PR_REF;

            double Pc = p.Pc;
            double Dt = p.ThroatRadius * 2.0;
            double At = p.At;

            double Aratio = At / A_local;
            double Rc = Dt_local / 2.0;

            double hg =
                C_FACTOR *
                Math.Pow(mu, 0.2) *
                Math.Pow(cp, 0.6) *
                Math.Pow(Pc / CSTAR_REF, 0.8) *
                Math.Pow(Dt, 0.2) *
                Math.Pow(Rc, -0.1) *
                Math.Pow(Aratio, 0.9) /
                Math.Pow(Pr, 0.6);

            return Math.Max(0.0, hg);
        }

        /// <summary>
        /// Calcula el flujo de calor de pared Qw = hg * (Tg - Tw).
        /// </summary>
        public static double HeatFlux(
            EngineParams p,
            double Tg,
            double Tw,
            double Dt_local,
            double A_local)
        {
            double hg = Evaluate(p, Tg, Dt_local, A_local);
            return hg * (Tg - Tw);
        }
    }
}
