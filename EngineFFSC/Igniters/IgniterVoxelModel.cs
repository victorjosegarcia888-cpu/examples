// IgniterVoxelModel.cs
//
// Modelado voxelizado del igniter.
// Carcasa, boquilla de ignicion y canales de combustible.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Igniters
{
    public class IgniterVoxelModel
    {
        public float HousingRadius { get; set; } = 0.012f;
        public float HousingLength { get; set; } = 0.040f;
        public float IgnitionChamberRadius { get; set; } = 0.006f;
        public float IgnitionChamberLength { get; set; } = 0.015f;
        public float FuelInjectorRadius { get; set; } = 0.002f;
        public float OxidizerInjectorRadius { get; set; } = 0.002f;
        public float VoxelResolution { get; set; } = 0.0005f;

        public Voxels CreateHousing()
        {
            Voxels housing = new Voxels();
            float r = HousingRadius;
            float h = HousingLength;

            for (float z = -h / 2.0f; z <= h / 2.0f; z += VoxelResolution)
            {
                float taper = 1.0f - MathF.Abs(z) / (h / 2.0f) * 0.1f;
                for (float x = -r * taper; x <= r * taper; x += VoxelResolution)
                for (float y = -r * taper; y <= r * taper; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r * taper * taper)
                    {
                        housing += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                    }
                }
            }

            return housing;
        }

        public Voxels CreateIgnitionChamber()
        {
            Voxels chamber = new Voxels();
            float r = IgnitionChamberRadius;
            float h = IgnitionChamberLength;

            for (float z = 0; z <= h; z += VoxelResolution)
            for (float x = -r; x <= r; x += VoxelResolution)
            for (float y = -r; y <= r; y += VoxelResolution)
            {
                if (x * x + y * y <= r * r)
                {
                    chamber += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                }
            }

            return chamber;
        }

        public Voxels CreateInjectors()
        {
            Voxels injectors = new Voxels();
            Vector3 fuelPos = new Vector3(0, 0, IgnitionChamberLength);
            Vector3 oxPos = new Vector3(0, 0.003f, IgnitionChamberLength);

            for (float z = 0; z <= 0.010f; z += VoxelResolution)
            {
                injectors += Voxels.voxSphere(fuelPos + new Vector3(0, 0, z), FuelInjectorRadius);
                injectors += Voxels.voxSphere(oxPos + new Vector3(0, 0, z), OxidizerInjectorRadius);
            }

            return injectors;
        }

        public Voxels Create()
        {
            Voxels igniter = new Voxels();

            igniter += CreateHousing();
            igniter += CreateIgnitionChamber();
            igniter += CreateInjectors();

            return igniter;
        }
    }
}
