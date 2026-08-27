using System;
using System.Numerics;
using PikoGK;

namespace RocketEngineDesign
{
    public static class HotGasDuctGenerator
    {
        /// <summary>
        /// Genera el conducto curvado de gases calientes (Hot Gas Duct) con cavidad interior 
        /// y bridas de conexión en los extremos.
        /// </summary>
        /// <param name="fBendRadiusMm">Radio de curvatura del eje central del tubo.</param>
        /// <param name="fOuterRadiusMm">Radio exterior del tubo.</param>
        /// <param name="fWallThicknessMm">Espesor de pared del conducto.</param>
        /// <param name="fBendAngleDeg">Ángulo total de la curva en grados (ej. 90°).</param>
        /// <param name="fFlangeRadiusMm">Radio exterior de las bridas de montaje.</param>
        /// <param name="fFlangeThicknessMm">Espesor de las bridas de montaje.</param>
        public static Voxels voxCreateHotGasDuct(
            float fBendRadiusMm = 75.0f,
            float fOuterRadiusMm = 16.0f,
            float fWallThicknessMm = 3.0f,
            float fBendAngleDeg = 90.0f,
            float fFlangeRadiusMm = 24.0f,
            float fFlangeThicknessMm = 8.0f)
        {
            float fInnerRadiusMm = fOuterRadiusMm - fWallThicknessMm;
            float fBendAngleRad = fBendAngleDeg * (float)Math.PI / 180.0f;

            // 1. Malla del cuerpo exterior del tubo curvado
            Mesh meshOuterDuct = CreateSweptPipeMesh(fBendRadiusMm, fOuterRadiusMm, fBendAngleRad);
            Voxels voxDuct = new Voxels(meshOuterDuct);

            // 2. Malla del canal de fluido interior (ligeramente extendida para perforación limpia)
            Mesh meshInnerPassage = CreateSweptPipeMesh(fBendRadiusMm, fInnerRadiusMm, fBendAngleRad, fExtensionMm: 4.0f);
            Voxels voxFluidPassage = new Voxels(meshInnerPassage);

            // 3. Bridas de acople estructural en la entrada y salida
            Voxels voxInletFlange = voxCreateFlange(
                new Vector3(0, 0, 0), 
                new Vector3(1, 0, 0), 
                fFlangeRadiusMm, 
                fFlangeThicknessMm
            );

            Vector3 vOutletPos = GetCurvePoint(fBendRadiusMm, fBendAngleRad);
            Vector3 vOutletDir = GetCurveTangent(fBendAngleRad);

            Voxels voxOutletFlange = voxCreateFlange(
                vOutletPos, 
                vOutletDir, 
                fFlangeRadiusMm, 
                fFlangeThicknessMm
            );

            // 4. Unión de cuerpo y bridas
            voxDuct.Add(voxInletFlange);
            voxDuct.Add(voxOutletFlange);

            // 5. Vaciado del conducto interno (Boolean Subtract)
            voxDuct.Subtract(voxFluidPassage);

            return voxDuct;
        }

        private static Mesh CreateSweptPipeMesh(
            float fBendRadius, float fCrossRadius, float fMaxAngleRad, float fExtensionMm = 0.0f, 
            int nStepsBend = 36, int nStepsCircle = 24)
        {
            Mesh mesh = new Mesh();
            float dAngle = fMaxAngleRad / nStepsBend;
            float dPhi = 2.0f * (float)Math.PI / nStepsCircle;

            for (int i = 0; i <= nStepsBend; i++)
            {
                float fAngle = i * dAngle;
                Vector3 vCenter = GetCurvePoint(fBendRadius, fAngle);
                Vector3 vTangent = GetCurveTangent(fAngle);

                // Si hay extensión, alargamos los extremos para asegurar perforación completa
                if (i == 0 && fExtensionMm > 0.0f)
                    vCenter -= vTangent * fExtensionMm;
                else if (i == nStepsBend && fExtensionMm > 0.0f)
                    vCenter += vTangent * fExtensionMm;

                // Sistema de coordenadas local para el plano transversal
                Vector3 vNormal = new Vector3(-(float)Math.Sin(fAngle), 0, (float)Math.Cos(fAngle));
                Vector3 vBinormal = Vector3.UnitY;

                for (int j = 0; j < nStepsCircle; j++)
                {
                    float phi = j * dPhi;
                    float cosP = (float)Math.Cos(phi);
                    float sinP = (float)Math.Sin(phi);

                    Vector3 vPos = vCenter + fCrossRadius * (cosP * vNormal + sinP * vBinormal);
                    mesh.nAddVertex(vPos);
                }
            }

            // Triangulación de la pared del cilindro curvado
            for (int i = 0; i < nStepsBend; i++)
            {
                for (int j = 0; j < nStepsCircle; j++)
                {
                    int jNext = (j + 1) % nStepsCircle;

                    int idx00 = i * nStepsCircle + j;
                    int idx10 = (i + 1) * nStepsCircle + j;
                    int idx01 = i * nStepsCircle + jNext;
                    int idx11 = (i + 1) * nStepsCircle + jNext;

                    mesh.AddTriangle(idx00, idx10, idx11);
                    mesh.AddTriangle(idx00, idx11, idx01);
                }
            }

            return mesh;
        }

        private static Voxels voxCreateFlange(Vector3 vCenter, Vector3 vNormal, float fRadius, float fThickness)
        {
            Mesh meshFlange = new Mesh();
            int nSteps = 32;
            float dTheta = 2.0f * (float)Math.PI / nSteps;

            // Construcción del plano local ortogonal a la normal
            Vector3 vUp = Math.Abs(vNormal.Y) > 0.9f ? Vector3.UnitZ : Vector3.UnitY;
            Vector3 vTangent = Vector3.Normalize(Vector3.Cross(vNormal, vUp));
            Vector3 vBinormal = Vector3.Normalize(Vector3.Cross(vTangent, vNormal));

            Vector3 vStart = vCenter - vNormal * (fThickness * 0.5f);
            Vector3 vEnd = vCenter + vNormal * (fThickness * 0.5f);

            int[] botIndices = new int[nSteps];
            int[] topIndices = new int[nSteps];

            int vCenterBot = meshFlange.nAddVertex(vStart);
            int vCenterTop = meshFlange.nAddVertex(vEnd);

            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * dTheta;
                Vector3 vOffset = fRadius * ((float)Math.Cos(theta) * vTangent + (float)Math.Sin(theta) * vBinormal);

                botIndices[s] = meshFlange.nAddVertex(vStart + vOffset);
                topIndices[s] = meshFlange.nAddVertex(vEnd + vOffset);
            }

            for (int s = 0; s < nSteps; s++)
            {
                int sNext = (s + 1) % nSteps;
                meshFlange.AddTriangle(botIndices[s], topIndices[s], topIndices[sNext]);
                meshFlange.AddTriangle(botIndices[s], topIndices[sNext], botIndices[sNext]);
                meshFlange.AddTriangle(vCenterBot, botIndices[sNext], botIndices[s]);
                meshFlange.AddTriangle(vCenterTop, topIndices[s], topIndices[sNext]);
            }

            return new Voxels(meshFlange);
        }

        private static Vector3 GetCurvePoint(float fR, float fAngleRad)
        {
            return new Vector3(
                fR * (float)Math.Sin(fAngleRad), 
                0.0f, 
                fR * (1.0f - (float)Math.Cos(fAngleRad))
            );
        }

        private static Vector3 GetCurveTangent(float fAngleRad)
        {
            return new Vector3(
                (float)Math.Cos(fAngleRad), 
                0.0f, 
                (float)Math.Sin(fAngleRad)
            );
        }
    }
}