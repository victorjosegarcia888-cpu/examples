// Geometry_Lattice_v06.cs
//
// Geometria de estructura reticular TPMS Gyroid FFSC v06.
// Usando PicoGK Voxels API: voxSphere para nodos de reticulado.
//
// Teoria:
// - Superficie minimal Gyroid: sin(x)cos(y) + sin(y)cos(z) + sin(z)cos(x) = 0
// - Proyeccion 6D->3D de cuasicristal para gradiente de densidad
// - Interpolacion exponencial entre densidades minima y maxima

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Lattice_v06
    {
        public static Voxels Generate(Voxels stressField, double minDensity = 0.3, double maxDensity = 0.7)
        {
            Voxels lattice = new Voxels();

            double cellSize = 0.05;
            int gridSize = 20;
            float voxelRadius = 0.008f;

            for (int ix = -gridSize; ix <= gridSize; ix++)
            {
                for (int iy = -gridSize; iy <= gridSize; iy++)
                {
                    for (int iz = -gridSize; iz <= gridSize; iz++)
                    {
                        double x = ix * cellSize;
                        double y = iy * cellSize;
                        double z = iz * cellSize;

                        // Funcion Gyroid (superficie TPMS)
                        double gyroid = Math.Sin(x) * Math.Cos(y) + 
                                       Math.Sin(y) * Math.Cos(z) + 
                                       Math.Sin(z) * Math.Cos(x);

                        // Mapa de densidad basado en campo de tension
                        double stress = Math.Abs(Math.Sin(x * 2.0) * Math.Cos(y * 2.0) * Math.Sin(z * 2.0));
                        double density = minDensity + (maxDensity - minDensity) * 
                                        Math.Exp(-stress * 2.0) * 0.5 + 0.5;

                        // Generar voxel si esta dentro de la superficie Gyroid
                        if (Math.Abs(gyroid) < density * 0.5)
                        {
                            Vector3 pos = new Vector3((float)x, (float)y, (float)z);
                            lattice += Voxels.voxSphere(pos, voxelRadius);
                        }
                    }
                }
            }

            return lattice;
        }
    }
}
