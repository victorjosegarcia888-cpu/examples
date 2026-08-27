// IgniterVectorModel.cs
//
// Modelado vectorial del igniter para motor FFSC.
// Define trayectorias, electrodos y geometria de ignicion.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Igniters
{
    public class IgniterVectorModel
    {
        public float IgniterRadius { get; set; } = 0.008f;
        public float IgniterLength { get; set; } = 0.030f;
        public float ElectrodeGap { get; set; } = 0.004f;
        public float ElectrodeLength { get; set; } = 0.020f;
        public int ElectrodeCount { get; set; } = 4;
        public float WireRadius { get; set; } = 0.001f;
        public float SparkGap { get; set; } = 0.003f;

        public Voxels CreateSparkGap(Vector3 origin)
        {
            Voxels gap = new Voxels();
            Vector3 dir = new Vector3(0, 0, 1);

            for (int i = 0; i < 8; i++)
            {
                float t = i / 8.0f;
                Vector3 pos = origin + dir * t * SparkGap;
                gap += Voxels.voxSphere(pos, WireRadius * 0.5f);
            }

            return gap;
        }

        public Voxels CreateIgniterBody(Vector3 origin)
        {
            Voxels body = Voxels.voxSphere(origin, IgniterRadius);
            body += Voxels.voxSphere(origin + new Vector3(0, 0, IgniterLength * 0.5f), IgniterRadius * 0.8f);
            return body;
        }

        public Voxels Create()
        {
            Voxels igniter = new Voxels();
            Vector3 origin = new Vector3(0, 0, 0);

            igniter += CreateIgniterBody(origin);

            for (int i = 0; i < ElectrodeCount; i++)
            {
                float angle = 2.0f * MathF.PI * i / ElectrodeCount;
                float ex = (float)MathF.Cos(angle) * ElectrodeGap;
                float ey = (float)MathF.Sin(angle) * ElectrodeGap;
                Vector3 electrodePos = origin + new Vector3(ex, ey, 0);

                igniter += Voxels.voxSphere(electrodePos, WireRadius);
                igniter += Voxels.voxSphere(electrodePos + new Vector3(0, 0, ElectrodeLength), WireRadius);
                igniter += CreateSparkGap(electrodePos + new Vector3(0, 0, ElectrodeLength * 0.5f));
            }

            return igniter;
        }
    }
}
