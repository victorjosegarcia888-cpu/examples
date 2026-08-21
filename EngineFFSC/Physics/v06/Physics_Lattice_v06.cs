// Physics_Lattice_v06.cs
//
// Modelo de estructura reticular TPMS Gyroid para FFSC v06.
//
// Teoria:
// - Superficie minimal Gyroid: sin(x)cos(y) + sin(y)cos(z) + sin(z)cos(x) = 0
// - Proyeccion 6D->3D de cuasicristal para patrones de densidad
// - Interpolacion exponencial para gradientes de rigidez

using System;

namespace EngineFFSC.Physics
{
    public static class Physics_Lattice_v06
    {
        public static double GyroidDensity(double x, double y, double z, double t)
        {
            // Ecuacion implicita de la superficie Gyroid
            // G(x,y,z) = sin(x)cos(y) + sin(y)cos(z) + sin(z)cos(x)
            // t es el parametro de grosor (sheet thickness)
            
            double g = Math.Sin(x) * Math.Cos(y) + 
                      Math.Sin(y) * Math.Cos(z) + 
                      Math.Sin(z) * Math.Cos(x);

            // Densidad de material en el punto
            // Si |g| < t/2 -> dentro del material
            // Si |g| > t/2 -> vacio
            // Transicion suave con funcion sigmoide
            double halfT = t / 2.0;
            if (Math.Abs(g) < halfT)
                return 1.0;
            else if (Math.Abs(g) < halfT * 1.5)
                return 1.0 - (Math.Abs(g) - halfT) / (halfT * 0.5);
            else
                return 0.0;
        }

        public static double QuasicrystalProjection(double x, double y, double z)
        {
            // Proyeccion 6D->3D para patron de cuasicristal
            // Usando 2 planos de proyeccion ortogonales
            double theta1 = x + y * 0.5 + z * 0.3;
            double theta2 = x * 0.7 + y - z * 0.4;
            
            return Math.Sin(theta1) * Math.Cos(theta2);
        }

        public static double DensityField(double x, double y, double z, double minDensity, double maxDensity)
        {
            // Campo de densidad con interpolacion exponencial
            double gyroid = GyroidDensity(x, y, z, 0.15);
            double quasicrystal = QuasicrystalProjection(x, y, z);
            
            // Interpolacion exponencial entre min y max densidad
            double density = minDensity + (maxDensity - minDensity) * 
                            Math.Exp(-gyroid * quasicrystal * 2.0) * 0.5 + 0.5;
            
            return Math.Clamp(density, minDensity, maxDensity);
        }
    }
}
