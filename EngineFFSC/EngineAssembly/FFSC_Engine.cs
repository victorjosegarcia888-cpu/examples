// FFSC_Engine.cs
//
// Clase principal del motor FFSC.
// Orquesta todos los subsistemas para construir el motor completo.

using PicoGK;
using System.Numerics;
using EngineFFSC.Igniters;
using EngineFFSC.Preburners;
using EngineFFSC.Turbopumps;
using EngineFFSC.CombustionChamber;
using EngineFFSC.Geometry;
using EngineFFSC.Materials;

namespace EngineFFSC.EngineAssembly
{
    public class FFSC_Engine
    {
        public Voxels EngineVoxels { get; private set; } = new Voxels();
        public string ExportPath { get; set; } = "FFSC_engine.stl";
        public bool IncludeIgniter { get; set; } = true;
        public bool IncludePreburners { get; set; } = true;
        public bool IncludeTurbopump { get; set; } = true;
        public bool IncludeChamber { get; set; } = true;
        public bool IncludeNozzle { get; set; } = true;
        public bool IncludeLattice { get; set; } = true;
        public bool IncludeQuasicrystal { get; set; } = false;
        public float Scale { get; set; } = 1.0f;

        private InconelA286 _material;

        public FFSC_Engine()
        {
            _material = AlloyDatabase.Get("Inconel_A286");
        }

        public void SetMaterial(string name)
        {
            _material = AlloyDatabase.Get(name);
        }

        public void BuildIgniter()
        {
            if (!IncludeIgniter) return;

            IgniterVectorModel vectorIgniter = new IgniterVectorModel();
            IgniterVoxelModel voxelIgniter = new IgniterVoxelModel();

            Voxels igniter = vectorIgniter.Create() + voxelIgniter.Create();
            EngineVoxels += igniter;
        }

        public void BuildPreburners()
        {
            if (!IncludePreburners) return;

            ORPBModel orpb = new ORPBModel();
            FRPBModel frpb = new FRPBModel();

            Voxels preburners = orpb.Create();
            preburners += Voxels.voxSphere(new Vector3(0.15f, 0, 0), 0.0005f);
            preburners += frpb.Create();

            EngineVoxels += preburners;
        }

        public void BuildTurbopump()
        {
            if (!IncludeTurbopump) return;

            TurbopumpVoxelModel voxelPump = new TurbopumpVoxelModel();
            TurbopumpVectorModel vectorPump = new TurbopumpVectorModel();

            Voxels turbopump = voxelPump.Create() + vectorPump.Create();
            EngineVoxels += turbopump;
        }

        public void BuildChamber()
        {
            if (!IncludeChamber) return;

            Voxels chamber = new Voxels();
            float Rc = 0.35f * Scale;
            float Lc = 0.50f * Scale;
            float Rt = 0.12f * Scale;

            chamber += Voxels.voxSphere(new Vector3(0, 0, Lc * 0.5f), Rc);
            chamber += Voxels.voxSphere(new Vector3(0, 0, Lc * 0.75f), Rc);
            chamber += Voxels.voxSphere(new Vector3(0, 0, Lc), Rc);

            for (int i = 0; i < 8; i++)
            {
                float t = i / 8.0f;
                float r = Rc * (1.0f - t * 0.5f);
                float z = Lc + t * Lc * 0.5f;
                chamber += Voxels.voxSphere(new Vector3(0, 0, z), r);
            }

            EngineVoxels += chamber;
        }

        public void BuildNozzle()
        {
            if (!IncludeNozzle) return;

            NozzleGeometry nozzle = new NozzleGeometry
            {
                ThroatRadius = 0.12f * Scale,
                ExitRadius = 0.80f * Scale,
                NozzleLength = 1.20f * Scale,
                VoxelResolution = 0.0005f * Scale
            };

            EngineVoxels += nozzle.Create();
        }

        public void BuildLattice()
        {
            if (!IncludeLattice) return;

            ChamberGrid grid = new ChamberGrid();
            Voxels lattice = grid.CreateDualLayerGrid(EngineVoxels);

            EngineVoxels += lattice;
        }

        public void BuildQuasicrystal()
        {
            if (!IncludeQuasicrystal) return;

            Voxels qc = QuasicrystalStructures.GeneratePenrose(EngineVoxels, 0.015f * Scale, 0.002f * Scale);
            EngineVoxels += qc;
        }

        public void Assemble()
        {
            EngineVoxels = new Voxels();

            BuildIgniter();
            BuildPreburners();
            BuildTurbopump();
            BuildChamber();
            BuildNozzle();
            BuildLattice();
            BuildQuasicrystal();
        }

        public void Export(string path)
        {
            Library.Go(0.5f * Scale, () =>
            {
                Library.oViewer().Add(EngineVoxels);
            });
        }

        public void ExportSTL(string filename)
        {
            Library.Go(0.5f * Scale, () =>
            {
                Library.oViewer().Add(EngineVoxels);
            });
        }

        public string GetMaterialInfo()
        {
            return $"Material: {_material.Name} | Yield: {_material.YieldStrength_Pa / 1e9:F1} GPa | Density: {_material.Density_kg_m3} kg/m3 | Tmax: {_material.MaxServiceTemp_C} C";
        }
    }
}
