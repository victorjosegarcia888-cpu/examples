// TurbopumpVectorModel.cs
//
// Modelado vectorial de turbobomba FFSC.
// Define curvas de palas y geometria de impeller.

using PicoGK;
using System.Numerics;
using EngineFFSC.Geometry;

namespace EngineFFSC.Turbopumps
{
    public class TurbopumpVectorModel
    {
        public float ImpellerRadius { get; set; } = 0.050f;
        public float HubRadius { get; set; } = 0.012f;
        public float BladeHeight { get; set; } = 0.015f;
        public int BladeCount { get; set; } = 12;
        public float BackfaceRadius { get; set; } = 0.055f;

        public Voxels CreateImpeller()
        {
            Voxels impeller = new Voxels();

            Voxels disk = ShapeKernelExtensions.CreateSphere(ImpellerRadius);
            impeller += disk;

            for (int i = 0; i < BladeCount; i++)
            {
                float angle = 2.0f * MathF.PI * i / BladeCount;
                float bx = (float)MathF.Cos(angle) * (HubRadius + BladeHeight / 2.0f);
                float by = (float)MathF.Sin(angle) * (HubRadius + BladeHeight / 2.0f);

                Voxels blade = ShapeKernelExtensions.CreateCone(BladeHeight, BladeHeight * 0.3f, ImpellerRadius);
                blade = blade + Voxels.voxSphere(new Vector3(bx, by, 0), 0.0001f);
                impeller += blade;
            }

            return impeller;
        }

        public Voxels CreateDiffuser()
        {
            Voxels diffuser = new Voxels();
            float r = ImpellerRadius * 1.15f;

            Voxels volute = ShapeKernelExtensions.CreateTorus(r, BladeHeight * 0.6f);
            diffuser += volute;

            return diffuser;
        }

        public Voxels CreateVolute()
        {
            Voxels volute = new Voxels();
            float r = BackfaceRadius;
            float h = BladeHeight * 0.8f;

            Voxels casing = ShapeKernelExtensions.CreateTorus(r, h);
            volute += casing;

            return volute;
        }

        public Voxels Create()
        {
            Voxels turbopump = new Voxels();

            turbopump += CreateImpeller();
            turbopump += CreateDiffuser();
            turbopump += CreateVolute();

            return turbopump;
        }
    }
}
