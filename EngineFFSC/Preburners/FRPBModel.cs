// FRPBModel.cs
//
// Modelo del Fuel Rich Preburner (FRPB) para ciclo FFSC.
// Utiliza RP-1 / CH4 como combustible rico.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Preburners
{
    public class FRPBModel
    {
        public float CasingRadius { get; set; } = 0.022f;
        public float CasingLength { get; set; } = 0.070f;
        public float CombustionZoneRadius { get; set; } = 0.015f;
        public float CombustionZoneLength { get; set; } = 0.035f;
        public float InjectorCount { get; set; } = 18;
        public float VoxelResolution { get; set; } = 0.0005f;
        public float NozzleRadius { get; set; } = 0.006f;
        public float NozzleLength { get; set; } = 0.025f;
        public float FuelPortRadius { get; set; } = 0.004f;

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

                for (float z = -0.008f; z <= 0.004f; z += VoxelResolution)
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
            float r2 = r1 * 1.6f;
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

        public Voxels CreateFuelPort()
        {
            Voxels port = new Voxels();
            float r = FuelPortRadius;

            for (float z = -CasingLength / 2.0f; z <= CasingLength / 2.0f; z += VoxelResolution)
            {
                port += Voxels.voxSphere(new Vector3(r, 0, z), r);
                port += Voxels.voxSphere(new Vector3(-r, 0, z), r);
            }

            return port;
        }

        public Voxels Create()
        {
            Voxels frpb = new Voxels();

            frpb += CreateCasing();
            frpb += CreateCombustionZone();
            frpb += CreateInjectors();
            frpb += CreateNozzle();
            frpb += CreateFuelPort();

            return frpb;
        }
    }
}
