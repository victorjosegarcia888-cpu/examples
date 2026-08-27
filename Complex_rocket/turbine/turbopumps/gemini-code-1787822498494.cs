using System;
using System.Numerics;
using PikoGK;

namespace RocketEngineDesign
{
    public static class SwirlerInjectorGenerator
    {
        /// <summary>
        /// Genera un elemento inyector coaxial con álabes de remolino (Swirl Vanes)
        /// para atomización y mezcla de GCH4 y GOX.
        /// </summary>
        public static Voxels voxCreateSwirlInjectorElement(
            float fCoreTubeInnerRadiusMm = 2.0f,
            float fCoreTubeOuterRadiusMm = 3.0f,
            float fSwirlerOuterRadiusMm = 6.0f,
            float fElementLengthMm = 30.0f,
            int nVaneCount = 6,
            float fHelixPitchMm = 25.0f)
        {
            // 1. Tubo central de oxidante (GOX Core Flow Tube)
            Mesh meshCoreOuter = CreateCylinderMesh(fCoreTubeOuterRadiusMm, fElementLengthMm);
            Mesh meshCoreInner = CreateCylinderMesh(fCoreTubeInnerRadiusMm, fElementLengthMm + 4.0f, -2.0f);
            
            Voxels voxCoreTube = new Voxels(meshCoreOuter);
            Voxels voxCoreVoid = new Voxels(meshCoreInner);
            voxCoreTube.Subtract(voxCoreVoid);

            // 2. Manga exterior del paso anular (GCH4 Outer Sleeve)
            Mesh meshSleeveOuter = CreateCylinderMesh(fSwirlerOuterRadiusMm, fElementLengthMm);
            Mesh meshSleeveInner = CreateCylinderMesh(fCoreTubeOuterRadiusMm, fElementLengthMm);
            
            Voxels voxSwirlChamber = new Voxels(meshSleeveOuter);
            Voxels voxSwirlVoid = new Voxels(meshSleeveInner);
            voxSwirlChamber.Subtract(voxSwirlVoid);

            // 3. Generación de los álabes helicoidales (Swirl Vanes)
            Voxels voxVanes = voxCreateHelicalVanes(
                fCoreTubeOuterRadiusMm, 
                fSwirlerOuterRadiusMm, 
                fElementLengthMm, 
                nVaneCount, 
                fHelixPitchMm
            );

            // 4. Integrar álabes dentro del canal anular de combustible
            voxSwirlChamber.Intersect(voxVanes);

            // 5. Unir conducto central y ensamble de remolino exterior
            Voxels voxInjector = new Voxels();
            voxInjector.Add(voxCoreTube);
            voxInjector.Add(voxSwirlChamber);

            return voxInjector;
        }

        private static Voxels voxCreateHelicalVanes(
            float fRInner, float fROuter, float fLength, int nVanes, float fPitchMm)
        {
            Voxels voxVanesGroup = new Voxels();
            float fVaneThicknessMm = 0.8f;
            int nStepsZ = 30;
            float fDeltaZ = fLength / nStepsZ;

            for (int v = 0; v < nVanes; v++)
            {
                float fBaseAngle = v * (2.0f * (float)Math.PI / nVanes);
                Mesh meshVane = new Mesh();

                for (int i = 0; i <= nStepsZ; i++)
                {
                    float fZ = i * fDeltaZ;
                    float fAngle = fBaseAngle + (fZ / fPitchMm) * 2.0f * (float)Math.PI;

                    Vector3 vRadDir = new Vector3((float)Math.Cos(fAngle), (float)Math.Sin(fAngle), 0);
                    Vector3 pInner = new Vector3(fZ, fRInner * vRadDir.X, fRInner * vRadDir.Y);
                    Vector3 pOuter = new Vector3(fZ, fROuter * vRadDir.X, fROuter * vRadDir.Y);

                    meshVane.nAddVertex(pInner);
                    meshVane.nAddVertex(pOuter);
                }

                for (int i = 0; i < nStepsZ; i++)
                {
                    int i0 = i * 2;
                    int i1 = i0 + 1;
                    int i2 = (i + 1) * 2;
                    int i3 = i2 + 1;

                    meshVane.AddTriangle(i0, i1, i3);
                    meshVane.AddTriangle(i0, i3, i2);
                }

                Voxels voxSingleVane = new Voxels(meshVane);
                voxSingleVane.DoOffset(fVaneThicknessMm * 0.5f);
                voxVanesGroup.Add(voxSingleVane);
            }

            return voxVanesGroup;
        }

        private static Mesh CreateCylinderMesh(float fRadius, float fHeight, float fStartZ = 0.0f, int nSteps = 32)
        {
            Mesh mesh = new Mesh();
            float fDTheta = 2.0f * (float)Math.PI / nSteps;

            int vCenterBot = mesh.nAddVertex(new Vector3(fStartZ, 0, 0));
            int vCenterTop = mesh.nAddVertex(new Vector3(fStartZ + fHeight, 0, 0));

            int[] botIndices = new int[nSteps];
            int[] topIndices = new int[nSteps];

            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * fDTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                botIndices[s] = mesh.nAddVertex(new Vector3(fStartZ, fRadius * cosT, fRadius * sinT));
                topIndices[s] = mesh.nAddVertex(new Vector3(fStartZ + fHeight, fRadius * cosT, fRadius * sinT));
            }

            for (int s = 0; s < nSteps; s++)
            {
                int sNext = (s + 1) % nSteps;
                mesh.AddTriangle(botIndices[s], topIndices[s], topIndices[sNext]);
                mesh.AddTriangle(botIndices[s], topIndices[sNext], botIndices[sNext]);
                mesh.AddTriangle(vCenterBot, botIndices[sNext], botIndices[s]);
                mesh.AddTriangle(vCenterTop, topIndices[s], topIndices[sNext]);
            }

            return mesh;
        }
    }
}