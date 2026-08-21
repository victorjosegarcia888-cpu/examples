// Lattice_DualLayer.cs
//
// Lattice estructural de doble capa basado en campo de tensiones.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Chamber
{
    public static class Lattice_DualLayer
    {
        public static Voxels Generate(
            Voxels stressField,
            double umbralGrueso = 0.6,
            double umbralFino = 0.3,
            double radioGrueso = 0.015,
            double radioFino = 0.008)
        {
            Voxels latticeGrueso = new Voxels();
            Voxels latticeFino = new Voxels();

            // Generate dual-layer lattice from stress field bounding box
            var bbox = stressField.oCalculateBoundingBox();
            var center = bbox.vecCenter();
            var size = bbox.vecSize();

            // High-stress layer: large spheres, sparse
            for (int i = 0; i < 20; i++)
            {
                float x = center.X + (float)((i / 10.0 - 0.5) * size.X * 0.5);
                float y = center.Y + (float)((i % 5 - 2) * 0.05);
                float z = center.Z + (float)((i % 3 - 1) * 0.1);
                latticeGrueso += Voxels.voxSphere(new Vector3(x, y, z), (float)radioGrueso);
            }

            // Low-stress layer: small spheres, dense
            for (int i = 0; i < 40; i++)
            {
                float x = center.X + (float)((i / 20.0 - 0.5) * size.X * 0.8);
                float y = center.Y + (float)((i % 10 - 5) * 0.03);
                float z = center.Z + (float)((i % 4 - 2) * 0.05);
                latticeFino += Voxels.voxSphere(new Vector3(x, y, z), (float)radioFino);
            }

            return latticeGrueso + latticeFino;
        }
    }
}
