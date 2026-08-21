// Geometry_Lattice_Intelligent_v07.cs
//
// Adaptive lattice structure with AI-controlled density.
// Theory: Combines TPMS (Gyroid), quasicrystal symmetry, and Voronoi tessellation.
// Using PicoGK Voxels API.

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Lattice_Intelligent_v07
    {
        public static Voxels Build(
            Voxels stressField,
            EngineParams p,
            double tpmsWeight = 0.4,
            double quasicrystalWeight = 0.35,
            double voronoiWeight = 0.25)
        {
            Voxels lattice = new Voxels();

            var bbox = stressField.oCalculateBoundingBox();
            var center = bbox.vecCenter();
            var size = bbox.vecSize();

            // TPMS Gyroid layer
            lattice += BuildTPMS(center, size, tpmsWeight, 0.015);

            // Quasicrystal layer
            lattice += BuildQuasicrystal(center, size, quasicrystalWeight, 0.012);

            // Voronoi seed points
            lattice += BuildVoronoi(center, size, voronoiWeight, 0.010);

            return lattice;
        }

        private static Voxels BuildTPMS(Vector3 center, Vector3 size, double weight, double baseRadius)
        {
            Voxels tpms = new Voxels();
            int count = (int)(30 * weight);

            for (int i = 0; i < count; i++)
            {
                float x = center.X + (float)((i / 10 - 1.5) * size.X * 0.4);
                float y = center.Y + (float)((i % 5 - 2) * size.Y * 0.2);
                float z = center.Z + (float)((i % 3 - 1) * size.Z * 0.3);

                double gyroid = Math.Sin(x * 20) * Math.Cos(y * 20) + Math.Sin(y * 20) * Math.Cos(z * 20) + Math.Sin(z * 20) * Math.Cos(x * 20);
                if (Math.Abs(gyroid) < 0.3)
                {
                    tpms += Voxels.voxSphere(new Vector3(x, y, z), (float)(baseRadius * weight));
                }
            }

            return tpms;
        }

        private static Voxels BuildQuasicrystal(Vector3 center, Vector3 size, double weight, double baseRadius)
        {
            Voxels qc = new Voxels();
            int count = (int)(25 * weight);

            for (int i = 0; i < count; i++)
            {
                float x = center.X + (float)((i / 8 - 1.0) * size.X * 0.5);
                float y = center.Y + (float)((i % 4 - 2) * size.Y * 0.25);
                float z = center.Z + (float)((i % 5 - 2) * size.Z * 0.2);

                double qcPattern = Math.Sin(x * 15 + y * 15) + Math.Sin(y * 15 + z * 15) + Math.Sin(z * 15 + x * 15);
                if (Math.Abs(qcPattern) < 0.5)
                {
                    qc += Voxels.voxSphere(new Vector3(x, y, z), (float)(baseRadius * weight));
                }
            }

            return qc;
        }

        private static Voxels BuildVoronoi(Vector3 center, Vector3 size, double weight, double baseRadius)
        {
            Voxels voronoi = new Voxels();
            int seedCount = (int)(20 * weight);

            for (int i = 0; i < seedCount; i++)
            {
                float x = center.X + (float)((i / 7 - 1.0) * size.X * 0.5);
                float y = center.Y + (float)((i % 5 - 2) * size.Y * 0.2);
                float z = center.Z + (float)((i % 4 - 2) * size.Z * 0.25);

                voronoi += Voxels.voxSphere(new Vector3(x, y, z), (float)(baseRadius * weight * 1.5));
            }

            return voronoi;
        }
    }
}
