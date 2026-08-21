// Geometry_Turbopump_Triple_v07.cs
//
// Triple turbopump geometry: LOX rotor, CH4 rotor, preburner rotor.
// Theory: Compound shaft couples LOX and CH4 pumps; preburner turbine extracts power.
// Using PicoGK Voxels API.

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Turbopump_Triple_v07
    {
        public static Voxels Build(EngineParams p)
        {
            float RR = (float)(p.TurbopumpRPM > 0 ? 0.16 : 0.12f);
            float HR = 0.05f;
            float BH = 0.06f;

            Voxels geometry = new Voxels();

            // LOX pump rotor
            Voxels loxRotor = BuildRotor(RR, HR, BH, 10, 0.04f, new Vector3(0, 0, BH * 0.5f));
            geometry += loxRotor;

            // CH4 pump rotor
            Voxels ch4Rotor = BuildRotor(RR * 0.9f, HR * 0.9f, BH * 0.9f, 10, 0.038f, new Vector3(RR * 2.0f, 0, BH * 0.5f));
            geometry += ch4Rotor;

            // Preburner turbine rotor
            Voxels turbineRotor = BuildRotor(RR * 1.1f, HR * 1.0f, BH * 1.2f, 12, 0.035f, new Vector3(-RR * 2.0f, 0, BH * 0.5f));
            geometry += turbineRotor;

            // Compound shaft
            Voxels shaft = Voxels.voxSphere(new Vector3(0, 0, -0.15f), 0.03f);
            shaft += Voxels.voxSphere(new Vector3(0, 0, -0.05f), 0.03f);
            shaft += Voxels.voxSphere(new Vector3(RR * 2.0f, 0, -0.05f), 0.025f);
            shaft += Voxels.voxSphere(new Vector3(-RR * 2.0f, 0, -0.05f), 0.025f);
            geometry += shaft;

            // Triple volute housing
            Voxels housing = BuildVolute(RR, BH, new Vector3(0, 0, BH * 0.6f));
            housing += BuildVolute(RR * 0.9f, BH * 0.9f, new Vector3(RR * 2.0f, 0, BH * 0.6f));
            housing += BuildVolute(RR * 1.1f, BH * 1.2f, new Vector3(-RR * 2.0f, 0, BH * 0.6f));
            geometry += housing;

            return geometry;
        }

        private static Voxels BuildRotor(float radius, float hubRadius, float bladeHeight, int bladeCount, float bladeChord, Vector3 offset)
        {
            Voxels rotor = new Voxels();

            // Hub
            for (int i = 0; i < 4; i++)
            {
                float z = offset.Z + i * bladeHeight / 4.0f;
                rotor += Voxels.voxSphere(new Vector3(offset.X, offset.Y, z), hubRadius);
            }

            // Blades
            for (int i = 0; i < bladeCount; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / bladeCount;
                float midR = (radius + hubRadius) / 2.0f;
                float x = offset.X + (float)Math.Cos(ang) * midR;
                float y = offset.Y + (float)Math.Sin(ang) * midR;

                rotor += Voxels.voxSphere(new Vector3(x, y, offset.Z + bladeHeight * 0.5f), bladeChord);
                rotor += Voxels.voxSphere(new Vector3(x * 1.1f, y * 1.1f, offset.Z + bladeHeight * 0.5f), bladeChord * 0.8f);
            }

            return rotor;
        }

        private static Voxels BuildVolute(float radius, float height, Vector3 offset)
        {
            Voxels volute = Voxels.voxSphere(new Vector3(offset.X + radius * 0.9f, offset.Y, offset.Z), 0.05f);
            for (int i = 1; i < 8; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / 8.0f;
                float x = offset.X + (float)Math.Cos(ang) * radius * 0.9f;
                float y = offset.Y + (float)Math.Sin(ang) * radius * 0.9f;
                volute += Voxels.voxSphere(new Vector3(x, y, offset.Z), 0.04f);
            }

            Voxels housing = Voxels.voxSphere(new Vector3(offset.X, offset.Y, offset.Z + height * 0.6f), radius * 1.2f);
            housing += Voxels.voxSphere(new Vector3(offset.X, offset.Y, offset.Z + height * 1.0f), radius * 1.1f);
            return volute + housing;
        }
    }
}
