// NozzleGeometry.cs
//
// Geometria de tobera para motor FFSC.
// Perfil convergente-divergente de Laval.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.CombustionChamber
{
    public class NozzleGeometry
    {
        public float ThroatRadius { get; set; } = 0.12f;
        public float ExitRadius { get; set; } = 0.80f;
        public float NozzleLength { get; set; } = 1.20f;
        public float ConvergenceAngle_deg { get; set; } = 25.0f;
        public float DivergenceAngle_deg { get; set; } = 12.0f;
        public float VoxelResolution { get; set; } = 0.0005f;

        public Voxels CreateConvergent()
        {
            Voxels convergent = new Voxels();
            float r1 = ExitRadius * 0.6f;
            float r2 = ThroatRadius;
            float angle = ConvergenceAngle_deg * MathF.PI / 180.0f;
            float length = (r1 - r2) / (float)MathF.Tan(angle);

            for (float z = 0; z <= length; z += VoxelResolution)
            {
                float t = z / length;
                float r = r1 * (1.0f - t) + r2 * t;

                for (float x = -r; x <= r; x += VoxelResolution)
                for (float y = -r; y <= r; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r)
                    {
                        convergent += Voxels.voxSphere(new Vector3(x, y, z - length), VoxelResolution);
                    }
                }
            }

            return convergent;
        }

        public Voxels CreateDivergent()
        {
            Voxels divergent = new Voxels();
            float r1 = ThroatRadius;
            float r2 = ExitRadius;
            float angle = DivergenceAngle_deg * MathF.PI / 180.0f;
            float length = (r2 - r1) / (float)MathF.Tan(angle);

            for (float z = 0; z <= length; z += VoxelResolution)
            {
                float t = z / length;
                float r = r1 * (1.0f - t) + r2 * t;

                for (float x = -r; x <= r; x += VoxelResolution)
                for (float y = -r; y <= r; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r)
                    {
                        divergent += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                    }
                }
            }

            return divergent;
        }

        public Voxels CreateExtendableBell()
        {
            Voxels bell = new Voxels();
            float r1 = ThroatRadius;
            float r2 = ExitRadius;
            float length = NozzleLength * 0.9f;

            for (float z = 0; z <= length; z += VoxelResolution)
            {
                float t = z / length;
                float r = r1 * (float)MathF.Pow(r2 / r1, t);

                for (float x = -r; x <= r; x += VoxelResolution)
                for (float y = -r; y <= r; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r)
                    {
                        bell += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                    }
                }
            }

            return bell;
        }

        public Voxels Create()
        {
            Voxels nozzle = new Voxels();

            nozzle += CreateConvergent();
            nozzle += CreateDivergent();

            return nozzle;
        }

        public Voxels CreateBell()
        {
            Voxels bell = new Voxels();

            bell += CreateConvergent();
            bell += CreateExtendableBell();

            return bell;
        }
    }
}
