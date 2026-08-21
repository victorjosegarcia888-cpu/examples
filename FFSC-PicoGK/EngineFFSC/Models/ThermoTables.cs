// ThermoTables.cs
//
// Tablas termodinamicas simplificadas para LOX/CH4.

using System;

namespace FFSC_PicoGK.Models
{
    public static class ThermoTables
    {
        public static double Cp_CO2(double T)
        {
            if (T < 1000) return 844.0 + 0.1 * (T - 300.0);
            return 1000.0 + 0.02 * (T - 1000.0);
        }

        public static double Cp_H2O(double T)
        {
            if (T < 1000) return 1850.0 + 0.15 * (T - 300.0);
            return 2100.0 + 0.03 * (T - 1000.0);
        }

        public static double Cp_O2(double T)
        {
            return 900.0 + 0.1 * (T - 300.0);
        }

        public static double Cp_N2(double T)
        {
            return 1040.0 + 0.1 * (T - 300.0);
        }

        public static double Cp_CH4(double T)
        {
            if (T < 500) return 1950.0 + 0.1 * (T - 300.0);
            if (T < 1500) return 2000.0 + 0.05 * (T - 500.0);
            return 2250.0 + 0.01 * (T - 1500.0);
        }

        public static double Cp_Mixture(double OF, double Tad)
        {
            double cp_CO2 = Cp_CO2(Tad);
            double cp_H2O = Cp_H2O(Tad);
            double cp_CH4 = Cp_CH4(Tad);
            double cp_O2 = Cp_O2(Tad);

            double w_CO2 = 0.3, w_H2O = 0.3, w_CH4 = 0.2, w_O2 = 0.2;
            return w_CO2 * cp_CO2 + w_H2O * cp_H2O + w_CH4 * cp_CH4 + w_O2 * cp_O2;
        }

        public static double Prandtl(double T)
        {
            return 0.7 + 0.1 * Math.Sin(T / 1000.0);
        }
    }
}
