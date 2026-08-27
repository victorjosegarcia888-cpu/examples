#region Task 5: Trazado Automático de Tuberías de Alta Presión (Flexible Hot Gas Ducts)
        /// <summary>
        /// Genera el sistema de tuberías de alta presión para conectar los escapes/salidas de 
        /// las turbobombas con las colectores del prequemador. Incorpora curvas Bezier continuas,
        /// pared con espesor paramétrico y juntas de expansión térmicas (bellows).
        /// </summary>
        /// <param name="vFuelPumpOutlet">Punto de salida de la bomba de combustible</param>
        /// <param name="vOxPumpOutlet">Punto de salida de la bomba de oxidador</param>
        /// <param name="vPreburnerInletFuel">Entrada de combustible en el prequemador</param>
        /// <param name="vPreburnerInletOx">Entrada de oxidador en el prequemador</param>
        /// <param name="fOuterRadiusMm">Radio exterior de la tubería</param>
        /// <param name="fWallThicknessMm">Espesor de pared de alta presión</param>
        public static Voxels Task_GenerateHighPressureDucts(
            Vector3 vFuelPumpOutlet,
            Vector3 vOxPumpOutlet,
            Vector3 vPreburnerInletFuel,
            Vector3 vPreburnerInletOx,
            float fOuterRadiusMm = 9.0f,
            float fWallThicknessMm = 2.2f)
        {
            Voxels voxAllDucts = new Voxels();

            // 1. Trazado Tubería de Combustible (Bomba de combustible -> Prequemador)
            List<Vector3> fuelPath = GenerateBezierPath(
                vFuelPumpOutlet,
                vFuelPumpOutlet + new Vector3(-20.0f, 30.0f, 15.0f),  // Punto de control 1
                vPreburnerInletFuel + new Vector3(-15.0f, -20.0f, -10.0f), // Punto de control 2
                vPreburnerInletFuel,
                nSubdivisions: 40
            );

            Voxels voxFuelDuct = CreateSweptPipeWithBellows(fuelPath, fOuterRadiusMm, fWallThicknessMm);
            voxAllDucts.Add(voxFuelDuct);

            // 2. Trazado Tubería de Oxidador (Turbina de oxidador -> Prequemador)
            List<Vector3> oxPath = GenerateBezierPath(
                vOxPumpOutlet,
                vOxPumpOutlet + new Vector3(20.0f, 30.0f, 15.0f),   // Punto de control 1
                vPreburnerInletOx + new Vector3(15.0f, -20.0f, -10.0f),  // Punto de control 2
                vPreburnerInletOx,
                nSubdivisions: 40
            );

            Voxels voxOxDuct = CreateSweptPipeWithBellows(oxPath, fOuterRadiusMm, fWallThicknessMm);
            voxAllDucts.Add(voxOxDuct);

            return voxAllDucts;
        }

        #region Duct & Bellows Helper Builders
        /// <summary>
        /// Construye una tubería a lo largo del camino trayectado, vacía su interior para el fluido 
        /// y añade anómalos de dilatación flexible (Bellows) en los tramos intermedios.
        /// </summary>
        private static Voxels CreateSweptPipeWithBellows(List<Vector3> path, float fOuterRadius, float fThickness)
        {
            Voxels voxOuterSolid = new Voxels();
            Voxels voxInnerCore = new Voxels();
            Voxels voxBellowRings = new Voxels();

            float fInnerRadius = fOuterRadius - fThickness;

            // Generar la tubería uniendo micro-cilindros a lo largo de los waypoints
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 pA = path[i];
                Vector3 pB = path[i + 1];
                Vector3 vDir = pB - pA;
                float fLen = vDir.Length();

                if (fLen < 0.001f) continue;

                // Segmento exterior e interior
                Mesh meshSegmentOuter = CreateCapsuleSegmentMesh(pA, pB, fOuterRadius);
                Mesh meshSegmentInner = CreateCapsuleSegmentMesh(pA, pB, fInnerRadius);

                voxOuterSolid.Add(new Voxels(meshSegmentOuter));
                voxInnerCore.Add(new Voxels(meshSegmentInner));

                // Agregar anillos de fuelle (Bellows/Corrugaciones) en el tercio medio de la tubería
                if (i > path.Count / 3 && i < (2 * path.Count) / 3 && i % 2 == 0)
                {
                    Mesh meshBellowRing = CreateTorusMeshAtPoint(pA, vDir, fOuterRadius + 2.5f, fRingRadius: 1.8f);
                    voxBellowRings.Add(new Voxels(meshBellowRing));
                }
            }

            // Unir exterior con los anillos de dilatación
            voxOuterSolid.Add(voxBellowRings);

            // Vaciado hidrodinámico (Subtract del núcleo del fluido)
            voxOuterSolid.Subtract(voxInnerCore);

            return voxOuterSolid;
        }

        /// <summary>
        /// Genera una curva Cúbica Bezier entre cuatro puntos de control.
        /// </summary>
        private static List<Vector3> GenerateBezierPath(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int nSubdivisions)
        {
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i <= nSubdivisions; i++)
            {
                float t = (float)i / nSubdivisions;
                float u = 1.0f - t;
                float tt = t * t;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * t;

                Vector3 p = uuu * p0;           // (1-t)^3 * P0
                p += 3 * uu * t * p1;           // 3(1-t)^2 * t * P1
                p += 3 * u * tt * p2;           // 3(1-t) * t^2 * P2
                p += ttt * p3;                  // t^3 * P3

                points.Add(p);
            }
            return points;
        }

        private static Mesh CreateCapsuleSegmentMesh(Vector3 pA, Vector3 pB, float fRadius, int nSteps = 16)
        {
            Mesh mesh = new Mesh();
            Vector3 vAxis = Vector3.Normalize(pB - pA);
            
            // Vector ortogonal para construir las circunferencias
            Vector3 vUp = Math.Abs(vAxis.Z) < 0.99f ? Vector3.UnitZ : Vector3.UnitX;
            Vector3 vRight = Vector3.Normalize(Vector3.Cross(vAxis, vUp));
            Vector3 vNormal = Vector3.Normalize(Vector3.Cross(vRight, vAxis));

            float dTh = 2.0f * (float)Math.PI / nSteps;
            int[] idxA = new int[nSteps];
            int[] idxB = new int[nSteps];

            for (int s = 0; s < nSteps; s++)
            {
                float th = s * dTh;
                Vector3 vOffset = (fRadius * (float)Math.Cos(th) * vRight) + (fRadius * (float)Math.Sin(th) * vNormal);
                idxA[s] = mesh.nAddVertex(pA + vOffset);
                idxB[s] = mesh.nAddVertex(pB + vOffset);
            }

            for (int s = 0; s < nSteps; s++)
            {
                int sn = (s + 1) % nSteps;
                mesh.AddTriangle(idxA[s], idxB[s], idxB[sn]);
                mesh.AddTriangle(idxA[s], idxB[sn], idxA[sn]);
            }

            return mesh;
        }

        private static Mesh CreateTorusMeshAtPoint(Vector3 vCenter, Vector3 vNormalDir, float fMajorRadius, float fRingRadius, int nSteps = 20)
        {
            Mesh mesh = new Mesh();
            vNormalDir = Vector3.Normalize(vNormalDir);
            Vector3 vUp = Math.Abs(vNormalDir.Z) < 0.99f ? Vector3.UnitZ : Vector3.UnitX;
            Vector3 vRight = Vector3.Normalize(Vector3.Cross(vNormalDir, vUp));
            Vector3 vForward = Vector3.Normalize(Vector3.Cross(vRight, vNormalDir));

            float dPhi = 2.0f * (float)Math.PI / nSteps;
            float dTh = 2.0f * (float)Math.PI / 12;

            for (int i = 0; i < nSteps; i++)
            {
                float phi = i * dPhi;
                Vector3 vDirRing = (float)Math.Cos(phi) * vRight + (float)Math.Sin(phi) * vForward;
                Vector3 vRingCenter = vCenter + fMajorRadius * vDirRing;

                for (int j = 0; j < 12; j++)
                {
                    float th = j * dTh;
                    Vector3 vOffset = (fRingRadius * (float)Math.Cos(th) * vDirRing) + (fRingRadius * (float)Math.Sin(th) * vNormalDir);
                    mesh.nAddVertex(vRingCenter + vOffset);
                }
            }

            for (int i = 0; i < nSteps; i++)
            {
                int iNext = (i + 1) % nSteps;
                for (int j = 0; j < 12; j++)
                {
                    int jNext = (j + 1) % 12;
                    int i00 = i * 12 + j;
                    int i10 = iNext * 12 + j;
                    int i01 = i * 12 + jNext;
                    int i11 = iNext * 12 + jNext;

                    mesh.AddTriangle(i00, i10, i11);
                    mesh.AddTriangle(i00, i11, i01);
                }
            }

            return mesh;
        }
        #endregion
        #endregion