using System;
using System.Collections.Generic;
using System.Numerics;
using PikoGK;

namespace RocketEngineDesign
{
    public static class RegenCoolingGenerator
    {
        /// <summary>
        /// Genera la tobera con canales de refrigeración regenerativa integrados usando operaciones de vóxeles.
        /// </summary>
        /// <param name="nChannelCount">Número total de canales de refrigeración alrededor del perímetro (ej. 120)</param>
        /// <param name="fChannelWidthMm">Ancho del canal en mm</param>
        /// <param name="fChannelHeightMm">Altura/profundidad del canal en mm</param>
        /// <param name="fInnerLinerThickMm">Espesor de la pared interna (entre el gas caliente y el canal)</param>
        /// <param name="fHelixAngleDeg">Ángulo de hélice/torsión de los canales en grados (0 para canales rectos)</param>
        public static Voxels voxCreateRegenNozzle(
            int nChannelCount = 96,
            float fChannelWidthMm = 3.5f,
            float fChannelHeightMm = 5.0f,
            float fInnerLinerThickMm = 2.5f,
            float fOuterJacketThickMm = 3.0f,
            float fHelixAngleDeg = 15.0f)
        {
            // 1. Obtener los vóxeles de la pared interna base (Liner)
            Voxels voxLiner = NozzleGenerator.voxCreateFig422Nozzle(
                fWallThicknessMm: fInnerLinerThickMm + fChannelHeightMm);

            // 2. Generar el sólido de los canales de refrigeración (Herramienta de corte)
            Mesh meshChannels = CreateChannelToolMesh(
                nChannelCount, fChannelWidthMm, fChannelHeightMm, 
                fInnerLinerThickMm, fHelixAngleDeg);

            Voxels voxChannels = new Voxels(meshChannels);

            // 3. Generar la camisa exterior de sellado (Outer Jacket)
            float fTotalThickness = fInnerLinerThickMm + fChannelHeightMm + fOuterJacketThickMm;
            Voxels voxOuterJacket = NozzleGenerator.voxCreateFig422Nozzle(
                fWallThicknessMm: fTotalThickness);

            // 4. Operaciones Booleanas Voxel en PikoGK
            // Restar la matriz de canales del Liner ampliado
            voxLiner.Subtract(voxChannels);

            // Unir la camisa exterior para sellar los canales por fuera
            voxLiner.Add(voxOuterJacket);

            return voxLiner;
        }

        private static Mesh CreateChannelToolMesh(
            int nChannels,
            float fWidth,
            float fHeight,
            float fRadialOffset,
            float fHelixAngleDeg)
        {
            Mesh meshTool = new Mesh();
            float fDTheta = (2.0f * (float)Math.PI) / nChannels;
            float fHelixRad = fHelixAngleDeg * (float)Math.PI / 180.0f;

            // Recorrido axial simplificado para construir las barras de corte de los canales
            int nAxialSteps = 100;
            float fZStart = -406.4f; // Longitud de cámara en mm
            float fZEnd = 1729.74f;  // Longitud de tobera en mm
            float fDz = (fZEnd - fZStart) / nAxialSteps;

            for (int c = 0; c < nChannels; c++)
            {
                float fBaseAngle = c * fDTheta;

                for (int i = 0; i < nAxialSteps; i++)
                {
                    float z1 = fZStart + (i * fDz);
                    float z2 = fZStart + ((i + 1) * fDz);

                    // Radio local de la pared interna (aproximación cuadrática simplificada)
                    float rLocal1 = GetLocalRadiusAtZ(z1) + fRadialOffset;
                    float rLocal2 = GetLocalRadiusAtZ(z2) + fRadialOffset;

                    // Torsión helicoidal acumulada
                    float theta1 = fBaseAngle + (z1 * (float)Math.Tan(fHelixRad) / rLocal1);
                    float theta2 = fBaseAngle + (z2 * (float)Math.Tan(fHelixRad) / rLocal2);

                    // Generar sección transversal rectangular (4 vértices por capa)
                    AddChannelSegment(meshTool, z1, z2, rLocal1, rLocal2, theta1, theta2, fWidth, fHeight);
                }
            }

            return meshTool;
        }

        private static float GetLocalRadiusAtZ(float z)
        {
            // Retorna el radio interno de la tobera en función de Z (mm)
            float fRt = 134.11f; // Radio garganta
            if (z < 0) return fRt + (z * z * 0.001f);
            return fRt + (float)Math.Sqrt(z * 180.0f);
        }

        private static void AddChannelSegment(
            Mesh mesh, float z1, float z2, float r1, float r2, 
            float t1, float t2, float w, float h)
        {
            // Puntos de la sección 1 (Z1)
            Vector3 p1 = new Vector3(z1, (r1) * (float)Math.Cos(t1), (r1) * (float)Math.Sin(t1));
            Vector3 p2 = new Vector3(z1, (r1 + h) * (float)Math.Cos(t1), (r1 + h) * (float)Math.Sin(t1));

            // Puntos de la sección 2 (Z2)
            Vector3 p3 = new Vector3(z2, (r2) * (float)Math.Cos(t2), (r2) * (float)Math.Sin(t2));
            Vector3 p4 = new Vector3(z2, (r2 + h) * (float)Math.Cos(t2), (r2 + h) * (float)Math.Sin(t2));

            // Agregar volumen extruido al Mesh
            int v1 = mesh.nAddVertex(p1);
            int v2 = mesh.nAddVertex(p2);
            int v3 = mesh.nAddVertex(p3);
            int v4 = mesh.nAddVertex(p4);

            mesh.AddTriangle(v1, v2, v3);
            mesh.AddTriangle(v2, v4, v3);
        }
    }
}