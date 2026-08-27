// ComplexRocketTasks.cs
//
// Tareas modulares para el diseno de cohete complejo.
// Implementa turbobomba, inyectores, prequemador, tobera, refrigeracion y ensamblaje.
// Adaptado a PicoGK 1.7.7.4 API.

using PicoGK;
using System.Numerics;

namespace ComplexRocket.Tasks
{
    public static class TurbopumpTask
    {
        public static Voxels BuildTurbopump(
            Vector3 origin,
            float housingRadius = 60.0f,
            float housingLength = 120.0f,
            float rotorRadius = 50.0f,
            float rotorLength = 80.0f,
            float shaftRadius = 12.0f,
            float bladeHeight = 15.0f,
            int bladeCount = 12,
            float voxelSize = 0.5f)
        {
            Voxels pump = new Voxels();

            float hr = housingRadius;
            float hl = housingLength;
            for (float z = -hl / 2.0f; z <= hl / 2.0f; z += voxelSize)
            {
                for (float x = -hr; x <= hr; x += voxelSize)
                for (float y = -hr; y <= hr; y += voxelSize)
                {
                    if (x * x + y * y <= hr * hr)
                    {
                        pump += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            float rr = rotorRadius;
            float rl = rotorLength;
            for (float z = -rl / 2.0f; z <= rl / 2.0f; z += voxelSize)
            {
                float taper = 1.0f - MathF.Abs(z) / (rl / 2.0f) * 0.15f;
                for (float x = -rr * taper; x <= rr * taper; x += voxelSize)
                for (float y = -rr * taper; y <= rr * taper; y += voxelSize)
                {
                    if (x * x + y * y <= rr * rr * taper * taper)
                    {
                        pump += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            for (int i = 0; i < bladeCount; i++)
            {
                float angle = 2.0f * MathF.PI * i / bladeCount;
                float bx = (float)MathF.Cos(angle) * (rr * 0.3f);
                float by = (float)MathF.Sin(angle) * (rr * 0.3f);

                for (float z = -rl / 2.0f; z <= rl / 2.0f; z += voxelSize)
                {
                    pump += Voxels.voxSphere(origin + new Vector3(bx, by, z * 0.8f), bladeHeight);
                }
            }

            float sr = shaftRadius;
            float sh = housingLength * 1.2f;
            for (float z = -sh / 2.0f; z <= sh / 2.0f; z += voxelSize)
            {
                for (float x = -sr; x <= sr; x += voxelSize)
                for (float y = -sr; y <= sr; y += voxelSize)
                {
                    if (x * x + y * y <= sr * sr)
                    {
                        pump += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            return pump;
        }
    }

    public static class InjectorTask
    {
        public static Voxels BuildCoaxialInjectors(
            Vector3 origin,
            float innerPostInnerRadius = 2.5f,
            float innerPostOuterRadius = 3.5f,
            float outerSleeveOuterRadius = 6.4f,
            float injectorLength = 35.0f,
            float ringRadius = 48.0f,
            float centerZ = -5.0f,
            int count = 13,
            float voxelSize = 0.5f)
        {
            Voxels injectors = new Voxels();

            for (int i = 0; i < count; i++)
            {
                float angle = 2.0f * MathF.PI * i / count;
                float x = (float)MathF.Cos(angle) * ringRadius;
                float y = (float)MathF.Sin(angle) * ringRadius;
                Vector3 pos = origin + new Vector3(x, y, centerZ);

                for (float z = 0; z <= injectorLength; z += voxelSize)
                {
                    for (float dx = -outerSleeveOuterRadius; dx <= outerSleeveOuterRadius; dx += voxelSize)
                    for (float dy = -outerSleeveOuterRadius; dy <= outerSleeveOuterRadius; dy += voxelSize)
                    {
                        if (dx * dx + dy * dy <= outerSleeveOuterRadius * outerSleeveOuterRadius)
                        {
                            injectors += Voxels.voxSphere(pos + new Vector3(dx, dy, z), voxelSize * 0.9f);
                        }
                    }
                }

                for (float z = 0; z <= injectorLength; z += voxelSize)
                {
                    for (float dx = -innerPostOuterRadius; dx <= innerPostOuterRadius; dx += voxelSize)
                    for (float dy = -innerPostOuterRadius; dy <= innerPostOuterRadius; dy += voxelSize)
                    {
                        if (dx * dx + dy * dy <= innerPostOuterRadius * innerPostOuterRadius)
                        {
                            injectors += Voxels.voxSphere(pos + new Vector3(dx, dy, z), voxelSize * 0.9f);
                        }
                    }
                }
            }

            return injectors;
        }

        public static Voxels BuildInjectorPlate(
            Vector3 origin,
            float plateRadius = 80.0f,
            float plateThickness = 12.0f,
            float holeRadius = 6.5f,
            float ringRadius = 48.0f,
            float centerZ = 0.0f,
            float voxelSize = 0.5f)
        {
            Voxels plate = new Voxels();

            for (float z = -plateThickness / 2.0f; z <= plateThickness / 2.0f; z += voxelSize)
            {
                for (float x = -plateRadius; x <= plateRadius; x += voxelSize)
                for (float y = -plateRadius; y <= plateRadius; y += voxelSize)
                {
                    if (x * x + y * y <= plateRadius * plateRadius)
                    {
                        plate += Voxels.voxSphere(origin + new Vector3(x, y, centerZ + z), voxelSize * 0.9f);
                    }
                }
            }

            Voxels holes = new Voxels();
            for (int i = 0; i < 13; i++)
            {
                float angle = 2.0f * MathF.PI * i / 13;
                float x = (float)MathF.Cos(angle) * ringRadius;
                float y = (float)MathF.Sin(angle) * ringRadius;

                for (float z = -plateThickness; z <= plateThickness; z += voxelSize)
                {
                    holes += Voxels.voxSphere(origin + new Vector3(x, y, centerZ + z), holeRadius);
                }
            }

            return plate - holes;
        }
    }

    public static class PreburnerTask
    {
        public static Voxels BuildPreburner(
            Vector3 origin,
            float housingRadius = 85.0f,
            float wallThickness = 5.0f,
            float chamberLength = 90.0f,
            float ductLength = 120.0f,
            float ductExitRadius = 45.0f,
            float manifoldMajorRadius = 95.0f,
            float manifoldMinorRadius = 22.0f,
            float voxelSize = 0.5f)
        {
            Voxels preburner = new Voxels();

            float hr = housingRadius;
            float hl = chamberLength + ductLength;
            for (float z = 0; z <= hl; z += voxelSize)
            {
                float r = hr;
                if (z > chamberLength)
                {
                    float t = (z - chamberLength) / ductLength;
                    r = hr * (1.0f - t) + ductExitRadius * t;
                }
                for (float x = -r; x <= r; x += voxelSize)
                for (float y = -r; y <= r; y += voxelSize)
                {
                    if (x * x + y * y <= r * r)
                    {
                        preburner += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            Voxels cavity = new Voxels();
            float ir = hr - wallThickness;
            for (float z = 0; z <= hl; z += voxelSize)
            {
                float r = ir;
                if (z > chamberLength)
                {
                    float t = (z - chamberLength) / ductLength;
                    r = ir * (1.0f - t) + (ductExitRadius - wallThickness) * t;
                }
                for (float x = -r; x <= r; x += voxelSize)
                for (float y = -r; y <= r; y += voxelSize)
                {
                    if (x * x + y * y <= r * r)
                    {
                        cavity += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            preburner = preburner - cavity;

            float majorR = manifoldMajorRadius;
            float minorR = manifoldMinorRadius;
            for (float theta = 0; theta < 2.0f * MathF.PI; theta += voxelSize / majorR)
            {
                float cx = (float)MathF.Cos(theta) * majorR;
                float cy = (float)MathF.Sin(theta) * majorR;
                for (float phi = 0; phi < 2.0f * MathF.PI; phi += voxelSize / minorR)
                {
                    float x = cx + (float)MathF.Cos(phi) * minorR;
                    float y = cy + (float)MathF.Sin(phi) * minorR;
                    float z = 15.0f + (float)MathF.Sin(phi) * minorR;
                    preburner += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                }
            }

            return preburner;
        }
    }

    public static class NozzleTask
    {
        public static Voxels BuildNozzle(
            Vector3 origin,
            float wallThickness = 6.0f,
            float voxelSize = 0.5f)
        {
            const float inToMm = 25.4f;
            float dc = 14.98f * inToMm;
            float dt = 10.56f * inToMm;
            float de = 62.50f * inToMm;
            float ln = 68.10f * inToMm;
            float lchamber = 16.0f * inToMm;

            float rc = dc / 2.0f;
            float rt = dt / 2.0f;
            float re = de / 2.0f;

            Voxels nozzle = new Voxels();

            for (float z = -lchamber; z <= 0; z += voxelSize)
            {
                float t = (z + lchamber) / lchamber;
                float r = rc * (1.0f - t) + rt * t;
                for (float x = -r; x <= r; x += voxelSize)
                for (float y = -r; y <= r; y += voxelSize)
                {
                    if (x * x + y * y <= r * r)
                    {
                        nozzle += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            for (float z = 0; z <= ln; z += voxelSize)
            {
                float t = z / ln;
                float r = rt + (re - rt) * (float)MathF.Pow(t, 1.5f);
                for (float x = -r; x <= r; x += voxelSize)
                for (float y = -r; y <= r; y += voxelSize)
                {
                    if (x * x + y * y <= r * r)
                    {
                        nozzle += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            Voxels outer = new Voxels();
            for (float z = -lchamber; z <= ln; z += voxelSize)
            {
                float r;
                if (z <= 0)
                {
                    float t = (z + lchamber) / lchamber;
                    r = (rc + wallThickness) * (1.0f - t) + (rt + wallThickness) * t;
                }
                else
                {
                    float t = z / ln;
                    r = (rt + wallThickness) + (re + wallThickness - rt - wallThickness) * (float)MathF.Pow(t, 1.5f);
                }
                for (float x = -r; x <= r; x += voxelSize)
                for (float y = -r; y <= r; y += voxelSize)
                {
                    if (x * x + y * y <= r * r)
                    {
                        outer += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            return outer - nozzle;
        }
    }

    public static class CoolingTask
    {
        public static Voxels BuildRegenCooling(
            Voxels nozzle,
            int channelCount = 96,
            float channelWidth = 3.5f,
            float channelHeight = 5.0f,
            float voxelSize = 0.5f)
        {
            Voxels channels = new Voxels();

            for (int c = 0; c < channelCount; c++)
            {
                float angle = 2.0f * MathF.PI * c / channelCount;
                float cx = (float)MathF.Cos(angle) * 150.0f;
                float cy = (float)MathF.Sin(angle) * 150.0f;

                for (float z = -400.0f; z <= 1700.0f; z += voxelSize)
                {
                    float r = 150.0f + channelHeight;
                    channels += Voxels.voxSphere(new Vector3(cx, cy, z), channelWidth);
                }
            }

            return nozzle - channels;
        }
    }

    public static class DuctTask
    {
        public static Voxels BuildHotGasDuct(
            Vector3 origin,
            float bendRadius = 75.0f,
            float outerRadius = 16.0f,
            float wallThickness = 3.0f,
            float bendAngleDeg = 90.0f,
            float flangeRadius = 24.0f,
            float flangeThickness = 8.0f,
            float voxelSize = 0.5f)
        {
            Voxels duct = new Voxels();
            float bendAngleRad = bendAngleDeg * MathF.PI / 180.0f;
            float innerRadius = outerRadius - wallThickness;

            int steps = 64;
            for (float angle = 0; angle <= bendAngleRad; angle += bendAngleRad / steps)
            {
                float x = bendRadius * (1.0f - (float)MathF.Cos(angle));
                float y = bendRadius * (float)MathF.Sin(angle);

                for (float dx = -outerRadius; dx <= outerRadius; dx += voxelSize)
                for (float dy = -outerRadius; dy <= outerRadius; dy += voxelSize)
                for (float dz = -outerRadius; dz <= outerRadius; dz += voxelSize)
                {
                    float dist = (float)MathF.Sqrt(dx * dx + dy * dy);
                    if (dist <= outerRadius)
                    {
                        duct += Voxels.voxSphere(origin + new Vector3(x + dx, y + dy, dz), voxelSize * 0.9f);
                    }
                }
            }

            for (float z = -flangeThickness; z <= 0; z += voxelSize)
            {
                for (float dx = -flangeRadius; dx <= flangeRadius; dx += voxelSize)
                for (float dy = -flangeRadius; dy <= flangeRadius; dy += voxelSize)
                {
                    if (dx * dx + dy * dy <= flangeRadius * flangeRadius)
                    {
                        duct += Voxels.voxSphere(origin + new Vector3(dx, dy, z), voxelSize * 0.9f);
                    }
                }
            }

            Voxels cavity = new Voxels();
            for (float angle = 0; angle <= bendAngleRad; angle += bendAngleRad / steps)
            {
                float x = bendRadius * (1.0f - (float)MathF.Cos(angle));
                float y = bendRadius * (float)MathF.Sin(angle);

                for (float dx = -innerRadius; dx <= innerRadius; dx += voxelSize)
                for (float dy = -innerRadius; dy <= innerRadius; dy += voxelSize)
                for (float dz = -innerRadius; dz <= innerRadius; dz += voxelSize)
                {
                    float dist = (float)MathF.Sqrt(dx * dx + dy * dy);
                    if (dist <= innerRadius)
                    {
                        cavity += Voxels.voxSphere(origin + new Vector3(x + dx, y + dy, dz), voxelSize * 0.9f);
                    }
                }
            }

            return duct - cavity;
        }
    }

    public static class AssemblyTask
    {
        public static Voxels AssembleEngine(float voxelSize = 0.5f)
        {
            Voxels engine = new Voxels();

            engine += PreburnerTask.BuildPreburner(new Vector3(0, 0, 0), voxelSize: voxelSize);
            engine += InjectorTask.BuildInjectorPlate(new Vector3(0, 0, -20.0f), voxelSize: voxelSize);
            engine += InjectorTask.BuildCoaxialInjectors(new Vector3(0, 0, -25.0f), voxelSize: voxelSize);
            engine += TurbopumpTask.BuildTurbopump(new Vector3(-200.0f, 0, 0), voxelSize: voxelSize);
            
            Voxels nozzle = NozzleTask.BuildNozzle(new Vector3(0, 0, 100.0f), voxelSize: voxelSize);
            engine += nozzle;
            
            Voxels cooledNozzle = CoolingTask.BuildRegenCooling(nozzle, voxelSize: voxelSize);
            engine = engine - nozzle + cooledNozzle;
            
            engine += DuctTask.BuildHotGasDuct(new Vector3(100.0f, 0, 50.0f), voxelSize: voxelSize);

            return engine;
        }
    }

    public static class SmoothingTask
    {
        public static Voxels ApplySmoothing(Voxels input, float filletRadius = 1.2f, float blurSigma = 0.9f)
        {
            Voxels smoothed = input.voxDuplicate();
            smoothed.voxOffset(filletRadius);
            smoothed.Gaussian(blurSigma);
            smoothed.voxOffset(-filletRadius);
            return smoothed;
        }
    }
}
