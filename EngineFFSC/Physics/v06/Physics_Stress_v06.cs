// Physics_Stress_v06.cs
//
// Calculo de esfuerzos estructurales para FFSC v06.
//
// Teoria:
// - Esfuerzo circunferencial (hoop): sigma_hoop = P*R/t
// - Esfuerzo axial: sigma_axial = P*R/(2*t)
// - Ecuaciones de Lame para pared gruesa
// - Criterio de Von Mises para comparacion con limite de fluencia

using System;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Physics
{
    public static class Physics_Stress_v06
    {
        public static double HoopStress(double P, double R, double t)
        {
            // Esfuerzo circunferencial (hoop) en pared delgada
            // sigma_hoop = P * R / t
            double sigmaHoop = P * R / t;
            return sigmaHoop;
        }

        public static double AxialStress(double P, double R, double t)
        {
            // Esfuerzo axial en pared delgada
            // sigma_axial = P * R / (2 * t)
            double sigmaAxial = P * R / (2.0 * t);
            return sigmaAxial;
        }

        public static double RadialStress(double P, double R, double t, double ri)
        {
            // Esfuerzo radial en pared gruesa (ecuacion de Lame)
            // sigma_r = A - B/r^2
            // sigma_theta = A + B/r^2
            
            double ro = ri + t;
            double A = P * ri * ri / (ro * ro - ri * ri);
            double B = P * ri * ri * ro * ro / (ro * ri * (ro * ro - ri * ri));
            
            // Esfuerzo en superficie interna (r = ri)
            double sigmaR = A - B / (ri * ri);
            return sigmaR;
        }

        public static double VonMisesStress(double sigmaHoop, double sigmaAxial, double sigmaRadial)
        {
            // Criterio de Von Mises para esfuerzo equivalente
            // sigma_vm = sqrt(0.5 * ((s1-s2)^2 + (s2-s3)^2 + (s3-s1)^2))
            
            double s1 = sigmaHoop;
            double s2 = sigmaAxial;
            double s3 = sigmaRadial;

            double sigmaVm = Math.Sqrt(0.5 * (
                Math.Pow(s1 - s2, 2.0) + 
                Math.Pow(s2 - s3, 2.0) + 
                Math.Pow(s3 - s1, 2.0)
            ));

            return sigmaVm;
        }

        public static double SafetyMargin(double sigmaVm, double yieldStrength, double safetyFactor = 1.5)
        {
            // Margen de seguridad
            double allowable = yieldStrength / safetyFactor;
            return allowable / sigmaVm;
        }
    }
}
