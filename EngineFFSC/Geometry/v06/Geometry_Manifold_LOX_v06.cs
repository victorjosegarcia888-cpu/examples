// Geometry_Manifold_LOX_v06.cs
//
// Geometria del colector LOX para FFSC v06.
// Usando PicoGK Voxels API: voxSphere para cuerpo toroidal y ramas Y.
//
// Teoria:
// - Bifurcacion en Y (Y-bifurcation) para distribucion de flujo
// - Colector toroidal para distribucion uniforme de presion
// - Caida de presion y balanceo de flujo entre ramas

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Manifold_LOX_v06
    {
        public static Voxels Create(double radius = 0.2, double length = 0.4, int branches = 6)
        {
            Voxels manifold = new Voxels();

            // Cuerpo principal toroidal (colector anular)
            float Rtoroidal = (float)radius;
            float rTube = (float)(radius * 0.15);
            float zCenter = (float)(length * 0.5);

            // Generar toroide usando esferas
            int segments = 36;
            for (int i = 0; i < segments; i++)
            {
                float theta = (float)(i * 2.0 * Math.PI / segments);
                float x = Rtoroidal * (float)Math.Cos(theta);
                float y = Rtoroidal * (float)Math.Sin(theta);
                
                manifold += Voxels.voxSphere(new Vector3(x, y, zCenter), rTube);
            }

            // Ramas en Y para distribucion a inyectores
            float branchLength = (float)(length * 0.6);

            for (int i = 0; i < branches; i++)
            {
                float branchAngle = (float)(i * 2.0 * Math.PI / branches);
                float bx = Rtoroidal * (float)Math.Cos(branchAngle);
                float by = Rtoroidal * (float)Math.Sin(branchAngle);

                // Rama principal
                for (int j = 0; j <= 8; j++)
                {
                    float t = j / 8.0f;
                    float z = zCenter + t * branchLength;
                    float r = rTube * (1.0f - t * 0.3f);
                    manifold += Voxels.voxSphere(new Vector3(bx, by, z), r);
                }

                // Bifurcacion en Y
                float yBase = zCenter + branchLength;
                for (int k = 0; k <= 4; k++)
                {
                    float tk = k / 4.0f;
                    float yz = yBase + tk * branchLength * 0.5f;
                    float yr = rTube * 0.8f * (1.0f - tk * 0.4f);
                    
                    // Rama Y-1
                    manifold += Voxels.voxSphere(
                        new Vector3(bx + tk * branchLength * 0.3f, by, yz), yr);
                    // Rama Y-2
                    manifold += Voxels.voxSphere(
                        new Vector3(bx - tk * branchLength * 0.3f, by, yz), yr);
                }
            }

            return manifold;
        }
    }
}
