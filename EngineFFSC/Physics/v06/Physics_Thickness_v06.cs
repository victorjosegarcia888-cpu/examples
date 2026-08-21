// Physics_Thickness_v06.cs
//
// Calculo de espesor de pared para motor FFSC v06.
//
// Teoria:
// - Formula de Barlow: pared delgada t = P*R/S
// - Ecuaciones de Lame: pared gruesa
// - Factor termico por gradiente de temperatura
// - Factor de seguridad segun normativa

using System;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Physics
{
    public static class Physics_Thickness_v06
    {
        public static double WallThickness_Barlow(double P, double R, double S, double safetyFactor = 1.5)
        {
            // Formula de Barlow para pared delgada
            // t = (P * R) / (S * FS)
            double t = (P * R) / (S * safetyFactor);
            return Math.Max(t, 0.001); // Espesor minimo constructivo
        }

        public static double WallThickness_Lame(double P, double R, double S, double safetyFactor = 1.5)
        {
            // Ecuacion de Lame para pared gruesa
            // Solucion para esfuerzo circunferencial maximo en superficie interna
            // sigma_max = P * (Ri^2 + Ro^2) / (Ro^2 - Ri^2)
            
            double sigmaAllow = S / safetyFactor;
            double Ri = R;
            
            // Iterar para encontrar Ro tal que sigma_max = sigmaAllow
            double Ro = Ri * 1.01; // Inicializacion
            for (int i = 0; i < 50; i++)
            {
                double sigmaMax = P * (Ri * Ri + Ro * Ro) / (Ro * Ro - Ri * Ri);
                if (Math.Abs(sigmaMax - sigmaAllow) / sigmaAllow < 1e-6)
                    break;
                
                // Ajuste simple
                if (sigmaMax > sigmaAllow)
                    Ro *= 1.02;
                else
                    Ro *= 0.98;
            }
            
            return Ro - Ri;
        }

        public static double ThermalFactor(double Tw, double Tcoolant, double kMaterial)
        {
            // Factor termico de degradacion
            double deltaT = Tw - Tcoolant;
            double q = 1e6; // Flujo de calor aproximado W/m2
            return 1.0 + (q * deltaT) / (kMaterial * 1000.0);
        }
    }
}
