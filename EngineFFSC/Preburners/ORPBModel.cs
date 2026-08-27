// ORPBModel.cs
//
// Modelo del Oxidizer Rich Preburner (ORPB) para ciclo FFSC.
// Utiliza LOX como oxidizer rico.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Preburners
{
    public class ORPBModel
    {
        public float CasingRadius { get; set; } = 0.025f;
        public float CasingLength { get; set; } = 0.080f;
        public float CombustionZoneRadius { get; set; } = 0.018f;
        public float CombustionZoneLength { get; set; } = 0.040f;
        public float InjectorCount { get; set; } = 24;
        public float VoxelResolution { get; set; } = 0.0005f;
        public float NozzleRadius { get; set; } = 0.008f;
        public float NozzleLength { get; set; } = 0.030f;

        public Voxels CreateCasing()
        {
            Voxels casing = new Voxels();
            float r = CasingRadius;
            float h = CasingLength;

            for (float z = -h / 2.0f; z <= h / 2.0f; z += VoxelResolution)
            {
                for (float x = -r; x <= r; x += VoxelResolution)
                for (float y = -r; y <= r; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r)
                    {
                        casing += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                    }
                }
            }

            return casing;
        }

        public Voxels CreateCombustionZone()
        {
            Voxels zone = new Voxels();
            float r = CombustionZoneRadius;
            float h = CombustionZoneLength;

            for (float z = 0; z <= h; z += VoxelResolution)
            for (float x = -r; x <= r; x += VoxelResolution)
            for (float y = -r; y <= r; y += VoxelResolution)
            {
                if (x * x + y * y <= r * r)
                {
                    zone += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                }
            }

            return zone;
        }

        public Voxels CreateInjectors()
        {
            Voxels injectors = new Voxels();
            float r = CombustionZoneRadius * 0.8f;

            for (int i = 0; i < InjectorCount; i++)
            {
                float angle = 2.0f * MathF.PI * i / InjectorCount;
                float x = (float)MathF.Cos(angle) * r;
                float y = (float)MathF.Sin(angle) * r;

                for (float z = -0.010f; z <= 0.005f; z += VoxelResolution)
                {
                    injectors += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution * 2.0f);
                }
            }

            return injectors;
        }

        public Voxels CreateNozzle()
        {
            Voxels nozzle = new Voxels();
            float r1 = NozzleRadius;
            float r2 = r1 * 1.8f;
            float h = NozzleLength;

            for (float z = 0; z <= h; z += VoxelResolution)
            {
                float t = z / h;
                float r = r1 * (1.0f - t) + r2 * t;

                for (float x = -r; x <= r; x += VoxelResolution)
                for (float y = -r; y <= r; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r)
                    {
                        nozzle += Voxels.voxSphere(new Vector3(x, y, z + CombustionZoneLength), VoxelResolution);
                    }
                }
            }

            return nozzle;
        }

        public Voxels Create()
        {
            Voxels orpb = new Voxels();

            orpb += CreateCasing();
            orpb += CreateCombustionZone();
            orpb += CreateInjectors();
            orpb += CreateNozzle();

            return orpb;
        }
    }
}
