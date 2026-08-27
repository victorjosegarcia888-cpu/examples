// EngineIntegrator.cs
//
// Integrador del motor FFSC completo.
// Combina todos los subsistemas en una sola malla exportable.

using PicoGK;
using System.Numerics;
using EngineFFSC.Igniters;
using EngineFFSC.Preburners;
using EngineFFSC.Turbopumps;
using EngineFFSC.CombustionChamber;
using EngineFFSC.Materials;
using EngineFFSC.Geometry;

namespace EngineFFSC.EngineAssembly
{
    public class EngineIntegrator
    {
        public Voxels FullEngine { get; private set; } = new Voxels();
        public bool ExportToSTL { get; set; } = true;
        public string ExportFilename { get; set; } = "FFSC_Engine_Full.stl";
        public float VoxelSize { get; set; } = 0.0005f;

        public void Integrate(
            IgniterVectorModel igniterVector,
            IgniterVoxelModel igniterVoxel,
            ORPBModel orpb,
            FRPBModel frpb,
            TurbopumpVoxelModel turbopumpVoxel,
            TurbopumpVectorModel turbopumpVector,
            NozzleGeometry nozzle)
        {
            FullEngine = new Voxels();

            if (igniterVector != null)
                FullEngine += igniterVector.Create();

            if (igniterVoxel != null)
                FullEngine += igniterVoxel.Create();

            if (orpb != null)
                FullEngine += orpb.Create();

            if (frpb != null)
            {
                FullEngine += Voxels.voxSphere(new Vector3(0.15f, 0, 0), 0.0005f);
                FullEngine += frpb.Create();
            }

            if (turbopumpVoxel != null)
                FullEngine += turbopumpVoxel.Create();

            if (turbopumpVector != null)
            {
                FullEngine += Voxels.voxSphere(new Vector3(0.30f, 0, 0), 0.0005f);
                FullEngine += turbopumpVector.Create();
            }

            if (nozzle != null)
            {
                FullEngine += Voxels.voxSphere(new Vector3(0, 0, -0.60f), 0.0005f);
                FullEngine += nozzle.Create();
            }
        }

        public void AddGrid(Voxels grid)
        {
            FullEngine += grid;
        }

        public void AddQuasicrystal(Voxels qc)
        {
            FullEngine += qc;
        }

        public void Export(string filename)
        {
            ExportFilename = filename;

            Library.Go(VoxelSize, () =>
            {
                Library.oViewer().Add(FullEngine);
            });
        }

        public string GetStatistics()
        {
            return $"Engine Voxels: {FullEngine.ToString()} | Export: {ExportFilename} | VoxelSize: {VoxelSize}";
        }
    }
}
