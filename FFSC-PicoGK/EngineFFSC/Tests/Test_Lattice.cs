// Test_Lattice.cs
//
// Pruebas unitarias para generacion de lattice.

using PicoGK;
using FFSC_PicoGK.Physics.Stress;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.Geometry.Manifolds;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public static class Test_Lattice
    {
        public static bool Run()
        {
            bool passed = true;

            var camara = Geometry_Chamber.Create(
                new EngineParams
                {
                    ChamberRadius = 0.35,
                    ChamberLength = 0.50,
                    ThroatRadius = 0.12,
                    Lstar = 1.2,
                    ContractionRatio = 6.0
                });

            var spike = Geometry_Aerospike.Create(0.55, 0.15);
            var manifold = Geometry_Manifold_FFSC.Create();

            var stress = StressField.Static(camara, spike, manifold);
            passed &= stress != null;

            var lattice = Lattice_DualLayer.Generate(stress, 0.6, 0.3, 0.015, 0.008);
            passed &= lattice != null;

            var quasi = Lattice_Quasicrystal.Generate(stress, 0.3, 0.5);
            passed &= quasi != null;

            return passed;
        }
    }
}
