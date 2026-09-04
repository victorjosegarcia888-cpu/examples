// EngineTasks.cs
//
// Modulo de tareas parametricas para motor hibrido regenerativo.
// Genera geometrias volumetricas usando PicoGK 1.7.7.4 API.
// Unidades: mm, voxelSize = 0.25 mm
//
// Arquitectura basada en instrucciones de notas01.txt:
// - Task_GenerateVectorTurbine: rotor axial 26 alabes, D=115.4mm, h=12mm
// - Task_GeneratePreburnerToroid: colector toroidal, R=60mm, espesor=10mm
// - Task_GenerateFuelPumpImpeller: impulsor helicoidal, Din=45mm, Dout=134mm, 12 alabes
// - Task_GenerateAperiodicLattice: campo quasicrystal orden=10, amplitud=0.015m, freq=8.0
// - Task_GenerateCoolingChannels: 120 canales helicoidales, torsion=35°, seccion=1mm
// - Task_GenerateHybridRocketEngine: ensamblaje completo

using PicoGK;
using System.Numerics;

namespace AseRocketEngine.Tasks
{
    public static class Task_GenerateFuelPumpImpeller
    {
        // Parametros base: Din=45mm, Dout=134mm, hb=2.5mm, 12 alabes helicoidales
        public static Voxels Build(
            Vector3 origin,
            float dInMm = 45.0f,
            float dOutMm = 134.0f,
            float bladeHeightMm = 2.5f,
            int bladeCount = 12,
            float hubLengthMm = 40.0f,
            float voxelSize = 0.25f)
        {
            Voxels impeller = new Voxels();
            float rIn = dInMm / 2.0f;
            float rOut = dOutMm / 2.0f;

            // Hub conico
            for (float z = -hubLengthMm / 2.0f; z <= hubLengthMm / 2.0f; z += voxelSize)
            {
                float t = (z + hubLengthMm / 2.0f) / hubLengthMm;
                float r = rIn * (1.0f - t) + rOut * t;
                for (float x = -r; x <= r; x += voxelSize)
                for (float y = -r; y <= r; y += voxelSize)
                {
                    if (x * x + y * y <= r * r)
                    {
                        impeller += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            // 12 alabes helicoidales
            for (int i = 0; i < bladeCount; i++)
            {
                float baseAngle = 2.0f * MathF.PI * i / bladeCount;
                float helixTurns = 1.5f;

                for (float z = -hubLengthMm / 2.0f; z <= hubLengthMm / 2.0f; z += voxelSize)
                {
                    float t = (z + hubLengthMm / 2.0f) / hubLengthMm;
                    float r = rIn + (rOut - rIn) * t;
                    float angle = baseAngle + t * helixTurns * 2.0f * MathF.PI;

                    float bx = (float)MathF.Cos(angle) * r;
                    float by = (float)MathF.Sin(angle) * r;

                    for (float dx = -bladeHeightMm; dx <= bladeHeightMm; dx += voxelSize)
                    for (float dy = -bladeHeightMm; dy <= bladeHeightMm; dy += voxelSize)
                    {
                        if (dx * dx + dy * dy <= bladeHeightMm * bladeHeightMm)
                        {
                            impeller += Voxels.voxSphere(origin + new Vector3(bx + dx, by + dy, z), voxelSize * 0.9f);
                        }
                    }
                }
            }

            // Pasaje de induccion central (vaciado)
            Voxels core = new Voxels();
            float coreR = rIn * 0.6f;
            for (float z = -hubLengthMm / 2.0f; z <= hubLengthMm / 2.0f; z += voxelSize)
            {
                for (float x = -coreR; x <= coreR; x += voxelSize)
                for (float y = -coreR; y <= coreR; y += voxelSize)
                {
                    if (x * x + y * y <= coreR * coreR)
                    {
                        core += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            return impeller - core;
        }
    }

    public static class Task_GenerateVectorTurbine
    {
        // Parametros base: D_mean=115.4mm, h_b=12mm, 26 alabes axiales
        public static Voxels Build(
            Vector3 origin,
            float dMeanMm = 115.4f,
            float bladeHeightMm = 12.0f,
            int bladeCount = 26,
            float diskThicknessMm = 20.0f,
            float diskRadiusMm = 80.0f,
            float voxelSize = 0.25f)
        {
            Voxels rotor = new Voxels();
            float rDisk = diskRadiusMm;

            // Disco central
            for (float z = -diskThicknessMm / 2.0f; z <= diskThicknessMm / 2.0f; z += voxelSize)
            {
                for (float x = -rDisk; x <= rDisk; x += voxelSize)
                for (float y = -rDisk; y <= rDisk; y += voxelSize)
                {
                    if (x * x + y * y <= rDisk * rDisk)
                    {
                        rotor += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            // 26 alabes axiales
            float rBlade = rDisk * 0.9f;
            for (int i = 0; i < bladeCount; i++)
            {
                float angle = 2.0f * MathF.PI * i / bladeCount;
                float bx = (float)MathF.Cos(angle) * rBlade;
                float by = (float)MathF.Sin(angle) * rBlade;

                for (float z = -diskThicknessMm / 2.0f; z <= diskThicknessMm / 2.0f; z += voxelSize)
                {
                    for (float dx = -bladeHeightMm / 2.0f; dx <= bladeHeightMm / 2.0f; dx += voxelSize)
                    for (float dy = -bladeHeightMm; dy <= bladeHeightMm; dy += voxelSize)
                    {
                        float wx = bx + dx;
                        float wy = by + dy;
                        if (wx * wx + wy * wy <= rDisk * rDisk)
                        {
                            rotor += Voxels.voxSphere(origin + new Vector3(wx, wy, z), voxelSize * 0.9f);
                        }
                    }
                }
            }

            return rotor;
        }
    }

    public static class Task_GeneratePreburnerToroid
    {
        // Parametros base: radio=60mm, espesor=10mm
        public static Voxels Build(
            Vector3 origin,
            float housingRadiusMm = 85.0f,
            float wallThicknessMm = 10.0f,
            float chamberLengthMm = 90.0f,
            float ductLengthMm = 120.0f,
            float ductExitRadiusMm = 45.0f,
            float manifoldMajorRadiusMm = 60.0f,
            float manifoldMinorRadiusMm = 22.0f,
            float absorberRingRadiusMm = 70.0f,
            float absorberRingWidthMm = 8.0f,
            float voxelSize = 0.25f)
        {
            Voxels assembly = new Voxels();
            float hr = housingRadiusMm;
            float hl = chamberLengthMm + ductLengthMm;

            // Cuerpo principal
            for (float z = 0; z <= hl; z += voxelSize)
            {
                float r = hr;
                if (z > chamberLengthMm)
                {
                    float t = (z - chamberLengthMm) / ductLengthMm;
                    r = hr * (1.0f - t) + ductExitRadiusMm * t;
                }
                for (float x = -r; x <= r; x += voxelSize)
                for (float y = -r; y <= r; y += voxelSize)
                {
                    if (x * x + y * y <= r * r)
                    {
                        assembly += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            // Vaciado camara interna
            Voxels cavity = new Voxels();
            float ir = hr - wallThicknessMm;
            for (float z = 0; z <= hl; z += voxelSize)
            {
                float r = ir;
                if (z > chamberLengthMm)
                {
                    float t = (z - chamberLengthMm) / ductLengthMm;
                    r = ir * (1.0f - t) + (ductExitRadiusMm - wallThicknessMm) * t;
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

            assembly = assembly - cavity;

            // Colector toroidal de combustible
            float majorR = manifoldMajorRadiusMm;
            float minorR = manifoldMinorRadiusMm;
            for (float theta = 0; theta < 2.0f * MathF.PI; theta += voxelSize / majorR)
            {
                float cx = (float)MathF.Cos(theta) * majorR;
                float cy = (float)MathF.Sin(theta) * majorR;
                for (float phi = 0; phi < 2.0f * MathF.PI; phi += voxelSize / minorR)
                {
                    float x = cx + (float)MathF.Cos(phi) * minorR;
                    float y = cy + (float)MathF.Sin(phi) * minorR;
                    float z = 15.0f + (float)MathF.Sin(phi) * minorR;
                    assembly += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                }
            }

            // Resonador Acustico Anular (vaciado)
            Voxels absorber = new Voxels();
            float ar = absorberRingRadiusMm;
            float aw = absorberRingWidthMm;
            for (float z = 0; z <= chamberLengthMm; z += voxelSize)
            {
                for (float x = -ar - aw; x <= ar + aw; x += voxelSize)
                for (float y = -ar - aw; y <= ar + aw; y += voxelSize)
                {
                    float dist = (float)MathF.Sqrt(x * x + y * y);
                    if (dist >= ar && dist <= ar + aw)
                    {
                        absorber += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            return assembly - absorber;
        }
    }

    public static class Task_GenerateMainChamberHybrid
    {
        // Parametros base: Dc=380mm, Dt=198mm, De=1060mm, wall=2-4mm
        public static Voxels Build(
            Vector3 origin,
            float chamberDiameterMm = 380.0f,
            float throatDiameterMm = 198.0f,
            float exitDiameterMm = 1060.0f,
            float chamberLengthMm = 400.0f,
            float nozzleLengthMm = 1729.0f,
            float wallThicknessMm = 3.0f,
            float voxelSize = 0.25f)
        {
            float rc = chamberDiameterMm / 2.0f;
            float rt = throatDiameterMm / 2.0f;
            float re = exitDiameterMm / 2.0f;
            float lc = chamberLengthMm;
            float ln = nozzleLengthMm;

            // Perfil interior
            Voxels inner = new Voxels();
            for (float z = -lc; z <= ln; z += voxelSize)
            {
                float r;
                if (z <= 0)
                {
                    float t = (z + lc) / lc;
                    r = rc * (1.0f - t) + rt * t;
                }
                else
                {
                    float t = z / ln;
                    r = rt + (re - rt) * (float)MathF.Pow(t, 1.5f);
                }
                for (float x = -r; x <= r; x += voxelSize)
                for (float y = -r; y <= r; y += voxelSize)
                {
                    if (x * x + y * y <= r * r)
                    {
                        inner += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                    }
                }
            }

            // Perfil exterior
            Voxels outer = new Voxels();
            for (float z = -lc; z <= ln; z += voxelSize)
            {
                float r;
                if (z <= 0)
                {
                    float t = (z + lc) / lc;
                    r = (rc + wallThicknessMm) * (1.0f - t) + (rt + wallThicknessMm) * t;
                }
                else
                {
                    float t = z / ln;
                    r = (rt + wallThicknessMm) + (re + wallThicknessMm - rt - wallThicknessMm) * (float)MathF.Pow(t, 1.5f);
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

            return outer - inner;
        }
    }

    public static class Task_GenerateHighPressureDucts
    {
        public static Voxels Build(
            Vector3 origin,
            Vector3 pumpOutletFuel,
            Vector3 pumpOutletOx,
            Vector3 preburnerInletFuel,
            Vector3 preburnerInletOx,
            float outerRadiusMm = 22.0f,
            float wallThicknessMm = 2.0f,
            float voxelSize = 0.25f)
        {
            Voxels ducts = new Voxels();
            float innerRadius = outerRadiusMm - wallThicknessMm;

            // Tuberia de combustible
            ducts += BuildDuct(
                origin + pumpOutletFuel,
                origin + preburnerInletFuel,
                outerRadiusMm,
                innerRadius,
                voxelSize);

            // Tuberia de oxidador
            ducts += BuildDuct(
                origin + pumpOutletOx,
                origin + preburnerInletOx,
                outerRadiusMm,
                innerRadius,
                voxelSize);

            return ducts;
        }

        private static Voxels BuildDuct(
            Vector3 start,
            Vector3 end,
            float outerRadius,
            float innerRadius,
            float voxelSize)
        {
            Voxels duct = new Voxels();
            Vector3 mid1 = start + new Vector3(-20.0f, 30.0f, 15.0f);
            Vector3 mid2 = end + new Vector3(-15.0f, -20.0f, -10.0f);

            // Camino Bezier cubico aproximado
            int steps = 64;
            for (int i = 0; i < steps; i++)
            {
                float t = (float)i / steps;
                Vector3 pos = CubicBezier(start, mid1, mid2, end, t);

                for (float dx = -outerRadius; dx <= outerRadius; dx += voxelSize)
                for (float dy = -outerRadius; dy <= outerRadius; dy += voxelSize)
                for (float dz = -outerRadius; dz <= outerRadius; dz += voxelSize)
                {
                    if (dx * dx + dy * dy + dz * dz <= outerRadius * outerRadius)
                    {
                        duct += Voxels.voxSphere(pos + new Vector3(dx, dy, dz), voxelSize * 0.9f);
                    }
                }
            }

            // Vaciado interior
            Voxels core = new Voxels();
            for (int i = 0; i < steps; i++)
            {
                float t = (float)i / steps;
                Vector3 pos = CubicBezier(start, mid1, mid2, end, t);

                for (float dx = -innerRadius; dx <= innerRadius; dx += voxelSize)
                for (float dy = -innerRadius; dy <= innerRadius; dy += voxelSize)
                for (float dz = -innerRadius; dz <= innerRadius; dz += voxelSize)
                {
                    if (dx * dx + dy * dy + dz * dz <= innerRadius * innerRadius)
                    {
                        core += Voxels.voxSphere(pos + new Vector3(dx, dy, dz), voxelSize * 0.9f);
                    }
                }
            }

            // Anillos de dilatacion (bellows)
            Voxels bellows = new Voxels();
            for (int i = steps / 3; i < 2 * steps / 3; i += 2)
            {
                float t = (float)i / steps;
                Vector3 pos = CubicBezier(start, mid1, mid2, end, t);
                float ringR = outerRadius + 2.5f;

                for (float theta = 0; theta < 2.0f * MathF.PI; theta += voxelSize / ringR)
                {
                    float cx = (float)MathF.Cos(theta) * ringR;
                    float cy = (float)MathF.Sin(theta) * ringR;
                    bellows += Voxels.voxSphere(pos + new Vector3(cx, cy, 0), voxelSize * 0.9f);
                }
            }

            return (duct - core) + bellows;
        }

        private static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1.0f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            return uuu * p0 + 3.0f * uu * t * p1 + 3.0f * u * tt * p2 + ttt * p3;
        }
    }

    public static class Task_GenerateCoolingChannels
    {
        // Parametros base: 120 canales, torsion=35°, seccion=1mm
        public static Voxels Build(
            Voxels chamberBody,
            int channelCount = 120,
            float channelWidthMm = 1.0f,
            float channelHeightMm = 1.0f,
            float helixAngleDeg = 35.0f,
            float voxelSize = 0.25f)
        {
            Voxels channels = new Voxels();
            float helixRad = helixAngleDeg * MathF.PI / 180.0f;

            // Radio aproximado de la camara en funcion de Z
            float GetLocalRadius(float z)
            {
                float rt = 134.11f;
                if (z < 0) return rt + z * z * 0.001f;
                return rt + (float)MathF.Sqrt(z * 180.0f);
            }

            for (int c = 0; c < channelCount; c++)
            {
                float baseAngle = 2.0f * MathF.PI * c / channelCount;
                float zStart = -406.4f;
                float zEnd = 1729.74f;
                int axialSteps = 200;
                float dz = (zEnd - zStart) / axialSteps;

                for (int i = 0; i < axialSteps; i++)
                {
                    float z1 = zStart + i * dz;
                    float z2 = zStart + (i + 1) * dz;
                    float r1 = GetLocalRadius(z1) + channelHeightMm;
                    float r2 = GetLocalRadius(z2) + channelHeightMm;

                    float theta1 = baseAngle + (z1 * (float)MathF.Tan(helixRad) / r1);
                    float theta2 = baseAngle + (z2 * (float)MathF.Tan(helixRad) / r2);

                    // Seccion transversal rectangular
                    for (float dx = -channelWidthMm; dx <= channelWidthMm; dx += voxelSize)
                    for (float dy = -channelHeightMm; dy <= channelHeightMm; dy += voxelSize)
                    {
                        Vector3 p1 = new Vector3(z1, r1 * (float)MathF.Cos(theta1) + dx, r1 * (float)MathF.Sin(theta1) + dy);
                        Vector3 p2 = new Vector3(z2, r2 * (float)MathF.Cos(theta2) + dx, r2 * (float)MathF.Sin(theta2) + dy);
                        channels += Voxels.voxSphere(p1, voxelSize * 0.9f);
                        channels += Voxels.voxSphere(p2, voxelSize * 0.9f);
                    }
                }
            }

            return chamberBody - channels;
        }
    }

    public static class Task_GenerateAperiodicLattice
    {
        // Parametros base: orden=10, amplitud=0.015m, frecuencia=8.0
        public static Voxels Build(
            Vector3 origin,
            float sizeMm = 100.0f,
            int order = 10,
            float amplitudeMm = 15.0f,
            float frequency = 8.0f,
            float voxelSize = 0.25f)
        {
            Voxels lattice = new Voxels();
            float halfSize = sizeMm / 2.0f;

            for (float x = -halfSize; x <= halfSize; x += voxelSize)
            for (float y = -halfSize; y <= halfSize; y += voxelSize)
            for (float z = -halfSize; z <= halfSize; z += voxelSize)
            {
                // Campo quasicrystal simplificado
                float val = 0.0f;
                for (int i = 0; i < order; i++)
                {
                    float fi = (float)i / order;
                    val += (float)MathF.Sin(fi * x * frequency * 0.1f) *
                           (float)MathF.Cos(fi * y * frequency * 0.1f) *
                           (float)MathF.Sin(fi * z * frequency * 0.1f);
                }
                val /= order;

                if (val > amplitudeMm * 0.1f)
                {
                    lattice += Voxels.voxSphere(origin + new Vector3(x, y, z), voxelSize * 0.9f);
                }
            }

            return lattice;
        }
    }

    public static class Task_GenerateHybridRocketEngine
    {
        public static Voxels Build(float voxelSize = 0.25f)
        {
            Voxels engine = new Voxels();

            // Posiciones relativas de cada componente
            Vector3 turbinePos = new Vector3(-250, 150, 0);
            Vector3 preburnerPos = new Vector3(0, 0, 0);
            Vector3 impellerPos = new Vector3(-250, 0, 0);
            Vector3 chamberPos = new Vector3(200, 0, 0);
            Vector3 latticePos = new Vector3(200, 0, 0);

            // 1. Turbina vectorial
            Voxels turbine = Task_GenerateVectorTurbine.Build(turbinePos, voxelSize: voxelSize);
            engine += turbine;

            // 2. Prequemador toroidal
            Voxels preburner = Task_GeneratePreburnerToroid.Build(preburnerPos, voxelSize: voxelSize);
            engine += preburner;

            // 3. Impulsor de bomba
            Voxels impeller = Task_GenerateFuelPumpImpeller.Build(impellerPos, voxelSize: voxelSize);
            engine += impeller;

            // 4. Camara de combustion hibrida (sin canales de refrigeracion)
            Voxels chamber = Task_GenerateMainChamberHybrid.Build(chamberPos, voxelSize: voxelSize);
            engine += chamber;

            // 5. Lattice aperiodico interno
            Voxels lattice = Task_GenerateAperiodicLattice.Build(latticePos, voxelSize: voxelSize);
            engine += lattice;

            // 6. Canales regenerativos (se restan de la camara)
            Voxels coolingChannels = Task_GenerateCoolingChannels.Build(chamber, voxelSize: voxelSize);
            engine = engine - chamber + coolingChannels;

            // 7. Ductos HP
            Vector3 pumpOutletFuel = new Vector3(-200, 0, 0);
            Vector3 pumpOutletOx = new Vector3(-200, 50, 0);
            Vector3 preburnerInletFuel = new Vector3(0, 0, 0);
            Vector3 preburnerInletOx = new Vector3(0, 50, 0);
            Voxels ducts = Task_GenerateHighPressureDucts.Build(
                new Vector3(0, 0, 0),
                pumpOutletFuel,
                pumpOutletOx,
                preburnerInletFuel,
                preburnerInletOx,
                voxelSize: voxelSize);
            engine += ducts;

            return engine;
        }
    }
}
