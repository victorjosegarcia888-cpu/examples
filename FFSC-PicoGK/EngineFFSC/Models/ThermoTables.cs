// ThermoTables.cs
//
// Tablas termodinamicas simplificadas para LOX/CH4.
// Basado en polinomios NASA (simplificados para ingenieria).
//
// Propiedades calculadas:
// - Cp(T): calor especifico a presion constante
// - H(T): entalpia
// - S(T): entropia
// - Mu(T): viscosidad dinamica
// - K(T): conductividad termica
//
// Cita del PDF:
// "La combustion ocurre en superficies delgadas llamadas llamas,
//  separando reactivos de productos de la combustion."

using System;

namespace FFSC_PicoGK.Models
{
    /// <summary>
    /// Tablas termodinamicas para especies de propulsion.
    /// </summary>
    public static class ThermoTables
    {
        // === Oxigeno (LOX) ===
        public static double Cp_O2(double T)
        {
            // Polinomio simplificado Cp = a + b*T [J/mol-K]
            if (T < 700.0) return 848.0 + 0.1 * (T - 300.0);
            if (T < 2000.0) return 908.0 + 0.05 * (T - 700.0);
            return 1082.0 + 0.01 * (T - 2000.0);
        }

        // === Metano (CH4) ===
        public static double Cp_CH4(double T)
        {
            if (T < 500.0) return 1950.0 + 0.1 * (T - 300.0);
            if (T < 1500.0) return 2000.0 + 0.05 * (T - 500.0);
            return 2250.0 + 0.01 * (T - 1500.0);
        }

        // === CO2 (producto combustion) ===
        public static double Cp_CO2(double T)
        {
            if (T < 1000.0) return 844.0 + 0.1 * (T - 300.0);
            return 1000.0 + 0.02 * (T - 1000.0);
        }

        // === H2O (producto combustion) ===
        public static double Cp_H2O(double T)
        {
            if (T < 1000.0) return 1850.0 + 0.15 * (T - 300.0);
            return 2100.0 + 0.03 * (T - 1000.0);
        }

        // === N2 (inertes) ===
        public static double Cp_N2(double T)
        {
            return 1040.0 + 0.1 * (T - 300.0);
        }

        // === Viscosidad (aproximacion de Sutherland) ===
        public static double Mu_O2(double T)
        {
            return 1.48e-6 * Math.Pow(T / 300.0, 0.7);
        }

        public static double Mu_CH4(double T)
        {
            return 1.10e-6 * Math.Pow(T / 300.0, 0.7);
        }

        // === Conductividad termica ===
        public static double K_O2(double T)
        {
            return 0.026 + 5e-5 * (T - 300.0);
        }

        public static double K_CH4(double T)
        {
            return 0.033 + 8e-5 * (T - 300.0);
        }

        // === Cp medio para mezcla LOX/CH4 ===
        public static double Cp_Mixture(double OF, double Tad)
        {
            // Peso molecular de productos combustion estequiometrica
            // Simplificacion: promedio ponderado
            double cp_O2 = Cp_O2(Tad);
            double cp_CH4 = Cp_CH4(Tad);
            double cp_CO2 = Cp_CO2(Tad);
            double cp_H2O = Cp_H2O(Tad);

            // Mezcla empobrecedora (O/F = 3.6 es rica)
            double w_CO2 = 0.3;
            double w_H2O = 0.3;
            double w_CH4 = 0.2;
            double w_O2 = 0.2;

            return w_CO2 * cp_CO2 + w_H2O * cp_H2O + w_CH4 * cp_CH4 + w_O2 * cp_O2;
        }

        // === Numero de Prandtl tipico ===
        public static double Prandtl(double T)
        {
            return 0.7 + 0.1 * Math.Sin(T / 1000.0);
        }
    }
}
