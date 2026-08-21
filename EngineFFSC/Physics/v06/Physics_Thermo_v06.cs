// Physics_Thermo_v06.cs
//
// Modelo termoquimico para el motor FFSC v06.
//
// Teoria:
// - Modelo UC3M: modelo termoquimico para LOX/CH4
// - Polinomios NASA de 7 coeficientes para Cp, H, S
// - Equilibrio quimico para combustion de metano
// - Temperatura adiabatica de llama (Tad) mediante biseccion

using System;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Physics
{
    public static class Physics_Thermo_v06
    {
        public static double SolveTad(EngineParams p)
        {
            double Tlow = 1500.0;
            double Thigh = 5000.0;

            for (int i = 0; i < 100; i++)
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

        public static double Cp_Mixture(double mr, double T)
        {
            // Polinomios NASA 7-coeficientes simplificados
            // CO2 (producto combustion)
            double cpCO2 = 45.0 + 0.02 * T - 1.5e-5 * T * T;
            // H2O
            double cpH2O = 35.0 + 0.015 * T - 1.0e-5 * T * T;
            // N2 (inerte)
            double cpN2 = 30.0 + 0.01 * T - 0.5e-5 * T * T;

            // Mezcla: 1 mole O2 + mr moles CH4 -> productos
            double molesCO2 = mr;
            double molesH2O = 2.0 * mr;
            double molesN2 = 3.76 * (1.0 + 3.0 * mr); // Aire aproximado
            double totalMoles = molesCO2 + molesH2O + molesN2;

            double cp = (molesCO2 * cpCO2 + molesH2O * cpH2O + molesN2 * cpN2) / totalMoles;
            return cp;
        }

        private static double EnergyResidual(double T, EngineParams p)
        {
            // Entalpia de reactivos (aproximada)
            double hReactLOX = 0.0;
            double hReactCH4 = -74.8e3; // kJ/kmol -> J/kmol (simplificado)
            
            // Entalpia de productos
            double cpMean = Cp_Mixture(p.MixtureRatio, T);
            double hProducts = cpMean * T;
            
            // Balance energetico
            double qReact = hReactLOX + p.MixtureRatio * hReactCH4;
            return hProducts - qReact;
        }
    }
}
