// TurbopumpVoxelModel.cs
//
// Modelado voxelizado de turbobomba FFSC.
// Incluye housing, rotor, estator y eje.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Turbopumps
{
    public class TurbopumpVoxelModel
    {
        public float HousingRadius { get; set; } = 0.060f;
        public float HousingLength { get; set; } = 0.120f;
        public float RotorRadius { get; set; } = 0.050f;
        public float RotorLength { get; set; } = 0.080f;
        public float ShaftRadius { get; set; } = 0.012f;
        public float BladeCount { get; set; } = 12;
        public float BladeHeight { get; set; } = 0.015f;
        public float VoxelResolution { get; set; } = 0.0005f;

        public Voxels CreateHousing()
        {
            Voxels housing = new Voxels();
            float r = HousingRadius;
            float h = HousingLength;

            for (float z = -h / 2.0f; z <= h / 2.0f; z += VoxelResolution)
            {
                for (float x = -r; x <= r; x += VoxelResolution)
                for (float y = -r; y <= r; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r)
                    {
                        housing += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                    }
                }
            }

            return housing;
        }

        public Voxels CreateRotor()
        {
            Voxels rotor = new Voxels();
            float r = RotorRadius;
            float h = RotorLength;

            for (float z = -h / 2.0f; z <= h / 2.0f; z += VoxelResolution)
            {
                float taper = 1.0f - MathF.Abs(z) / (h / 2.0f) * 0.15f;
                for (float x = -r * taper; x <= r * taper; x += VoxelResolution)
                for (float y = -r * taper; y <= r * taper; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r * taper * taper)
                    {
                        rotor += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                    }
                }
            }

            for (int i = 0; i < BladeCount; i++)
            {
                float angle = 2.0f * MathF.PI * i / BladeCount;
                float bx = (float)MathF.Cos(angle) * (r * 0.3f);
                float by = (float)MathF.Sin(angle) * (r * 0.3f);

                for (float z = -h / 2.0f; z <= h / 2.0f; z += VoxelResolution)
                {
                    float bz = z * 0.8f;
                    rotor += Voxels.voxSphere(new Vector3(bx, by, bz), BladeHeight);
                }
            }

            return rotor;
        }

        public Voxels CreateShaft()
        {
            Voxels shaft = new Voxels();
            float r = ShaftRadius;
            float h = HousingLength * 1.2f;

            for (float z = -h / 2.0f; z <= h / 2.0f; z += VoxelResolution)
            {
                for (float x = -r; x <= r; x += VoxelResolution)
                for (float y = -r; y <= r; y += VoxelResolution)
                {
                    if (x * x + y * y <= r * r)
                    {
                        shaft += Voxels.voxSphere(new Vector3(x, y, z), VoxelResolution);
                    }
                }
            }

            return shaft;
        }

        public Voxels Create()
        {
            Voxels turbopump = new Voxels();

            turbopump += CreateHousing();
            turbopump += CreateRotor();
            turbopump += CreateShaft();

            return turbopump;
        }
    }
}
