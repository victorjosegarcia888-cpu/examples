// Geometry_Manifold_CH4_v06.cs
//
// Geometria del colector CH4 para FFSC v06.
// Usando PicoGK Voxels API: voxSphere para manifold de combustible.
//
// Teoria:
// - Distribucion de flujo de metano liquido
// - Premezclado parcial para combustion estabilizada
// - Control de temperatura mediante recirculacion

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Manifold_CH4_v06
    {
        public static Voxels Create(double radius = 0.18, double length = 0.35, int branches = 8)
        {
            Voxels manifold = new Voxels();

            // Cuerpo principal del manifold CH4
            float Rmain = (float)radius;
            float rTube = (float)(radius * 0.12);
            float zCenter = (float)(length * 0.5);

            // Seccion principal rectangular con transicion circular
            int segments = 32;
            for (int i = 0; i < segments; i++)
            {
                float theta = (float)(i * 2.0 * Math.PI / segments);
                float x = Rmain * (float)Math.Cos(theta);
                float y = Rmain * (float)Math.Sin(theta);
                
                manifold += Voxels.voxSphere(new Vector3(x, y, zCenter), rTube);
            }

            // Ramas de alimentacion a inyectores
            float branchLength = (float)(length * 0.5);
            
            for (int i = 0; i < branches; i++)
            {
                float branchAngle = (float)(i * 2.0 * Math.PI / branches);
                float bx = Rmain * (float)Math.Cos(branchAngle);
                float by = Rmain * (float)Math.Sin(branchAngle);

                // Rama de alimentacion
                for (int j = 0; j <= 6; j++)
                {
                    float t = j / 6.0f;
                    float z = zCenter + t * branchLength;
                    float r = rTube * (1.0f - t * 0.2f);
                    manifold += Voxels.voxSphere(new Vector3(bx, by, z), r);
                }
            }

            return manifold;
        }
    }
}
