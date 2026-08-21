// FFSC_Pipeline_Advanced.cs
//
// Pipeline completo estilo LEAP71 para el motor FFSC.
//
// Flujo:
// 1. Cargar parametros del motor
// 2. Computar termoquimica (Tad, hg, Qnorm)
// 3. Computar espesor estructural
// 4. Disenar turbobomba
// 5. Generar geometria de subsistemas
// 6. Computar campos fisicos (stress, CFD, cooling)
// 7. Generar lattice adaptativo
// 8. Ensamblar motor completo
// 9. Exportar resultados
//
// Cita PDF:
// "El pipeline LEAP71 integra diseno geometrico, analisis fisico
//  y optimizacion en un flujo continuo y automatizado."

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Structural;
using FFSC_PicoGK.Physics.Cooling;
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
    /// <summary>
    /// Pipeline completo del motor FFSC.
    /// </summary>
    public static class FFSC_Pipeline_Advanced
    {
        /// <summary>
        /// Ejecuta el pipeline completo.
        /// </summary>
        /// <param name="p">Parametros del motor</param>
        /// <returns>Field3D con el motor completo</returns>
        public static Field3D Execute(EngineParams p)
        {
            // Fase 1: Termoquimica
            ThermoMap thermo = ComputeThermoTask.Run(p);

            // Fase 2: Espesor estructural
            ThicknessMap thickness = ComputeThicknessTask.Run(p, thermo);

            // Fase 3: Diseno de turbobomba
            TurbopumpDesign pump = TurbopumpDesigner.Run(p, p.MassFlowOxidizer);

            // Fase 4: Geometria de subsistemas
            Field3D camara = Geometry_Chamber.Create(p);
            Field3D nozzle = Geometry_Nozzle.Create(p);
            Field3D spike = Geometry_Aerospike.Create(p);
            Field3D manifold = Geometry_Manifold_FFSC.Create();
            Field3D loxManifold = Geometry_Manifold_LOX.Create();
            Field3D ch4Manifold = Geometry_Manifold_CH4.Create();
            Field3D injectors = Geometry_Injectors.Create();
            Field3D turbopumpGeom = Geometry_Turbopump.Create();
            Field3D coolingPrimary = Geometry_Cooling.Primary(camara, spike);
            Field3D coolingSecondary = Geometry_Cooling.Secondary(camara, spike);
            Field3D pipes = Geometry_Pipes.Create();
            Field3D structural = Geometry_Structural.Create();
            Field3D supports = Geometry_Supports.Create();

            // Fase 5: Campos fisicos
            Field3D stress = StressField.Dynamic(camara, spike, manifold);
            Field3D thermal = CampoTermico.Dynamic(camara, spike);
            Field3D cfd = CFDTask.Dynamic(Field3D.Combine(camara, nozzle, spike, manifold));

            // Fase 6: Lattice adaptativo
            Field3D latticeDual = Lattice_DualLayer.Generate(stress, p.StressThresholdHigh, p.StressThresholdLow, 0.015, 0.008);
            Field3D latticeQuasi = Lattice_Quasicrystal.Generate(stress, 0.3, 0.5);

            // Fase 7: Refrigeracion
            CoolingMap coolingMap = CoolingTask.Run(p, thermo);

            // Fase 8: Ensamblado final
            Field3D ensamblado = Field3D.Combine(
                camara, nozzle, spike, manifold,
                loxManifold, ch4Manifold,
                injectors, turbopumpGeom,
                coolingPrimary, coolingSecondary,
                pipes, structural, supports,
                latticeDual, latticeQuasi,
                stress, thermal, cfd
            );

            return ensamblado;
        }
    }
}
