// QuasicrystalStructures.cs
//
// Estructuras cuasicristalinas para refuerzo y amortiguamiento.
// Basado en Penrose (5-fold) y Ammann-Beenker (8-fold).

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class QuasicrystalStructures
    {
        public static Voxels GeneratePenrose(
            Voxels baseShape,
            float scale = 0.015f,
            float thickness = 0.002f)
        {
            Voxels penrose = new Voxels();
            float resolution = 0.0015f;
            float angle = MathF.PI / 5.0f;

            for (float x = -0.5f; x <= 0.5f; x += resolution)
            for (float y = -0.5f; y <= 0.5f; y += resolution)
            for (float z = -0.5f; z <= 0.5f; z += resolution)
            {
                float r = (float)Math.Sqrt(x * x + y * y);
                float theta = (float)Math.Atan2(y, x);

                float pattern = 0.0f;
                for (int i = 0; i < 5; i++)
                {
                    float a = theta + i * angle;
                    float proj = (float)Math.Cos(a) * r;
                    float wave = (float)Math.Sin(2.0f * MathF.PI * proj / scale);
                    pattern += wave;
                }

                if (MathF.Abs(pattern) < thickness * 10.0f)
                {
                    penrose += Voxels.voxSphere(new Vector3(x, y, z), resolution * 0.7f);
                }
            }

            return penrose & baseShape;
        }

        public static Voxels GenerateAmmannBeenker(
            Voxels baseShape,
            float scale = 0.015f,
            float thickness = 0.002f)
        {
            Voxels ab = new Voxels();
            float resolution = 0.0015f;
            float angle = MathF.PI / 4.0f;

            for (float x = -0.5f; x <= 0.5f; x += resolution)
            for (float y = -0.5f; y <= 0.5f; y += resolution)
            for (float z = -0.5f; z <= 0.5f; z += resolution)
            {
                float r = (float)Math.Sqrt(x * x + y * y);
                float theta = (float)Math.Atan2(y, x);

                float pattern = 0.0f;
                for (int i = 0; i < 8; i++)
                {
                    float a = theta + i * angle;
                    float proj = (float)Math.Cos(a) * r;
                    float wave = (float)Math.Sin(2.0f * MathF.PI * proj / scale);
                    pattern += wave;
                }

                if (MathF.Abs(pattern) < thickness * 10.0f)
                {
                    ab += Voxels.voxSphere(new Vector3(x, y, z), resolution * 0.7f);
                }
            }

            return ab & baseShape;
        }

        public static Voxels GenerateOctagonal(
            Voxels baseShape,
            float scale = 0.012f,
            float thickness = 0.003f)
        {
            Voxels octa = new Voxels();
            float resolution = 0.002f;

            for (float x = -0.5f; x <= 0.5f; x += resolution)
            for (float y = -0.5f; y <= 0.5f; y += resolution)
            for (float z = -0.5f; z <= 0.5f; z += resolution)
            {
                float px = (float)MathF.Cos(2.0f * MathF.PI * x / scale);
                float py = (float)MathF.Cos(2.0f * MathF.PI * y / scale);
                float pz = (float)MathF.Cos(2.0f * MathF.PI * z / scale);

                float pxy = (float)MathF.Cos(2.0f * MathF.PI * (x + y) / (scale * 1.414f));
                float pyz = (float)MathF.Cos(2.0f * MathF.PI * (y + z) / (scale * 1.414f));
                float pzx = (float)MathF.Cos(2.0f * MathF.PI * (z + x) / (scale * 1.414f));

                float value = px + py + pz + pxy + pyz + pzx;

                if (MathF.Abs(value - 3.0f) < thickness * 8.0f)
                {
                    octa += Voxels.voxSphere(new Vector3(x, y, z), resolution * 0.8f);
                }
            }

            return octa & baseShape;
        }

        public static Voxels GenerateDodecagonal(
            Voxels baseShape,
            float scale = 0.012f,
            float thickness = 0.003f)
        {
            Voxels dodeca = new Voxels();
            float resolution = 0.002f;

            for (float x = -0.5f; x <= 0.5f; x += resolution)
            for (float y = -0.5f; y <= 0.5f; y += resolution)
            for (float z = -0.5f; z <= 0.5f; z += resolution)
            {
                float px = (float)MathF.Cos(2.0f * MathF.PI * x / scale);
                float py = (float)MathF.Cos(2.0f * MathF.PI * y / scale);
                float pz = (float)MathF.Cos(2.0f * MathF.PI * z / scale);
                float pm = (float)MathF.Cos(2.0f * MathF.PI * (x + y + z) / (scale * 1.732f));

                float value = px + py + pz + pm;

                if (MathF.Abs(value - 2.0f) < thickness * 8.0f)
                {
                    dodeca += Voxels.voxSphere(new Vector3(x, y, z), resolution * 0.8f);
                }
            }

            return dodeca & baseShape;
        }

        public static Voxels Generate(
            Voxels baseShape,
            string type = "Penrose",
            float scale = 0.015f,
            float thickness = 0.002f)
        {
            return type.ToLower() switch
            {
                "penrose" => GeneratePenrose(baseShape, scale, thickness),
                "ammannbeenker" => GenerateAmmannBeenker(baseShape, scale, thickness),
                "octagonal" => GenerateOctagonal(baseShape, scale, thickness),
                "dodecagonal" => GenerateDodecagonal(baseShape, scale, thickness),
                _ => GeneratePenrose(baseShape, scale, thickness)
            };
        }
    }
}
