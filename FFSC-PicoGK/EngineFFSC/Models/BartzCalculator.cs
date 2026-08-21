// BartzCalculator.cs
//
// Implementacion de la ecuacion de Bartz para transferencia de calor.
// Usando PicoGK Voxels API.

using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Models
{
    public static class BartzCalculator
    {
        private const double C_FACTOR = 0.026;
        private const double MU_REF = 3.5e-5;
        private const double CP_REF = 2000.0;
        private const double PR_REF = 0.7;
        private const double CSTAR_REF = 1500.0;

        public static double Evaluate(
            EngineParams p,
            double Tg,
            double Dt_local,
            double A_local)
        {
            double mu = MU_REF * (1.0 + 0.0005 * (Tg - 3000.0));
            double cp = CP_REF + 0.1 * (Tg - 3000.0);
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
