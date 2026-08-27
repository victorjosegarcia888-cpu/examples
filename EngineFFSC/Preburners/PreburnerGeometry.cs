// PreburnerGeometry.cs
//
// Geometria unificada de prequemadores ORPB y FRPB.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Preburners
{
    public static class PreburnerGeometry
    {
        public static Voxels AssembleORPB()
        {
            ORPBModel orpb = new ORPBModel();
            return orpb.Create();
        }

        public static Voxels AssembleFRPB()
        {
            FRPBModel frpb = new FRPBModel();
            return frpb.Create();
        }

        public static Voxels AssembleBoth()
        {
            Voxels both = new Voxels();

            ORPBModel orpb = new ORPBModel();
            FRPBModel frpb = new FRPBModel();

            both += orpb.Create();
            both += Voxels.voxSphere(new Vector3(0.15f, 0, 0), 0.001f);
            both += frpb.Create();

            return both;
        }
    }
}
