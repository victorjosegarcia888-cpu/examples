using System;
using System.Numerics;
using PikoGK;

namespace RocketEngineDesign
{
    public static class CoaxialInjectorGenerator
    {
        /// <summary>
        /// Genera los 13 elementos inyectores coaxiales compuestos por poste central (LOX) 
        /// y manga exterior (Fuel Annulus).
        /// </summary>
        public static Voxels voxCreate13CoaxialInjectors(
            float fInnerPostInnerRadiusMm = 2.5f,
            float fInnerPostOuterRadiusMm = 3.5f,
            float fOuterSleeveOuterRadiusMm = 6.4f, // Ligera interferencia/ajuste con los 6.5mm del orificio
            float fInjectorLengthMm = 35.0f,
            float fRingRadiusMm = 48.0f,
            float fCenterZ = -5.0f)
        {
            Voxels voxAllInjectors = new Voxels();

            // Posición 1: Inyector Central (0, 0)
            voxAllInjectors.Add(voxCreateSingleCoaxialElement(
                0.0f, 0.0f, fCenterZ, 
                fInnerPostInnerRadiusMm, fInnerPostOuterRadiusMm, 
                fOuterSleeveOuterRadiusMm, fInjectorLengthMm));

            // Posiciones 2 a 13: Anillo de 12 inyectores periféricos
            int nOuterInjectors = 12;
            float fAngleStep = 2.0f * (float)Math.PI / nOuterInjectors;

            for (int i = 0; i < nOuterInjectors; i++)
            {
                float fAngle = i * fAngleStep;
                float fX = fRingRadiusMm * (float)Math.Cos(fAngle);
                float fY = fRingRadiusMm * (float)Math.Sin(fAngle);

                Voxels voxSingle = voxCreateSingleCoaxialElement(
                    fX, fY, fCenterZ, 
                    fInnerPostInnerRadiusMm, fInnerPostOuterRadiusMm, 
                    fOuterSleeveOuterRadiusMm, fInjectorLengthMm);

                voxAllInjectors.Add(voxSingle);
            }

            return voxAllInjectors;
        }

        private static Voxels voxCreateSingleCoaxialElement(
            float fX, float fY, float fCenterZ,
            float fInnerR, float fOuterR, float fSleeveR, float fLength)
        {
            // 1. Cuerpo exterior de la manga (Sleeve)
            Mesh meshOuterSleeve = CreateCylinderMeshTranslated(fSleeveR, fLength, fCenterZ, fX, fY);
            Voxels voxElement = new Voxels(meshOuterSleeve);

            // 2. Anillo de paso de combustible (espacio entre poste interno y manga exterior)
            Mesh meshFuelGap = CreateCylinderMeshTranslated(fOuterR, fLength + 2.0f, fCenterZ - 1.0f, fX, fY);
            Voxels voxFuelGap = new Voxels(meshFuelGap);

            // 3. Pared del poste interno de oxidante (LOX Post)
            Mesh meshInnerPostWall = CreateCylinderMeshTranslated(fOuterR, fLength, fCenterZ, fX, fY);
            Voxels voxInnerPost = new Voxels(meshInnerPostWall);

            // 4. Conducto central de oxidante (LOX Inner Passage)
            Mesh meshLoxPassage = CreateCylinderMeshTranslated(fInnerR, fLength + 4.0f, fCenterZ - 2.0f, fX, fY);
            Voxels voxLoxPassage = new Voxels(meshLoxPassage);

            // Perforación interna del poste de oxidante
            voxInnerPost.Subtract(voxLoxPassage);

            // Vaciado del espacio anular en la manga e inserción del poste interno
            voxElement.Subtract(voxFuelGap);
            voxElement.Add(voxInnerPost);

            return voxElement;
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