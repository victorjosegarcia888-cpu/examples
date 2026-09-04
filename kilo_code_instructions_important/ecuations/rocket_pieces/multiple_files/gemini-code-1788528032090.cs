using System;
using System.Numerics;
using PicoGK;

namespace RocketEngineDesign
{
    public static class RegenerativeCooling
    {
        /// <summary>
        /// Genera la malla unificada de todos los canales de refrigeración helicoidales.
        /// </summary>
        public static Voxels VoxGenerateCoolingChannels(
            float fTotalHeight,
            float fThroatZ,
            float fChamberRadius,
            float fThroatRadius,
            float fNozzleRadius,
            float fWallThickness,
            int nChannelCount,
            float fChannelWidthMm,
            float fChannelDepthMm,
            float fHelixTurns,
            int nStepsZ = 80)
        {
            Mesh meshAllChannels = new Mesh();
            float fDeltaPhi = (2.0f * (float)Math.PI) / nChannelCount;
            float dZ = fTotalHeight / nStepsZ;

            for (int n = 0; n < nChannelCount; n++)
            {
                float fStartAngle = n * fDeltaPhi;
                int[][] channelVertices = new int[nStepsZ + 1][];

                for (int i = 0; i <= nStepsZ; i++)
                {
                    float z = i * dZ;
                    float progress = z / fTotalHeight;

                    float rInner = ThrusterGenerator.ComputeRadius(z, fTotalHeight, fThroatZ, fChamberRadius, fThroatRadius, fNozzleRadius) + (fWallThickness * 0.15f);
                    float rOuter = rInner + fChannelDepthMm;

                    float currentAngle = fStartAngle + (progress * fHelixTurns * 2.0f * (float)Math.PI);
                    float halfWidthAngle = (fChannelWidthMm * 0.5f) / rInner;

                    float aLeft = currentAngle - halfWidthAngle;
                    float aRight = currentAngle + halfWidthAngle;

                    Vector3 p0 = new Vector3(rInner * (float)Math.Cos(aLeft),  rInner * (float)Math.Sin(aLeft),  z);
                    Vector3 p1 = new Vector3(rInner * (float)Math.Cos(aRight), rInner * (float)Math.Sin(aRight), z);
                    Vector3 p2 = new Vector3(rOuter * (float)Math.Cos(aRight), rOuter * (float)Math.Sin(aRight), z);
                    Vector3 p3 = new Vector3(rOuter * (float)Math.Cos(aLeft),  rOuter * (float)Math.Sin(aLeft),  z);

                    int v0 = meshAllChannels.nAddVertex(p0);
                    int v1 = meshAllChannels.nAddVertex(p1);
                    int v2 = meshAllChannels.nAddVertex(p2);
                    int v3 = meshAllChannels.nAddVertex(p3);

                    channelVertices[i] = new int[] { v0, v1, v2, v3 };
                }

                for (int i = 0; i < nStepsZ; i++)
                {
                    int[] curr = channelVertices[i];
                    int[] next = channelVertices[i + 1];

                    for (int side = 0; side < 4; side++)
                    {
                        int sideNext = (side + 1) % 4;
                        meshAllChannels.AddTriangle(curr[side], curr[sideNext], next[sideNext]);
                        meshAllChannels.AddTriangle(curr[side], next[sideNext], next[side]);
                    }
                }

                // Tapas estancas en Z = 0 y Z = L
                meshAllChannels.AddTriangle(channelVertices[0][0], channelVertices[0][3], channelVertices[0][2]);
                meshAllChannels.AddTriangle(channelVertices[0][0], channelVertices[0][2], channelVertices[0][1]);

                meshAllChannels.AddTriangle(channelVertices[nStepsZ][0], channelVertices[nStepsZ][1], channelVertices[nStepsZ][2]);
                meshAllChannels.AddTriangle(channelVertices[nStepsZ][0], channelVertices[nStepsZ][2], channelVertices[nStepsZ][3]);
            }

            return new Voxels(meshAllChannels);
        }
    }
}