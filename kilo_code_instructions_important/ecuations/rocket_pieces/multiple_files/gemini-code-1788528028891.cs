using System;
using System.Numerics;
using PicoGK;

namespace RocketEngineDesign
{
    public static class ThrusterGenerator
    {
        /// <summary>
        /// Genera el sólido de revolución estanco (2-manifold) para la pared del motor.
        /// </summary>
        public static Voxels VoxBuildEngineShell(
            float fTotalHeight,
            float fThroatZ,
            float fChamberRadius,
            float fThroatRadius,
            float fNozzleRadius,
            float fWallThickness,
            int nRadialSteps = 72,
            int nStepsZ = 100)
        {
            Mesh meshShell = new Mesh();
            float dZ = fTotalHeight / nStepsZ;
            float dTh = 2.0f * (float)Math.PI / nRadialSteps;

            int[,] innerGrid = new int[nStepsZ + 1, nRadialSteps];
            int[,] outerGrid = new int[nStepsZ + 1, nRadialSteps];

            for (int i = 0; i <= nStepsZ; i++)
            {
                float z = i * dZ;
                float rInner = ComputeRadius(z, fTotalHeight, fThroatZ, fChamberRadius, fThroatRadius, fNozzleRadius);
                float rOuter = rInner + fWallThickness;

                for (int j = 0; j < nRadialSteps; j++)
                {
                    float th = j * dTh;
                    float cos = (float)Math.Cos(th);
                    float sin = (float)Math.Sin(th);

                    innerGrid[i, j] = meshShell.nAddVertex(new Vector3(rInner * cos, rInner * sin, z));
                    outerGrid[i, j] = meshShell.nAddVertex(new Vector3(rOuter * cos, rOuter * sin, z));
                }
            }

            for (int i = 0; i < nStepsZ; i++)
            {
                for (int j = 0; j < nRadialSteps; j++)
                {
                    int jNext = (j + 1) % nRadialSteps;

                    // Pared interna
                    meshShell.AddTriangle(innerGrid[i, j], innerGrid[i, jNext], innerGrid[i + 1, jNext]);
                    meshShell.AddTriangle(innerGrid[i, j], innerGrid[i + 1, jNext], innerGrid[i + 1, j]);

                    // Pared externa
                    meshShell.AddTriangle(outerGrid[i, j], outerGrid[i + 1, jNext], outerGrid[i, jNext]);
                    meshShell.AddTriangle(outerGrid[i, j], outerGrid[i + 1, j], outerGrid[i + 1, jNext]);
                }
            }

            // Tapas estancas en Z = 0 y Z = fTotalHeight
            for (int j = 0; j < nRadialSteps; j++)
            {
                int jNext = (j + 1) % nRadialSteps;

                // Tapa Base Z = 0
                meshShell.AddTriangle(innerGrid[0, j], outerGrid[0, j], outerGrid[0, jNext]);
                meshShell.AddTriangle(innerGrid[0, j], outerGrid[0, jNext], innerGrid[0, jNext]);

                // Tapa Superior Z = fTotalHeight
                meshShell.AddTriangle(innerGrid[nStepsZ, j], innerGrid[nStepsZ, jNext], outerGrid[nStepsZ, jNext]);
                meshShell.AddTriangle(innerGrid[nStepsZ, j], outerGrid[nStepsZ, jNext], outerGrid[nStepsZ, j]);
            }

            return new Voxels(meshShell);
        }

        public static float ComputeRadius(float z, float fTotalH, float fThroatZ, float fChamber, float fThroat, float fNozzle)
        {
            if (z <= fThroatZ)
            {
                float t = z / fThroatZ;
                return fChamber + (fThroat - fChamber) * (float)Math.Sin(t * Math.PI * 0.5);
            }
            else
            {
                float t = (z - fThroatZ) / (fTotalH - fThroatZ);
                return fThroat + (fNozzle - fThroat) * t;
            }
        }
    }
}