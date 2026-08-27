// ShapeKernelExtensions.cs
//
// Extensiones de ShapeKernel para integracion con PicoGK.
// Funciones parametricas y superficies NURBS.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class ShapeKernelExtensions
    {
        public static Voxels CreateCylinder(
            float radius,
            float height,
            int segments = 32)
        {
            Voxels cylinder = new Voxels();
            float halfHeight = height / 2.0f;

            for (int i = 0; i < segments; i++)
            {
                float angle = 2.0f * MathF.PI * i / segments;
                float x = (float)MathF.Cos(angle) * radius;
                float y = (float)MathF.Sin(angle) * radius;
                cylinder += Voxels.voxSphere(new Vector3(x, y, 0), radius / segments);
            }

            for (float z = -halfHeight; z <= halfHeight; z += radius / segments)
            {
                for (int i = 0; i < segments; i++)
                {
                    float angle = 2.0f * MathF.PI * i / segments;
                    float x = (float)MathF.Cos(angle) * radius;
                    float y = (float)MathF.Sin(angle) * radius;
                    cylinder += Voxels.voxSphere(new Vector3(x, y, z), radius / segments);
                }
            }

            return cylinder;
        }

        public static Voxels CreateCone(
            float radiusBottom,
            float radiusTop,
            float height,
            int segments = 32)
        {
            Voxels cone = new Voxels();
            float halfHeight = height / 2.0f;

            for (float z = -halfHeight; z <= halfHeight; z += (radiusTop / segments))
            {
                float t = (z + halfHeight) / height;
                float r = radiusBottom * (1.0f - t) + radiusTop * t;

                for (int i = 0; i < segments; i++)
                {
                    float angle = 2.0f * MathF.PI * i / segments;
                    float x = (float)MathF.Cos(angle) * r;
                    float y = (float)MathF.Sin(angle) * r;
                    cone += Voxels.voxSphere(new Vector3(x, y, z), r / segments);
                }
            }

            return cone;
        }

        public static Voxels CreateTorus(
            float majorRadius,
            float minorRadius,
            int segments = 32)
        {
            Voxels torus = new Voxels();
            float resolution = minorRadius / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = 2.0f * MathF.PI * i / segments;
                float cx = (float)MathF.Cos(angle) * majorRadius;
                float cy = (float)MathF.Sin(angle) * majorRadius;

                for (int j = 0; j < segments; j++)
                {
                    float phi = 2.0f * MathF.PI * j / segments;
                    float x = cx + (float)MathF.Cos(phi) * minorRadius;
                    float y = cy + (float)MathF.Sin(phi) * minorRadius;
                    float z = (float)MathF.Sin(phi) * minorRadius;
                    torus += Voxels.voxSphere(new Vector3(x, y, z), resolution);
                }
            }

            return torus;
        }

        public static Voxels CreateHelix(
            float radius,
            float pitch,
            float turns,
            float wireRadius,
            int pointsPerTurn = 32)
        {
            Voxels helix = new Voxels();
            float totalTurns = turns;
            float totalHeight = totalTurns * pitch;
            int totalPoints = (int)(totalTurns * pointsPerTurn);

            for (int i = 0; i < totalPoints; i++)
            {
                float t = (float)i / totalPoints;
                float angle = t * totalTurns * 2.0f * MathF.PI;
                float x = (float)MathF.Cos(angle) * radius;
                float y = (float)MathF.Sin(angle) * radius;
                float z = t * totalHeight - totalHeight / 2.0f;

                helix += Voxels.voxSphere(new Vector3(x, y, z), wireRadius);
            }

            return helix;
        }

        public static Voxels CreateSphere(float radius, int segments = 16)
        {
            Voxels sphere = new Voxels();
            float resolution = radius / segments;

            for (float theta = 0; theta < MathF.PI; theta += MathF.PI / segments)
            for (float phi = 0; phi < 2.0f * MathF.PI; phi += 2.0f * MathF.PI / segments)
            {
                float x = radius * (float)MathF.Sin(theta) * (float)MathF.Cos(phi);
                float y = radius * (float)MathF.Sin(theta) * (float)MathF.Sin(phi);
                float z = radius * (float)MathF.Cos(theta);
                sphere += Voxels.voxSphere(new Vector3(x, y, z), resolution);
            }

            return sphere;
        }

        public static Voxels CreateBox(float width, float height, float depth, float resolution = 0.01f)
        {
            Voxels box = new Voxels();
            float hw = width / 2.0f;
            float hh = height / 2.0f;
            float hd = depth / 2.0f;

            for (float x = -hw; x <= hw; x += resolution)
            for (float y = -hh; y <= hh; y += resolution)
            for (float z = -hd; z <= hd; z += resolution)
            {
                box += Voxels.voxSphere(new Vector3(x, y, z), resolution * 0.9f);
            }

            return box;
        }
    }
}
