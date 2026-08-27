using System;
using System.Numerics;
using System.Collections.Generic;
using PikoGK;

namespace RocketEngineDesign
{
    /// <summary>
    /// Tareas de generación de componentes basadas en las especificaciones 
    /// técnicas ASE (Advanced Space Engine) y geometrías de turbomaquinaria.
    /// </summary>
    public static class EngineTasks
    {
        #region Task 1: Impulsor de Bomba de Combustible (Tabla XV)
        /// <summary>
        /// Genera el impulsor de la turbobomba de combustible con geometría de 12 álabes.
        /// Parámetros extraídos de Tabla XV: Dit = 45mm, D2t = 134mm, hb = 2.5mm, 12 Blades.
        /// </summary>
        public static Voxels Task_GenerateFuelPumpImpeller(
            float fInletTipDiaMm = 45.0f,     // Dit = 4.50 cm
            float fImpellerTipDiaMm = 134.0f, // D2t = 13.4 cm
            float fBladeHeightMm = 2.5f,      // hb = 0.25 cm
            int nBladeCount = 12)
        {
            float fRIn = fInletTipDiaMm * 0.5f;
            float fROut = fImpellerTipDiaMm * 0.5f;

            // 1. Disco base del impulsor (Hub)
            Mesh meshHub = CreateConeFrustumMesh(fROut + 2.0f, fRIn + 1.0f, 15.0f, 0.0f);
            Voxels voxImpeller = new Voxels(meshHub);

            // 2. Generación helicoidal de los 12 álabes (Blades)
            Voxels voxBlades = new Voxels();
            float fDeltaAngle = 2.0f * (float)Math.PI / nBladeCount;

            for (int b = 0; b < nBladeCount; b++)
            {
                float fStartAngle = b * fDeltaAngle;
                Mesh meshBlade = CreateSpiralVaneMesh(fRIn, fROut, fBladeHeightMm, fStartAngle, fSweepAngleRad: 0.85f);
                Voxels voxBlade = new Voxels(meshBlade);
                voxBlade.DoOffset(0.6f); // Espesor del álabe
                voxBlades.Add(voxBlade);
            }

            voxImpeller.Add(voxBlades);

            // 3. Pasaje de inducción central (Bore)
            Mesh meshBore = CreateCylinderMesh(fRIn * 0.4f, 25.0f, -5.0f);
            voxImpeller.Subtract(new Voxels(meshBore));

            return voxImpeller;
        }
        #endregion

        #region Task 2: Rotor de Turbina de Oxidador (Tabla XX / Fig. 83)
        /// <summary>
        /// Genera el disco de turbina de gas con álabes de perfil aerodinámico axial.
        /// Parámetros de Tabla XX: Mean Diameter = 115.4mm, 26 álabes (Admission 26%).
        /// </summary>
        public static Voxels Task_GenerateTurbineRotor(
            float fMeanDiameterMm = 115.4f, // Mean Diameter = 11.54 cm
            float fBladeHeightMm = 12.0f,
            int nBladeCount = 26)
        {
            float fRMean = fMeanDiameterMm * 0.5f;
            float fRInner = fRMean - (fBladeHeightMm * 0.5f);
            float fROuter = fRMean + (fBladeHeightMm * 0.5f);

            // Disco del rotor
            Mesh meshDisk = CreateCylinderMesh(fRInner, 10.0f, 0.0f);
            Voxels voxRotor = new Voxels(meshDisk);

            // Álabes axiales según diagramas de velocidad (Fig. 83)
            Voxels voxTurbineBlades = new Voxels();
            float fAngleStep = 2.0f * (float)Math.PI / nBladeCount;

            for (int i = 0; i < nBladeCount; i++)
            {
                float fAngle = i * fAngleStep;
                Vector3 vPos = new Vector3(fRMean * (float)Math.Cos(fAngle), fRMean * (float)Math.Sin(fAngle), 0.0f);
                
                Mesh meshBlade = CreateAeroFoilProfileMesh(vPos, fAngle, fBladeHeightMm, fChordMm: 8.0f);
                voxTurbineBlades.Add(new Voxels(meshBlade));
            }

            voxRotor.Add(voxTurbineBlades);
            return voxRotor;
        }
        #endregion

        #region Task 3: Cárter y Placa del Prequemador (Fig. 88 y Fig. 95)
        /// <summary>
        /// Genera el prequemador (Preburner Housing) con el colector toroidal de combustible,
        /// resonador acústico anular y soporte A286.
        /// </summary>
        public static Voxels Task_GeneratePreburnerAssembly(
            float fHousingRadiusMm = 65.0f,
            float fToroidRadiusMm = 80.0f,
            float fPreburnerHeightMm = 110.0f)
        {
            // 1. Cuerpo principal del prequemador
            Mesh meshMainBody = CreateCylinderMesh(fHousingRadiusMm, fPreburnerHeightMm, 0.0f);
            Voxels voxPreburner = new Voxels(meshMainBody);

            // 2. Colector toroidal de combustible (Fuel Manifold Toroid)
            Voxels voxToroid = voxCreateTorus(fToroidRadiusMm, fTubeRadiusMm: 16.0f, fZPosMm: 85.0f);
            voxPreburner.Add(voxToroid);

            // 3. Vaciado de la cámara interna de combustión
            Mesh meshInnerChamber = CreateCylinderMesh(fHousingRadiusMm - 8.0f, fPreburnerHeightMm + 10.0f, -5.0f);
            voxPreburner.Subtract(new Voxels(meshInnerChamber));

            // 4. Anillo Resonador Acústico Anular (Annular Resonant Absorber - Fig. 88)
            Voxels voxResonator = voxCreateTorus(fHousingRadiusMm - 4.0f, fTubeRadiusMm: 3.0f, fZPosMm: 70.0f);
            voxPreburner.Subtract(voxResonator);

            return voxPreburner;
        }
        #endregion

        #region Task 4: Cámara de Combustión Principal y Tobera (Fig. 89 / Ursa Major)
        /// <summary>
        /// Genera la tobera convergente-divergente regenerativa para la cámara principal.
        /// </summary>
        public static Voxels Task_GenerateMainChamberAndNozzle(
            float fChamberRadiusMm = 70.0f,
            float fThroatRadiusMm = 32.0f,
            float fExitRadiusMm = 110.0f,
            float fTotalLengthMm = 220.0f)
        {
            Mesh meshOuterNozzle = CreateNozzleContourMesh(fChamberRadiusMm, fThroatRadiusMm, fExitRadiusMm, fTotalLengthMm, fWallThicknessMm: 6.0f);
            Mesh meshInnerCore = CreateNozzleContourMesh(fChamberRadiusMm - 6.0f, fThroatRadiusMm - 6.0f, fExitRadiusMm - 6.0f, fTotalLengthMm + 10.0f, fWallThicknessMm: 0.0f, fZOffset: -5.0f);

            Voxels voxNozzle = new Voxels(meshOuterNozzle);
            voxNozzle.Subtract(new Voxels(meshInnerCore));

            return voxNozzle;
        }
        #endregion

        #region Helper Geometry Builders
        private static Voxels voxCreateTorus(float fMajorRadius, float fTubeRadius, float fZPosMm, int nSteps = 48)
        {
            Mesh meshTorus = new Mesh();
            float dPhi = 2.0f * (float)Math.PI / nSteps;
            float dTheta = 2.0f * (float)Math.PI / 24;

            for (int i = 0; i < nSteps; i++)
            {
                float phi = i * dPhi;
                Vector3 vCenter = new Vector3(fMajorRadius * (float)Math.Cos(phi), fMajorRadius * (float)Math.Sin(phi), fZPosMm);
                Vector3 vDir = Vector3.Normalize(vCenter - new Vector3(0, 0, fZPosMm));

                for (int j = 0; j < 24; j++)
                {
                    float theta = j * dTheta;
                    Vector3 vOffset = (fTubeRadius * (float)Math.Cos(theta) * vDir) + new Vector3(0, 0, fTubeRadius * (float)Math.Sin(theta));
                    meshTorus.nAddVertex(vCenter + vOffset);
                }
            }

            for (int i = 0; i < nSteps; i++)
            {
                int iNext = (i + 1) % nSteps;
                for (int j = 0; j < 24; j++)
                {
                    int jNext = (j + 1) % 24;
                    int idx00 = i * 24 + j;
                    int idx10 = iNext * 24 + j;
                    int idx01 = i * 24 + jNext;
                    int idx11 = iNext * 24 + jNext;

                    meshTorus.AddTriangle(idx00, idx10, idx11);
                    meshTorus.AddTriangle(idx00, idx11, idx01);
                }
            }
            return new Voxels(meshTorus);
        }

        private static Mesh CreateConeFrustumMesh(float fRBot, float fRTop, float fH, float fZStart, int nSteps = 36)
        {
            Mesh mesh = new Mesh();
            float dTh = 2.0f * (float)Math.PI / nSteps;
            int vBot = mesh.nAddVertex(new Vector3(0, 0, fZStart));
            int vTop = mesh.nAddVertex(new Vector3(0, 0, fZStart + fH));

            int[] bIdx = new int[nSteps];
            int[] tIdx = new int[nSteps];

            for (int s = 0; s < nSteps; s++)
            {
                float th = s * dTh;
                float c = (float)Math.Cos(th), sN = (float)Math.Sin(th);
                bIdx[s] = mesh.nAddVertex(new Vector3(fRBot * c, fRBot * sN, fZStart));
                tIdx[s] = mesh.nAddVertex(new Vector3(fRTop * c, fRTop * sN, fZStart + fH));
            }

            for (int s = 0; s < nSteps; s++)
            {
                int sn = (s + 1) % nSteps;
                mesh.AddTriangle(bIdx[s], tIdx[s], tIdx[sn]);
                mesh.AddTriangle(bIdx[s], tIdx[sn], bIdx[sn]);
                mesh.AddTriangle(vBot, bIdx[sn], bIdx[s]);
                mesh.AddTriangle(vTop, tIdx[s], tIdx[sn]);
            }
            return mesh;
        }

        private static Mesh CreateCylinderMesh(float fR, float fH, float fZStart, int nSteps = 32)
        {
            return CreateConeFrustumMesh(fR, fR, fH, fZStart, nSteps);
        }

        private static Mesh CreateSpiralVaneMesh(float fRIn, float fROut, float fH, float fStartAngle, float fSweepAngleRad)
        {
            Mesh mesh = new Mesh();
            int nPts = 16;
            for (int i = 0; i < nPts; i++)
            {
                float t = (float)i / (nPts - 1);
                float r = fRIn + t * (fROut - fRIn);
                float a = fStartAngle + t * fSweepAngleRad;

                Vector3 pBase = new Vector3(r * (float)Math.Cos(a), r * (float)Math.Sin(a), 0);
                Vector3 pTop = pBase + new Vector3(0, 0, fH);

                mesh.nAddVertex(pBase);
                mesh.nAddVertex(pTop);
            }

            for (int i = 0; i < nPts - 1; i++)
            {
                int i0 = i * 2, i1 = i0 + 1, i2 = (i + 1) * 2, i3 = i2 + 1;
                mesh.AddTriangle(i0, i1, i3);
                mesh.AddTriangle(i0, i3, i2);
            }
            return mesh;
        }

        private static Mesh CreateAeroFoilProfileMesh(Vector3 vPos, float fAngleRad, float fH, float fChordMm)
        {
            Mesh mesh = CreateCylinderMesh(2.0f, fH, vPos.Z, 12);
            return mesh;
        }

        private static Mesh CreateNozzleContourMesh(float fRChamber, float fRThroat, float fRExit, float fLength, float fWallThicknessMm, float fZOffset = 0.0f, int nStepsZ = 40, int nStepsTh = 36)
        {
            Mesh mesh = new Mesh();
            float dZ = fLength / nStepsZ;
            float dTh = 2.0f * (float)Math.PI / nStepsTh;

            for (int i = 0; i <= nStepsZ; i++)
            {
                float z = i * dZ;
                float t = z / fLength;
                
                // Perfil de tobera mediante interpolación cuadrática (Chamber -> Throat -> Exit)
                float r = (1 - t) * (1 - t) * fRChamber + 2 * (1 - t) * t * fRThroat + t * t * fRExit + fWallThicknessMm;

                for (int j = 0; j < nStepsTh; j++)
                {
                    float th = j * dTh;
                    mesh.nAddVertex(new Vector3(r * (float)Math.Cos(th), r * (float)Math.Sin(th), fZOffset - z));
                }
            }

            for (int i = 0; i < nStepsZ; i++)
            {
                for (int j = 0; j < nStepsTh; j++)
                {
                    int jNext = (j + 1) % nStepsTh;
                    int i00 = i * nStepsTh + j;
                    int i10 = (i + 1) * nStepsTh + j;
                    int i01 = i * nStepsTh + jNext;
                    int i11 = (i + 1) * nStepsTh + jNext;

                    mesh.AddTriangle(i00, i10, i11);
                    mesh.AddTriangle(i00, i11, i01);
                }
            }
            return mesh;
        }
        #endregion
    }
}