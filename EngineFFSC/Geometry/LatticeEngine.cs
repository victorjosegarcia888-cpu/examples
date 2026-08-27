// LatticeEngine.cs
//
// Motor de rejillas lattice para el motor FFSC.
// Integra Diamond, Gyroid y otros patrones TPMS.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class LatticeEngine
    {
        public static Voxels GenerateDiamond(
            Voxels baseShape,
            float cellSize = 0.010f,
            float thickness = 0.003f)
        {
            Voxels lattice = new Voxels();
            float r = cellSize * 0.25f;
            float spacing = cellSize;

            for (float x = -0.5f; x <= 0.5f; x += spacing)
            for (float y = -0.5f; y <= 0.5f; y += spacing)
            for (float z = -0.5f; z <= 0.5f; z += spacing)
            {
                Vector3 center = new Vector3(x, y, z);
                lattice += Voxels.voxSphere(center, r);
                lattice += Voxels.voxSphere(center + new Vector3(spacing * 0.5f, 0, 0), r);
                lattice += Voxels.voxSphere(center + new Vector3(0, spacing * 0.5f, 0), r);
                lattice += Voxels.voxSphere(center + new Vector3(0, 0, spacing * 0.5f), r);
            }

            return lattice & baseShape;
        }

        public static Voxels GenerateGyroid(
            Voxels baseShape,
            float period = 0.020f,
            float thickness = 0.004f)
        {
            Voxels gyroid = new Voxels();
            float resolution = 0.002f;

            for (float x = -0.5f; x <= 0.5f; x += resolution)
            for (float y = -0.5f; y <= 0.5f; y += resolution)
            for (float z = -0.5f; z <= 0.5f; z += resolution)
            {
                float gx = (float)MathF.Sin(2.0f * MathF.PI * x / period) * (float)MathF.Cos(2.0f * MathF.PI * y / period);
                float gy = (float)MathF.Sin(2.0f * MathF.PI * y / period) * (float)MathF.Cos(2.0f * MathF.PI * z / period);
                float gz = (float)MathF.Sin(2.0f * MathF.PI * z / period) * (float)MathF.Cos(2.0f * MathF.PI * x / period);
                float value = gx + gy + gz;

                if (MathF.Abs(value) < thickness)
                {
                    gyroid += Voxels.voxSphere(new Vector3(x, y, z), resolution * 0.8f);
                }
            }

            return gyroid & baseShape;
        }

        public static Voxels GenerateSchwarz(
            Voxels baseShape,
            float period = 0.020f,
            float thickness = 0.004f)
        {
            Voxels schwarz = new Voxels();
            float resolution = 0.002f;

            for (float x = -0.5f; x <= 0.5f; x += resolution)
            for (float y = -0.5f; y <= 0.5f; y += resolution)
            for (float z = -0.5f; z <= 0.5f; z += resolution)
            {
                float gx = (float)MathF.Cos(2.0f * MathF.PI * x / period);
                float gy = (float)MathF.Cos(2.0f * MathF.PI * y / period);
                float gz = (float)MathF.Cos(2.0f * MathF.PI * z / period);
                float value = gx + gy + gz;

                if (value > 2.0f - thickness)
                {
                    schwarz += Voxels.voxSphere(new Vector3(x, y, z), resolution * 0.8f);
                }
            }

            return schwarz & baseShape;
        }

        public static Voxels GenerateIsoTpms(
            Voxels baseShape,
            string pattern = "Gyroid",
            float period = 0.020f,
            float thickness = 0.004f)
        {
            return pattern.ToLower() switch
            {
                "diamond" => GenerateDiamond(baseShape, period, thickness),
                "gyroid" => GenerateGyroid(baseShape, period, thickness),
                "schwarz" => GenerateSchwarz(baseShape, period, thickness),
                _ => GenerateGyroid(baseShape, period, thickness)
            };
        }
    }
}
