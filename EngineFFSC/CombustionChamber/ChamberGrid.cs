// ChamberGrid.cs
//
// Rejilla interna de la camara de combustion FFSC.
// Genera lattice structures para refuerzo estructural.

using PicoGK;
using System.Numerics;
using EngineFFSC.Geometry;

namespace EngineFFSC.CombustionChamber
{
    public class ChamberGrid
    {
        public float GridResolution { get; set; } = 0.004f;
        public float LatticeThickness { get; set; } = 0.001f;
        public string LatticePattern { get; set; } = "Diamond";
        public float SupportThickness { get; set; } = 0.002f;

        public Voxels CreateDualLayerGrid(Voxels chamberShape)
        {
            Voxels outerGrid = LatticeEngine.GenerateDiamond(chamberShape, GridResolution, LatticeThickness);
            Voxels innerGrid = LatticeEngine.GenerateDiamond(chamberShape, GridResolution * 0.5f, LatticeThickness * 0.6f);

            return outerGrid + innerGrid;
        }

        public Voxels CreateQuasicrystalReinforcement(Voxels chamberShape)
        {
            return QuasicrystalStructures.GeneratePenrose(chamberShape, GridResolution * 1.2f, LatticeThickness * 1.5f);
        }

        public Voxels CreateOrthogonalGrid(Voxels chamberShape)
        {
            Voxels grid = new Voxels();
            float res = GridResolution;
            float thick = LatticeThickness;

            for (float x = -0.5f; x <= 0.5f; x += res * 4.0f)
            for (float z = -0.5f; z <= 0.5f; z += res)
            {
                for (float y = -thick; y <= thick; y += res * 0.5f)
                {
                    grid += Voxels.voxSphere(new Vector3(x, y, z), res * 0.4f);
                }
            }

            for (float y = -0.5f; y <= 0.5f; y += res * 4.0f)
            for (float z = -0.5f; z <= 0.5f; z += res)
            {
                for (float x = -thick; x <= thick; x += res * 0.5f)
                {
                    grid += Voxels.voxSphere(new Vector3(x, y, z), res * 0.4f);
                }
            }

            return grid & chamberShape;
        }

        public Voxels CreateRadialRibbed(Voxels chamberShape, int ribCount = 12)
        {
            Voxels ribs = new Voxels();
            float res = GridResolution;

            for (int i = 0; i < ribCount; i++)
            {
                float angle = 2.0f * MathF.PI * i / ribCount;

                for (float r = 0.05f; r <= 0.5f; r += res)
                {
                    float x = (float)MathF.Cos(angle) * r;
                    float y = (float)MathF.Sin(angle) * r;

                    for (float z = -0.5f; z <= 0.5f; z += res)
                    {
                        ribs += Voxels.voxSphere(new Vector3(x, y, z), res * 0.5f);
                    }
                }
            }

            return ribs & chamberShape;
        }

        public Voxels Create(Voxels chamberShape, string pattern = "DualLayer")
        {
            return pattern.ToLower() switch
            {
                "duallayer" => CreateDualLayerGrid(chamberShape),
                "quasicrystal" => CreateQuasicrystalReinforcement(chamberShape),
                "orthogonal" => CreateOrthogonalGrid(chamberShape),
                "radial" => CreateRadialRibbed(chamberShape),
                _ => CreateDualLayerGrid(chamberShape)
            };
        }
    }
}
