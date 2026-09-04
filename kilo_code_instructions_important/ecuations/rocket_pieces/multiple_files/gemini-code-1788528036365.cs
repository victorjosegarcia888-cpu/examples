using System;
using System.Numerics;
using PicoGK;

namespace RocketEngineDesign
{
    public struct ManifoldResult
    {
        public Voxels VoxSolidShell;
        public Voxels VoxFluidDomain;
    }

    public static class ManifoldGenerator
    {
        public static ManifoldResult CreateToroidalManifold(
            float fCenterZ,
            float fMajorRadius,
            float fOuterPipeRadius,
            float fWallThickness,
            int nFeedPorts = 2,
            float fPortLengthMm = 30.0f,
            int nRadialSteps = 90)
        {
            Lattice latticeOuter = new Lattice();
            Lattice latticeInner = new Lattice();

            float fInnerPipeRadius = fOuterPipeRadius - fWallThickness;
            float dTh = 2.0f * (float)Math.PI / nRadialSteps;

            for (int j = 0; j < nRadialSteps; j++)
            {
                float a0 = j * dTh;
                float a1 = (j + 1) * dTh;

                Vector3 p0 = new Vector3(fMajorRadius * (float)Math.Cos(a0), fMajorRadius * (float)Math.Sin(a0), fCenterZ);
                Vector3 p1 = new Vector3(fMajorRadius * (float)Math.Cos(a1), fMajorRadius * (float)Math.Sin(a1), fCenterZ);

                latticeOuter.AddBeam(p0, p1, fOuterPipeRadius, fOuterPipeRadius);
                latticeInner.AddBeam(p0, p1, fInnerPipeRadius, fInnerPipeRadius);
            }

            float fPortDeltaAngle = 2.0f * (float)Math.PI / nFeedPorts;
            for (int k = 0; k < nFeedPorts; k++)
            {
                float portAngle = k * fPortDeltaAngle;
                Vector3 vDir = new Vector3((float)Math.Cos(portAngle), (float)Math.Sin(portAngle), 0.0f);

                Vector3 pStart = new Vector3(fMajorRadius * vDir.X, fMajorRadius * vDir.Y, fCenterZ);
                Vector3 pEnd = pStart + vDir * fPortLengthMm;

                latticeOuter.AddBeam(pStart, pEnd, fOuterPipeRadius, fOuterPipeRadius);
                latticeInner.AddBeam(pStart, pEnd, fInnerPipeRadius, fInnerPipeRadius);
            }

            ManifoldResult result;
            result.VoxSolidShell = new Voxels(latticeOuter);
            result.VoxFluidDomain = new Voxels(latticeInner);
            return result;
        }
    }
}