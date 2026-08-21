// Geometry_Cooling_Active_v07.cs
//
// Active regenerative cooling with dynamic channels.
// Theory: Qnorm-controlled channel width and thermal bypass for transient loads.
// Using PicoGK Voxels API.

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Cooling_Active_v07
    {
        public static Voxels Build(
            double channelBaseWidth = 0.006,
            double channelHeight = 0.0015,
            int channelCount = 120,
            double pitch = 0.02)
        {
            Voxels channels = new Voxels();
            float r = (float)(channelBaseWidth * 0.5);
            float h = (float)channelHeight;
            double altura = 0.45;

            for (double z = 0; z < altura; z += pitch)
            {
                double ang = z * 10.0;
                double x = Math.Cos(ang) * 0.22;
                double y = Math.Sin(ang) * 0.22;

                // Dynamic width modulation based on axial position
                float dynamicRadius = r * (float)(0.8 + 0.4 * Math.Sin(z * 25.0));

                channels += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)z), dynamicRadius);
                channels += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)(z + pitch * 0.5f)), dynamicRadius);
            }

            return channels;
        }

        public static Voxels BuildWithQnorm(
            EngineParams p,
            double[] qnormProfile)
        {
            Voxels channels = new Voxels();
            float h = (float)p.CoolingChannelHeight;
            double altura = 0.45;
            int Nz = qnormProfile.Length;
            double dz = altura / Nz;

            for (int i = 0; i < Nz; i++)
            {
                double z = i * dz;
                double ang = z * 10.0;
                double x = Math.Cos(ang) * 0.22;
                double y = Math.Sin(ang) * 0.22;

                // Channel width scales with thermal load
                double qnorm = qnormProfile[i];
                double dynamicWidth = p.CoolingChannelWidth * (0.6 + 0.8 * qnorm);
                float dynamicRadius = (float)(dynamicWidth * 0.5);

                channels += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)z), dynamicRadius);
                channels += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)(z + dz * 0.5)), dynamicRadius);
            }

            return channels;
        }
    }
}
