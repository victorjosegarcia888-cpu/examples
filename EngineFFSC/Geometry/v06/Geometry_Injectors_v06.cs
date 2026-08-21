// Geometry_Injectors_v06.cs
//
// Geometria del conjunto de inyectores FFSC v06.
// Usando PicoGK Voxels API: voxSphere para elementos coaxiales.
//
// Teoria:
// - Patron de inyectores: distribucion optima para combustion uniforme
// - Atomizacion: breakup de chorros liquidos
// - Impingement coaxial: colision de chorros LOX/CH4

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Injectors_v06
    {
        public static Voxels Create(int count = 60, double pitch = 0.006)
        {
            Voxels injectors = new Voxels();

            float p = (float)pitch;
            int gridSize = (int)Math.Ceiling(Math.Sqrt(count));
            float plateSize = gridSize * p;
            float plateThickness = 0.02f;

            // Plato de inyectores (placa base)
            for (float x = -plateSize / 2; x <= plateSize / 2; x += p * 0.3f)
            {
                for (float y = -plateSize / 2; y <= plateSize / 2; y += p * 0.3f)
                {
                    injectors += Voxels.voxSphere(new Vector3(x, y, 0), plateThickness * 0.5f);
                }
            }

            // Elementos coaxiales (injector elements)
            int elementCount = Math.Min(count, gridSize * gridSize);
            int idx = 0;
            
            for (int ix = 0; ix < gridSize && idx < elementCount; ix++)
            {
                for (int iy = 0; iy < gridSize && idx < elementCount; iy++)
                {
                    float cx = (ix - gridSize / 2) * p;
                    float cy = (iy - gridSize / 2) * p;
                    
                    // Elemento central (poste)
                    float postRadius = p * 0.25f;
                    float postHeight = 0.04f;
                    for (float z = 0; z <= postHeight; z += postRadius * 0.5f)
                    {
                        injectors += Voxels.voxSphere(new Vector3(cx, cy, z), postRadius);
                    }
                    
                    // Anillo LOX (externo)
                    float loxRadius = p * 0.4f;
                    float loxHeight = 0.03f;
                    for (float z = postHeight; z <= postHeight + loxHeight; z += loxRadius * 0.4f)
                    {
                        injectors += Voxels.voxSphere(new Vector3(cx, cy, z), loxRadius);
                    }
                    
                    // Anillo CH4 (interno, mas pequeno)
                    float ch4Radius = p * 0.15f;
                    float ch4Height = 0.025f;
                    for (float z = postHeight; z <= postHeight + ch4Height; z += ch4Radius * 0.4f)
                    {
                        injectors += Voxels.voxSphere(new Vector3(cx, cy, z), ch4Radius);
                    }
                    
                    idx++;
                }
            }

            return injectors;
        }
    }
}
