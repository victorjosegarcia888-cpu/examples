// Physics_Turbopump_Dual_v06.cs
//
// Analisis de turbobomba dual LOX/CH4 para FFSC v06.
//
// Teoria:
// - Ecuacion de Euler: Delta h = U * Delta Vtheta
// - Ecuacion de continuidad: Q = A * V
// - Triangulos de velocidades (absoluta, relativa, tangencial)
// - Velocidad especifica (Ns) para seleccion de bomba

using System;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Physics
{
    public static class Physics_Turbopump_Dual_v06
    {
        public static void AnalyzeDual(double flowLOX, double flowCH4, double PdischargeLOX, double PdischargeCH4, double rpm)
        {
            // Parametros comunes
            double rhoLOX = 1140.0; // kg/m3
            double rhoCH4 = 422.0;  // kg/m3
            double g = 9.81;

            // Cabeza de bomba requerida
            double headLOX = (PdischargeLOX - 350e5) / (rhoLOX * g); // m
            double headCH4 = (PdischargeCH4 - 350e5) / (rhoCH4 * g); // m

            // Potencia requerida
            double powerLOX = flowLOX * g * headLOX / 0.75; // 75% eficiencia
            double powerCH4 = flowCH4 * g * headCH4 / 0.75;

            Console.WriteLine($"=== Turbobomba Dual FFSC v06 ===");
            Console.WriteLine($"RPM: {rpm}");
            Console.WriteLine($"LOX: Q={flowLOX} kg/s, Head={headLOX:F1} m, Power={powerLOX/1000:F1} kW");
            Console.WriteLine($"CH4: Q={flowCH4} kg/s, Head={headCH4:F1} m, Power={powerCH4/1000:F1} kW");
            Console.WriteLine($"Total Power: {(powerLOX + powerCH4)/1000:F1} kW");
        }

        public static double EulerHead(double U, double DeltaVtheta)
        {
            // Ecuacion de Euler para turbomaquinas
            // Delta h = U * Delta Vtheta
            return U * DeltaVtheta;
        }

        public static double SpecificSpeed(double Q, double head, double rpm)
        {
            // Velocidad especifica Ns = N * sqrt(Q) / (H)^(3/4)
            return rpm * Math.Sqrt(Q) / Math.Pow(head, 0.75);
        }

        public static void VelocityTriangles(double U, double Vaxial, double VthetaIn, double VthetaOut)
        {
            // Triangulo de velocidades absolutas
            double VabsIn = Math.Sqrt(Vaxial * Vaxial + VthetaIn * VthetaIn);
            double VabsOut = Math.Sqrt(Vaxial * Vaxial + VthetaOut * VthetaOut);

            // Triangulo de velocidades relativas
            double VrelIn = Math.Sqrt(Vaxial * Vaxial + Math.Pow(VthetaIn - U, 2.0));
            double VrelOut = Math.Sqrt(Vaxial * Vaxial + Math.Pow(VthetaOut - U, 2.0));

            Console.WriteLine($"U={U:F2} m/s");
            Console.WriteLine($"Vabs_in={VabsIn:F2} m/s, Vrel_in={VrelIn:F2} m/s");
            Console.WriteLine($"Vabs_out={VabsOut:F2} m/s, Vrel_out={VrelOut:F2} m/s");
        }
    }
}
