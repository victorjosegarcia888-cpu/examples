// Physics_Cooling_v06.cs
//
// Modelo de refrigeracion regenerativa para FFSC v06.
//
// Teoria:
// - Refrigeracion regenerativa: calor extraido por refrigerante
// - Numero de Nusselt: Nu = h*D/k (conveccion forzada)
// - Correlacion de Dittus-Boelter: Nu = 0.023*Re^0.8*Pr^n
// - Ecuacion de Bartz para flujo de calor en tobera

using System;

namespace EngineFFSC.Physics
{
    public static class Physics_Cooling_v06
    {
        public static double HeatFlux(double Hg, double Tw, double Tcoolant)
        {
            // Flujo de calor total: q = Hg * (Tg - Tw)
            // donde Hg es coeficiente de conveccion gas
            // Tg aproximado como temperatura de camara
            
            double Tg = 3600.0; // K (aprox)
            double qTotal = Hg * (Tg - Tw);
            
            // Aporte de refrigeracion por conveccion
            // qcooling = h * (Tw - Tcoolant)
            double h = 5000.0; // W/m2K (coeficiente convectivo refrigerante)
            double qCooling = h * (Tw - Tcoolant);
            
            // Flujo neto
            double qNet = qTotal - qCooling;
            
            return qNet;
        }

        public static double NusseltNumber(double Re, double Pr, double Dh)
        {
            // Numero de Nusselt para tubo circular
            // Correlacion de Dittus-Boelter
            // Nu = 0.023 * Re^0.8 * Pr^n
            double n = 0.4; // Calentamiento (heating)
            double Nu = 0.023 * Math.Pow(Re, 0.8) * Math.Pow(Pr, n);
            return Nu;
        }

        public static double BartzCorrection(double gamma, double M, double Tw, double Tg)
        {
            // Factor de correccion de Bartz
            // sigma = (0.5 * (Tw/Tg) * (1 + 0.5*(gamma-1)*M^2) + 0.5)^-0.68 * (1 + 0.5*(gamma-1)*M^2)^-0.12
            
            double term1 = 0.5 * (Tw / Tg) * (1.0 + 0.5 * (gamma - 1.0) * M * M) + 0.5;
            double term2 = 1.0 + 0.5 * (gamma - 1.0) * M * M;
            
            double sigma = Math.Pow(term1, -0.68) * Math.Pow(term2, -0.12);
            return sigma;
        }

        public static double CoolingChannelHeatTransfer(double massFlow, double cp, double deltaT, double area)
        {
            // Transferencia de calor en canal de refrigeracion
            // q = m * cp * deltaT / A
            double q = massFlow * cp * deltaT / area;
            return q;
        }
    }
}
