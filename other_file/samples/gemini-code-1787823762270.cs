using System;
using System.Numerics;
using PicoGK;
using LEAP71.ShapeKernel;
using LEAP71.LatticeLibrary;

namespace RocketEngineDesign
{
    public static partial class EngineTasks
    {
        #region Task 6 (Avanzado): Canales de Refrigeración Regenerativa mediante HelixHeatX
        /// <summary>
        /// Genera el volumen del fluido y la estructura de refrigeración helicoidal integrada con aletas térmicas 
        /// siguiendo la trayectoria convergente-divergente de la liner de la tobera.
        /// Utiliza los principios de LEAP71_HelixHeatX y PicoGK/ShapeKernel.
        /// </summary>
        /// <param name="fChamberRadiusMm">Radio interno de la cámara de combustión (mm)</param>
        /// <param name="fThroatRadiusMm">Radio interno de la garganta (mm)</param>
        /// <param name="fExitRadiusMm">Radio interno de la salida de la tobera (mm)</param>
        /// <param name="fTotalLengthMm">Longitud axial total Z (mm)</param>

        public static Voxels Task_GenerateAdvancedHelixHeatXChannels(
            float fChamberRadiusMm = 70.0f,
            float fThroatRadiusMm  = 32.0f,
            float fExitRadiusMm    = 110.0f,
            float fTotalLengthMm   = 220.0f,
            int nHelixTurns        = 4,
            int nFluidChannels     = 60,
            float fChannelDepthMm  = 3.5f,
            float fFinThicknessMm  = 0.6f)
        {
            // 1. Inicializar el campo de vóxeles para alojar todos los pasajes de fluido helicoidales
            Voxels voxCoolantVolume = new Voxels();

            // Paso angular entre canales
            float fDeltaPhi = (2.0f * (float)Math.PI) / nFluidChannels;
            float fTotalRotationAngle = nHelixTurns * 2.0f * (float)Math.PI;

            // 2. Generar el patrón de void helicoidal con aletas térmicas internas
            for (int i = 0; i < nFluidChannels; i++)
            {
                float fStartAngle = i * fDeltaPhi;

                // Crear la malla de la trayectoria helicoidal continua siguiendo el perfil del motor
                Mesh meshHelixChannel = CreateHelixChannelWithFinsMesh(
                    fChamberRadiusMm,
                    fThroatRadiusMm,
                    fExitRadiusMm,
                    fTotalLengthMm,
                    fStartAngle,
                    fTotalRotationAngle,
                    fChannelDepthMm,
                    fFinThicknessMm,
                    nStepsZ: 100
                );

                // Convertir la malla helicoidal a Vóxeles e integrarla al conjunto de fluido
                Voxels voxSingleChannel = new Voxels(meshHelixChannel);
                voxCoolantVolume.Add(voxSingleChannel);
            }

            // 3. Opcional: Aplicar optimización morfológica de barrido o entramado con LatticeLibrary 
            // para mejorar la transferencia de calor convectiva sin bloquear el flujo.
            return voxCoolantVolume;
        }

        #region HelixHeatX Auxiliary Builders
        /// <summary>
        /// Construye un canal helicoidal individual adaptado al radio variable Z, 
        /// incorporando micro-aletas (fins) transversales siguiendo el principio de HelixHeatX.
        /// </summary>
        private static Mesh CreateHelixChannelWithFinsMesh(
            float fRChamber,
            float fRThroat,
            float fRExit,
            float fLengthZ,
            float fStartAngle,
            float fTotalRotation,
            float fDepth,
            float fFinThickness,
            int nStepsZ)
        {
            Mesh mesh = new Mesh();
            float dZ = fLengthZ / nStepsZ;

            int[] prevVertices = null;

            for (int step = 0; step <= nStepsZ; step++)
            {
                float z = step * dZ;
                float t = z / fLengthZ; // Parámetro normalized (0 a 1)

                // Perfil Bézier cuadrático del radio local interno del liner
                float rInner = (1 - t) * (1 - t) * fRChamber + 2 * (1 - t) * t * fRThroat + t * t * fRExit;
                
                // Aplicar modulaciones de aletas térmicas internas (HelixHeatX fins)
                // Se genera un patrón armónico para las aletas a lo largo del eje helicoidal
                float fFinHeightModulation = 0.5f * fDepth * (float)Math.Sin(step * 0.4f);
                float rOuter = rInner + fDepth + (fFinHeightModulation > 0 ? fFinHeightModulation : 0.0f);

                // Ángulo de torsión angular acumulado según Z
                float currentAngle = fStartAngle + (t * fTotalRotation);

                // Ancho angular dependiente de la pared y espacio entre aletas
                float fWidthMm = 2.0f; 
                float halfAngle = (fWidthMm * 0.5f) / rInner;

                float aLeft  = currentAngle - halfAngle;
                float aRight = currentAngle + halfAngle;

                // Generación de los puntos de la sección transversal helicoidal
                Vector3 pInnerLeft  = new Vector3(rInner * (float)Math.Cos(aLeft),  rInner * (float)Math.Sin(aLeft),  -z);
                Vector3 pInnerRight = new Vector3(rInner * (float)Math.Cos(aRight), rInner * (float)Math.Sin(aRight), -z);
                Vector3 pOuterRight = new Vector3(rOuter * (float)Math.Cos(aRight), rOuter * (float)Math.Sin(aRight), -z);
                Vector3 pOuterLeft  = new Vector3(rOuter * (float)Math.Cos(aLeft),  rOuter * (float)Math.Sin(aLeft),  -z);

                int v0 = mesh.nAddVertex(pInnerLeft);
                int v1 = mesh.nAddVertex(pInnerRight);
                int v2 = mesh.nAddVertex(pOuterRight);
                int v3 = mesh.nAddVertex(pOuterLeft);

                int[] currentVertices = new int[] { v0, v1, v2, v3 };

                // Unir segmentos transversales consecutivamente
                if (prevVertices != null)
                {
                    for (int side = 0; side < 4; side++)
                    {
                        int nextSide = (side + 1) % 4;

                        int p0 = prevVertices[side];
                        int p1 = prevVertices[nextSide];
                        int c0 = currentVertices[side];
                        int c1 = currentVertices[nextSide];

                        mesh.AddTriangle(p0, c0, c1);
                        mesh.AddTriangle(p0, c1, p1);
                    }
                }

                prevVertices = currentVertices;
            }

            return mesh;
        }
        #endregion
        #endregion
    }
}