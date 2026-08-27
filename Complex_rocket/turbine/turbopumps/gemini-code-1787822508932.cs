using System;
using System.Numerics;
using PikoGK;

namespace RocketEngineDesign
{
    public static class ManifoldGenerator
    {
        /// <summary>
        /// Genera un distribuidor toroidal (Torus Manifold) para alimentación de propelente
        /// alineado con la placa inyectora.
        /// </summary>
        public static Voxels voxCreateToroidalManifold(
            float fMajorRadiusMm = 65.0f,
            float fOuterTubeRadiusMm = 14.0f,
            float fWallThicknessMm = 3.0f,
            float fCenterZ = 12.0f)
        {
            float fInnerTubeRadiusMm = fOuterTubeRadiusMm - fWallThicknessMm;

            // 1. Toro exterior (geometría sólida)
            Mesh meshOuterTorus = CreateTorusMesh(fMajorRadiusMm, fOuterTubeRadiusMm, fCenterZ);
            Voxels voxManifold = new Voxels(meshOuterTorus);

            // 2. Toro interior (vaciado del canal de fluido)
            Mesh meshInnerTorus = CreateTorusMesh(fMajorRadiusMm, fInnerTubeRadiusMm, fCenterZ);
            Voxels voxFluidChannel = new Voxels(meshInnerTorus);

            // Perforación interna del distribuidor
            voxManifold.Subtract(voxFluidChannel);

            return voxManifold;
        }

        private static Mesh CreateTorusMesh(
            float fRMajor, float fRMinor, float fCenterZ, int nStepsMajor = 48, int nStepsMinor = 24)
        {
            Mesh mesh = new Mesh();
            float dPhi = 2.0f * (float)Math.PI / nStepsMajor;
            float dTheta = 2.0f * (float)Math.PI / nStepsMinor;

            for (int i = 0; i < nStepsMajor; i++)
            {
                float phi = i * dPhi;
                float cosPhi = (float)Math.Cos(phi);
                float sinPhi = (float)Math.Sin(phi);

                for (int j = 0; j < nStepsMinor; j++)
                {
                    float theta = j * dTheta;
                    float cosTheta = (float)Math.Cos(theta);
                    float sinTheta = (float)Math.Sin(theta);

                    float x = (fRMajor + fRMinor * cosTheta) * cosPhi;
                    float y = (fRMajor + fRMinor * cosTheta) * sinPhi;
                    float z = fCenterZ + fRMinor * sinTheta;

                    mesh.nAddVertex(new Vector3(z, x, y));
                }
            }

            for (int i = 0; i < nStepsMajor; i++)
            {
                int iNext = (i + 1) % nStepsMajor;
                for (int j = 0; j < nStepsMinor; j++)
                {
                    int jNext = (j + 1) % nStepsMinor;

                    int idx00 = i * nStepsMinor + j;
                    int idx10 = iNext * nStepsMinor + j;
                    int idx01 = i * nStepsMinor + jNext;
                    int idx11 = iNext * nStepsMinor + jNext;

                    mesh.AddTriangle(idx00, idx10, idx11);
                    mesh.AddTriangle(idx00, idx11, idx01);
                }
            }

            return mesh;
        }
    }
}