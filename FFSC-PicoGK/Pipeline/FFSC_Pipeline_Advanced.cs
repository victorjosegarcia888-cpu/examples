// FFSC_Pipeline_Advanced.cs
//
// Pipeline completo estilo LEAP71 para el motor FFSC.

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Structural;
using FFSC_PicoGK.Physics.CFD;
using FFSC_PicoGK.Physics.Stress;
using FFSC_PicoGK.EngineFFSC.Turbopump;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.Geometry.Manifolds;
using FFSC_PicoGK.Geometry.Injectors;
using FFSC_PicoGK.Geometry.Turbopump;
using FFSC_PicoGK.Geometry.Cooling;
using FFSC_PicoGK.Geometry.Pipes;
using FFSC_PicoGK.Geometry.Structural;
using FFSC_PicoGK.Geometry.Supports;

namespace FFSC_PicoGK.Pipeline
{
    public static class FFSC_Pipeline_Advanced
    {
        public static Voxels Execute(EngineParams p)
        {
            // Fase 1: Termoquimica
            ThermoMap thermo = ComputeThermoTask.Run(p);

            // Fase 2: Espesor estructural
            ThicknessMap thickness = ComputeThicknessTask.Run(p, thermo);

            // Fase 3: Diseno de turbobomba
            TurbopumpDesign pump = TurbopumpDesigner.Run(p, p.MassFlowOxidizer);

            // Fase 4: Geometria
            Voxels camara = Geometry_Chamber.Create(p);
            Voxels nozzle = Geometry_Nozzle.Create(p);
            Voxels spike = Geometry_Aerospike.Create(p);
            Voxels manifold = Geometry_Manifold_FFSC.Create();
            Voxels loxManifold = Geometry_Manifold_LOX.Create();
            Voxels ch4Manifold = Geometry_Manifold_CH4.Create();
            Voxels injectors = Geometry_Injectors.Create();
            Voxels turbopumpGeom = Geometry_Turbopump.Create();
            Voxels coolingPrimary = Geometry_Cooling.Primary(camara, spike);
            Voxels coolingSecondary = Geometry_Cooling.Secondary(camara, spike);
            Voxels pipes = Geometry_Pipes.Create();
            Voxels structural = Geometry_Structural.Create();
            Voxels supports = Geometry_Supports.Create();

            // Fase 5: Campos fisicos
            Voxels stress = StressField.Dynamic(camara, spike, manifold);
            Voxels thermal = CFDTask.Static(camara + spike); // simplified thermal
            Voxels cfd = CFDTask.Dynamic(camara + nozzle + spike + manifold);

            // Fase 6: Lattice
            Voxels latticeDual = Lattice_DualLayer.Generate(stress, p.StressThresholdHigh, p.StressThresholdLow, 0.015, 0.008);
            Voxels latticeQuasi = Lattice_Quasicrystal.Generate(stress, 0.3, 0.5);

            // Fase 7: Ensamblado
            Voxels ensamblado = camara + nozzle + spike + manifold +
                               loxManifold + ch4Manifold +
                               injectors + turbopumpGeom +
                               coolingPrimary + coolingSecondary +
                               pipes + structural + supports +
                               latticeDual + latticeQuasi +
                               stress + thermal + cfd;

            return ensamblado;
        }
    }
}
