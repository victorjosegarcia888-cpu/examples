using System;
using System.Numerics;
using PikoGK;

namespace RocketEngineDesign
{
    public static class InjectorPlateGenerator
    {
        /// <summary>
        /// Genera la placa del inyector (Faceplate) con un patrón de 13 elementos coaxiales
        /// (1 inyector central + 12 inyectores distribuidos circularmente).
        /// </summary>
        public static Voxels voxCreateFaceplateWithInjectors(
            float fPlateRadiusMm = 80.0f,
            float fPlateThicknessMm = 12.0f,
            float fInjectorHoleRadiusMm = 6.5f,
            float fRingRadiusMm = 48.0f,
            float fCenterZ = 0.0f)
        {
            // 1. Placa base maciza
            Mesh meshBasePlate = CreateCylinderMeshTranslated(fPlateRadiusMm, fPlateThicknessMm, fCenterZ, 0, 0);
            Voxels voxPlate = new Voxels(meshBasePlate);

            // 2. Inyector Central (1/13)
            Mesh meshCenterHole = CreateCylinderMeshTranslated(
                fInjectorHoleRadiusMm, 
                fPlateThicknessMm + 4.0f, // Sobredimensionado axial para garantizar perforación limpia
                fCenterZ - 2.0f,
                0.0f, 
                0.0f
            );
            Voxels voxHoles = new Voxels(meshCenterHole);

            // 3. Patrón angular para los 12 inyectores periféricos (12/13)
            int nOuterInjectors = 12;
            float fAngleStep = 2.0f * (float)Math.PI / nOuterInjectors;

            for (int i = 0; i < nOuterInjectors; i++)
            {
                float fAngle = i * fAngleStep;
                float fX = fRingRadiusMm * (float)Math.Cos(fAngle);
                float fY = fRingRadiusMm * (float)Math.Sin(fAngle);

                Mesh meshHole = CreateCylinderMeshTranslated(
                    fInjectorHoleRadiusMm,
                    fPlateThicknessMm + 4.0f,
                    fCenterZ - 2.0f,
                    fX,
                    fY
                );

                // Unificación booleana de las cavidades de inyección
                voxHoles.Add(new Voxels(meshHole));
            }

            // 4. Substracción CSG de los 13 elementos en la placa
            voxPlate.Subtract(voxHoles);

            return voxPlate;
        }

        private static Mesh CreateCylinderMeshTranslated(
            float fRadius, float fHeightMm, float fCenterZ, float fOffsetX, float fOffsetY, int nSteps = 32)
        {
            Mesh mesh = new Mesh();
            float fDTheta = 2.0f * (float)Math.PI / nSteps;

            int[] topIndices = new int[nSteps];
            int[] botIndices = new int[nSteps];

            int vCenterBot = mesh.nAddVertex(new Vector3(fCenterZ, fOffsetX, fOffsetY));
            int vCenterTop = mesh.nAddVertex(new Vector3(fCenterZ + fHeightMm, fOffsetX, fOffsetY));

            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * fDTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                botIndices[s] = mesh.nAddVertex(new Vector3(
                    fCenterZ, 
                    fOffsetX + fRadius * cosT, 
                    fOffsetY + fRadius * sinT
                ));
                topIndices[s] = mesh.nAddVertex(new Vector3(
                    fCenterZ + fHeightMm, 
                    fOffsetX + fRadius * cosT, 
                    fOffsetY + fRadius * sinT
                ));
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