using System;
using System.Numerics;
using System.Collections.Generic;
using PikoGK;

namespace RocketEngineDesign
{
    public static class FullInjectorAssemblyGenerator
    {
        /// <summary>
        /// Genera el ensamble monolítico completo del inyector de cohete,
        /// incluyendo cuerpo principal, galerías internas de distribución
        /// y 13 elementos inyectores coaxiales orientados radialmente.
        /// </summary>
        public static Voxels voxCreateFullInjectorAssembly(
            float fHousingOuterRadiusMm = 75.0f,
            float fHousingHeightMm = 85.0f,
            float fInnerRingRadiusMm = 28.0f,
            float fOuterRingRadiusMm = 52.0f)
        {
            // 1. Generar el cuerpo base del inyector con sus galerías de distribución
            Voxels voxHousing = InjectorManifoldsGenerator.voxCreateInjectorHousingWithGalleries(
                fHousingOuterRadiusMm: fHousingOuterRadiusMm,
                fHousingHeightMm: fHousingHeightMm,
                fGch4GalleryRadiusMm: fOuterRingRadiusMm,
                fGoxGalleryRadiusMm: fInnerRingRadiusMm
            );

            // 2. Definir las posiciones de la matriz de 13 inyectores (1 Centro, 6 Anillo Interno, 6 Anillo Externo)
            List<Vector3> listInjectorPositions = GetInjectorPatternPositions(
                fInnerRingRadiusMm, 
                fOuterRingRadiusMm
            );

            Voxels voxInjectorsGroup = new Voxels();
            Voxels voxInjectorDrillPassages = new Voxels();

            // 3. Generar e integrar cada elemento inyector coaxial en sus coordenadas
            foreach (Vector3 vPos in listInjectorPositions)
            {
                // Elemento inyector coaxial con swirlers desplazado a la posición (vPos.X, vPos.Y)
                Voxels voxSingleInjector = voxCreatePositionedInjectorElement(
                    vOffset: vPos,
                    fCoreTubeInnerRadiusMm: 2.0f,
                    fCoreTubeOuterRadiusMm: 3.2f,
                    fSwirlerOuterRadiusMm: 6.5f,
                    fElementLengthMm: fHousingHeightMm + 10.0f
                );
                voxInjectorsGroup.Add(voxSingleInjector);

                // Perforaciones de alimentacion vertical desde galerias a la camara
                Mesh meshPassage = CreateCylinderMeshAtPosition(
                    vCenter: vPos,
                    fRadius: 6.6f,
                    fHeight: fHousingHeightMm + 14.0f,
                    fZStart: -7.0f
                );
                voxInjectorDrillPassages.Add(new Voxels(meshPassage));
            }

            // 4. Operación Booleana: Taladrar los alojamientos de los inyectores en el cuerpo principal
            voxHousing.Subtract(voxInjectorDrillPassages);

            // 5. Operación Booleana: Fusionar la matriz de inyectores coaxiales en el bloque
            voxHousing.Add(voxInjectorsGroup);

            return voxHousing;
        }

        /// <summary>
        /// Calculador del patrón espacial de 13 inyectores (1 central, 6 en radio interno, 6 en radio externo).
        /// </summary>
        private static List<Vector3> GetInjectorPatternPositions(float fR1, float fR2)
        {
            List<Vector3> positions = new List<Vector3>();

            // Inyector central (0, 0)
            positions.Add(new Vector3(0.0f, 0.0f, 0.0f));

            // Anillo interior: 6 inyectores separados por 60 grados
            for (int i = 0; i < 6; i++)
            {
                float fAngle = i * (float)(Math.PI / 3.0);
                positions.Add(new Vector3(
                    fR1 * (float)Math.Cos(fAngle),
                    fR1 * (float)Math.Sin(fAngle),
                    0.0f
                ));
            }

            // Anillo exterior: 6 inyectores desfasados 30 grados respecto al interior
            for (int i = 0; i < 6; i++)
            {
                float fAngle = (i * (float)(Math.PI / 3.0)) + (float)(Math.PI / 6.0);
                positions.Add(new Vector3(
                    fR2 * (float)Math.Cos(fAngle),
                    fR2 * (float)Math.Sin(fAngle),
                    0.0f
                ));
            }

            return positions;
        }

        /// <summary>
        /// Genera la geometría de un inyector coaxial individual desplazado espacialmente.
        /// </summary>
        private static Voxels voxCreatePositionedInjectorElement(
            Vector3 vOffset,
            float fCoreTubeInnerRadiusMm,
            float fCoreTubeOuterRadiusMm,
            float fSwirlerOuterRadiusMm,
            float fElementLengthMm)
        {
            // Malla del tubo central desplazada
            Mesh meshCoreOuter = CreateCylinderMeshAtPosition(vOffset, fCoreTubeOuterRadiusMm, fElementLengthMm, -5.0f);
            Mesh meshCoreInner = CreateCylinderMeshAtPosition(vOffset, fCoreTubeInnerRadiusMm, fElementLengthMm + 10.0f, -7.0f);
            
            Voxels voxCore = new Voxels(meshCoreOuter);
            voxCore.Subtract(new Voxels(meshCoreInner));

            // Malla de la manga anular externa desplazada
            Mesh meshSleeveOuter = CreateCylinderMeshAtPosition(vOffset, fSwirlerOuterRadiusMm, fElementLengthMm, -5.0f);
            Mesh meshSleeveInner = CreateCylinderMeshAtPosition(vOffset, fCoreTubeOuterRadiusMm, fElementLengthMm, -5.0f);
            
            Voxels voxSwirlChannel = new Voxels(meshSleeveOuter);
            voxSwirlChannel.Subtract(new Voxels(meshSleeveInner));

            // Combinación final del elemento
            Voxels voxElement = new Voxels();
            voxElement.Add(voxCore);
            voxElement.Add(voxSwirlChannel);

            return voxElement;
        }

        private static Mesh CreateCylinderMeshAtPosition(
            Vector3 vCenter, float fRadius, float fHeight, float fZStart = 0.0f, int nSteps = 32)
        {
            Mesh mesh = new Mesh();
            float fDTheta = 2.0f * (float)Math.PI / nSteps;

            int vCenterBot = mesh.nAddVertex(new Vector3(vCenter.X, vCenter.Y, fZStart));
            int vCenterTop = mesh.nAddVertex(new Vector3(vCenter.X, vCenter.Y, fZStart + fHeight));

            int[] botIndices = new int[nSteps];
            int[] topIndices = new int[nSteps];

            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * fDTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                float x = vCenter.X + fRadius * cosT;
                float y = vCenter.Y + fRadius * sinT;

                botIndices[s] = mesh.nAddVertex(new Vector3(x, y, fZStart));
                topIndices[s] = mesh.nAddVertex(new Vector3(x, y, fZStart + fHeight));
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