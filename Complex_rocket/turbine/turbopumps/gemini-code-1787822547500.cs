using System;
using System.Collections.Generic;
using System.Numerics;
using PikoGK;

namespace RocketEngineDesign
{
    public static class NozzleGenerator
    {
        /// <summary>
        /// Genera el volumen (Voxels) de la tobera campana basada en la Figura 4-22.
        /// </summary>
        /// <param name="fWallThicknessMm">Espesor de pared de la tobera en mm (default: 6mm)</param>
        /// <param name="nAxialSteps">Resolución axial de la curva de la tobera</param>
        /// <param name="nRadialSteps">Resolución angular para la revolución 3D</param>
        /// <returns>Objeto Voxels listo para operaciones booleanas o exportación STL en PikoGK</returns>
        public static Voxels voxCreateFig422Nozzle(
            float fWallThicknessMm = 6.0f,
            int nAxialSteps = 150,
            int nRadialSteps = 128)
        {
            // --- 1. Conversión de unidades e insumos (Pulgadas a mm) ---
            const float fInToMm = 25.4f;

            float fDc = 14.98f * fInToMm;                 // Diámetro cámara: 380.49 mm
            float fDt = 10.56f * fInToMm;                 // Diámetro garganta: 268.22 mm
            float fDe = 62.50f * fInToMm;                 // Diámetro salida: 1587.50 mm
            float fLn = 68.10f * fInToMm;                 // Longitud tobera: 1729.74 mm
            float fLchamber = 16.0f * fInToMm;            // Longitud cámara: 406.40 mm

            float fThetaN = 34.0f * (float)Math.PI / 180.0f; // Ángulo inicial de expansión (rad)
            float fThetaE = 10.5f * (float)Math.PI / 180.0f; // Ángulo de salida (rad)
            float fRthroatArc = 2.02f * fInToMm;            // Radio de curvatura aguas abajo (51.31 mm)
            float fRentryArc = 7.93f * fInToMm;             // Radio de curvatura aguas arriba (201.42 mm)
            float fConvAngle = 25.0f * (float)Math.PI / 180.0f; // Ángulo del cono convergente

            float fRc = fDc / 2.0f;
            float fRt = fDt / 2.0f;
            float fRe = fDe / 2.0f;

            // --- 2. Generación del perfil 2D interno (Eje Z = Axial, Eje Y = Radio) ---
            List<Vector2> profileInner = GenerateNozzleProfile2D(
                fRc, fRt, fRe, fLchamber, fLn,
                fThetaN, fThetaE, fConvAngle, fRthroatArc, fRentryArc, nAxialSteps);

            // --- 3. Generación de la malla revolucionada 3D con espesor ---
            Mesh meshNozzle = RevolveProfileToMesh(profileInner, fWallThicknessMm, nRadialSteps);

            // --- 4. Voxelización en PikoGK ---
            return new Voxels(meshNozzle);
        }

        private static List<Vector2> GenerateNozzleProfile2D(
            float fRc, float fRt, float fRe, float fLchamber, float fLn,
            float fThetaN, float fThetaE, float fConvAngle, float fRthroatArc, float fRentryArc, int nSteps)
        {
            List<Vector2> profile = new List<Vector2>();

            // A. Sección de la Cámara de Combustión
            profile.Add(new Vector2(-fLchamber, fRc));

            // B. Arco de convergencia y Garganta (Aguas arriba Z < 0)
            float zArcStart = -fRentryArc * (float)Math.Sin(fConvAngle);
            float rArcStart = fRt + fRentryArc * (1.0f - (float)Math.Cos(fConvAngle));
            profile.Add(new Vector2(zArcStart, rArcStart));

            // Punto de la Garganta (Z = 0, R = Rt)
            profile.Add(new Vector2(0.0f, fRt));

            // C. Arco circular de Garganta (Aguas abajo Z > 0, de 0 a Theta_N)
            Vector2 pN = new Vector2(
                fRthroatArc * (float)Math.Sin(fThetaN),
                fRt + fRthroatArc * (1.0f - (float)Math.Cos(fThetaN))
            );

            for (int i = 1; i <= nSteps / 4; i++)
            {
                float t = i / (float)(nSteps / 4);
                float angle = t * fThetaN;
                float z = fRthroatArc * (float)Math.Sin(angle);
                float r = fRt + fRthroatArc * (1.0f - (float)Math.Cos(angle));
                profile.Add(new Vector2(z, r));
            }

            // D. Parábola de la Campana (Método Rao - Bézier Cuadrática entre N y E)
            Vector2 pE = new Vector2(fLn, fRe);

            // Intersección de rectas tangentes en N (tan(Theta_N)) y E (tan(Theta_E)) -> Punto de control Q
            float m1 = (float)Math.Tan(fThetaN);
            float m2 = (float)Math.Tan(fThetaE);

            float zQ = (pE.Y - pN.Y + m1 * pN.X - m2 * pE.X) / (m1 - m2);
            float rQ = pN.Y + m1 * (zQ - pN.X);
            Vector2 pQ = new Vector2(zQ, rQ);

            // Muestreo de la Bézier (1 - t)^2 * N + 2(1 - t)t * Q + t^2 * E
            for (int i = 1; i <= nSteps; i++)
            {
                float t = i / (float)nSteps;
                float u = 1.0f - t;

                Vector2 pBezier = (u * u * pN) + (2.0f * u * t * pQ) + (t * t * pE);
                profile.Add(pBezier);
            }

            return profile;
        }

        private static Mesh RevolveProfileToMesh(List<Vector2> innerProfile, float fThickness, int nRadialSteps)
        {
            Mesh mesh = new Mesh();
            int nPoints = innerProfile.Count;

            // Calcular normales 2D para desplazar la pared exterior
            List<Vector2> outerProfile = new List<Vector2>();
            for (int i = 0; i < nPoints; i++)
            {
                Vector2 tangent;
                if (i == 0) tangent = innerProfile[1] - innerProfile[0];
                else if (i == nPoints - 1) tangent = innerProfile[nPoints - 1] - innerProfile[nPoints - 2];
                else tangent = innerProfile[i + 1] - innerProfile[i - 1];

                tangent = Vector2.Normalize(tangent);
                Vector2 normal = new Vector2(-tangent.Y, tangent.X); // Normal apuntando hacia afuera
                outerProfile.Add(innerProfile[i] + normal * fThickness);
            }

            // Crear Vértices revolucionados en Z
            float fDTheta = 2.0f * (float)Math.PI / nRadialSteps;

            // Arreglo de índices de vértices [capa_radial, punto_perfil]
            int[,] innerIndices = new int[nRadialSteps, nPoints];
            int[,] outerIndices = new int[nRadialSteps, nPoints];

            for (int r = 0; r < nRadialSteps; r++)
            {
                float theta = r * fDTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                for (int i = 0; i < nPoints; i++)
                {
                    // Vértice interno (Z, X, Y)
                    Vector3 vIn = new Vector3(innerProfile[i].X, innerProfile[i].Y * cosT, innerProfile[i].Y * sinT);
                    innerIndices[r, i] = mesh.nAddVertex(vIn);

                    // Vértice externo (Z, X, Y)
                    Vector3 vOut = new Vector3(outerProfile[i].X, outerProfile[i].Y * cosT, outerProfile[i].Y * sinT);
                    outerIndices[r, i] = mesh.nAddVertex(vOut);
                }
            }

            // Construir Caras Triangulares
            for (int r = 0; r < nRadialSteps; r++)
            {
                int rNext = (r + 1) % nRadialSteps;

                for (int i = 0; i < nPoints - 1; i++)
                {
                    int iNext = i + 1;

                    // Malla Interna
                    mesh.AddTriangle(innerIndices[r, i], innerIndices[rNext, i], innerIndices[rNext, iNext]);
                    mesh.AddTriangle(innerIndices[r, i], innerIndices[rNext, iNext], innerIndices[r, iNext]);

                    // Malla Externa (orientación invertida para normales exteriores)
                    mesh.AddTriangle(outerIndices[r, i], outerIndices[rNext, iNext], outerIndices[rNext, i]);
                    mesh.AddTriangle(outerIndices[r, i], outerIndices[r, iNext], outerIndices[rNext, iNext]);
                }

                // Cierre frontal (Inyector, i = 0)
                mesh.AddTriangle(innerIndices[r, 0], outerIndices[r, 0], outerIndices[rNext, 0]);
                mesh.AddTriangle(innerIndices[r, 0], outerIndices[rNext, 0], innerIndices[rNext, 0]);

                // Cierre posterior (Labio de tobera, i = nPoints - 1)
                int last = nPoints - 1;
                mesh.AddTriangle(innerIndices[r, last], innerIndices[rNext, last], outerIndices[rNext, last]);
                mesh.AddTriangle(innerIndices[r, last], outerIndices[rNext, last], outerIndices[r, last]);
            }

            return mesh;
        }
    }
}