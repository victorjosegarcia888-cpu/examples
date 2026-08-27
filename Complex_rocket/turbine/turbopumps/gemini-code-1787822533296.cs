using System;
using System.Numerics;
using PikoGK;

namespace RocketEngineDesign
{
    public static class PreburnerGenerator
    {
        /// <summary>
        /// Genera el volumen 3D (Voxels) del prequemador y manifold de combustible (Figura 88).
        /// </summary>
        public static Voxels voxCreateASEPreburner(
            float fHousingRadiusMm = 85.0f,
            float fWallThicknessMm = 5.0f,
            float fChamberLengthMm = 90.0f,
            float fDuctLengthMm = 120.0f,
            float fDuctExitRadiusMm = 45.0f,
            float fManifoldMajorRadiusMm = 95.0f,
            float fManifoldMinorRadiusMm = 22.0f)
        {
            // 1. Geometría exterior de la cámara y conducto convergente (Haynes 188)
            Mesh meshOuterBody = CreatePreburnerBodyMesh(
                fHousingRadiusMm, 
                fChamberLengthMm, 
                fDuctLengthMm, 
                fDuctExitRadiusMm
            );
            Voxels voxOuter = new Voxels(meshOuterBody);

            // 2. Cavidad interna del flujo de gas caliente (para vaciado de pared)
            Mesh meshInnerBody = CreatePreburnerBodyMesh(
                fHousingRadiusMm - fWallThicknessMm, 
                fChamberLengthMm, 
                fDuctLengthMm, 
                fDuctExitRadiusMm - fWallThicknessMm
            );
            Voxels voxInner = new Voxels(meshInnerBody);

            // Vaciado del cuerpo principal
            voxOuter.Subtract(voxInner);

            // 3. Manifold Toroidal de Combustible (Fuel Manifold)
            Mesh meshManifold = CreateTorusMesh(
                fManifoldMajorRadiusMm, 
                fManifoldMinorRadiusMm, 
                fCenterZ: 15.0f
            );
            Voxels voxManifold = new Voxels(meshManifold);

            // 4. Placa de soporte e inyectores (A286 Support Plate / Faceplate)
            Mesh meshFaceplate = CreateCylinderMesh(
                fHousingRadiusMm - fWallThicknessMm, 
                fHeightMm: 12.0f, 
                fCenterZ: 0.0f
            );
            Voxels voxFaceplate = new Voxels(meshFaceplate);

            // 5. Integración booleana CSG en PikoGK
            voxOuter.Add(voxManifold);
            voxOuter.Add(voxFaceplate);

            return voxOuter;
        }

        private static Mesh CreatePreburnerBodyMesh(
            float fR1, float fLChamber, float fLDuct, float fR2, int nSteps = 64)
        {
            Mesh mesh = new Mesh();
            float fDTheta = 2.0f * (float)Math.PI / nSteps;

            // Perfil axial (Z, R): Tramo cilíndrico inicial + Tramo convergente
            Vector2[] profile = new Vector2[]
            {
                new Vector2(0.0f, fR1),
                new Vector2(fLChamber, fR1),
                new Vector2(fLChamber + fLDuct, fR2)
            };

            int[,] indices = new int[nSteps, profile.Length];
            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * fDTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                for (int p = 0; p < profile.Length; p++)
                {
                    Vector3 v = new Vector3(profile[p].X, profile[p].Y * cosT, profile[p].Y * sinT);
                    indices[s, p] = mesh.nAddVertex(v);
                }
            }

            for (int s = 0; s < nSteps; s++)
            {
                int sNext = (s + 1) % nSteps;
                for (int p = 0; p < profile.Length - 1; p++)
                {
                    int pNext = p + 1;
                    mesh.AddTriangle(indices[s, p], indices[sNext, p], indices[sNext, pNext]);
                    mesh.AddTriangle(indices[s, p], indices[sNext, pNext], indices[s, pNext]);
                }
            }

            return mesh;
        }

        private static Mesh CreateTorusMesh(
            float fRMajor, float fRMinor, float fCenterZ, int nRadialSteps = 48, int nTubularSteps = 24)
        {
            Mesh mesh = new Mesh();
            float fDPhi = 2.0f * (float)Math.PI / nRadialSteps;
            float fDTheta = 2.0f * (float)Math.PI / nTubularSteps;

            int[,] indices = new int[nRadialSteps, nTubularSteps];

            for (int r = 0; r < nRadialSteps; r++)
            {
                float phi = r * fDPhi;
                float cosPhi = (float)Math.Cos(phi);
                float sinPhi = (float)Math.Sin(phi);

                for (int t = 0; t < nTubularSteps; t++)
                {
                    float theta = t * fDTheta;
                    float rLocal = fRMajor + fRMinor * (float)Math.Cos(theta);

                    Vector3 v = new Vector3(
                        fCenterZ + fRMinor * (float)Math.Sin(theta),
                        rLocal * cosPhi,
                        rLocal * sinPhi
                    );

                    indices[r, t] = mesh.nAddVertex(v);
                }
            }

            for (int r = 0; r < nRadialSteps; r++)
            {
                int rNext = (r + 1) % nRadialSteps;
                for (int t = 0; t < nTubularSteps; t++)
                {
                    int tNext = (t + 1) % nTubularSteps;
                    mesh.AddTriangle(indices[r, t], indices[rNext, t], indices[rNext, tNext]);
                    mesh.AddTriangle(indices[r, t], indices[rNext, tNext], indices[r, tNext]);
                }
            }

            return mesh;
        }

        private static Mesh CreateCylinderMesh(float fRadius, float fHeightMm, float fCenterZ, int nSteps = 48)
        {
            Mesh mesh = new Mesh();
            float fDTheta = 2.0f * (float)Math.PI / nSteps;

            int[] topIndices = new int[nSteps];
            int[] botIndices = new int[nSteps];

            int vCenterBot = mesh.nAddVertex(new Vector3(fCenterZ, 0, 0));
            int vCenterTop = mesh.nAddVertex(new Vector3(fCenterZ + fHeightMm, 0, 0));

            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * fDTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                botIndices[s] = mesh.nAddVertex(new Vector3(fCenterZ, fRadius * cosT, fRadius * sinT));
                topIndices[s] = mesh.nAddVertex(new Vector3(fCenterZ + fHeightMm, fRadius * cosT, fRadius * sinT));
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