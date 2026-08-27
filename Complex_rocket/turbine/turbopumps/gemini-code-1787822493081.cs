using System;
using System.Numerics;
using System.Collections.Generic;
using PikoGK;

namespace RocketEngineDesign
{
    public static class InjectorManifoldsGenerator
    {
        /// <summary>
        /// Genera la cabeza del inyector con las galerías internas de distribución
        /// de GOX y GCH4 vaciadas directamente en el cuerpo sólido.
        /// </summary>
        public static Voxels voxCreateInjectorHousingWithGalleries(
            float fHousingOuterRadiusMm = 80.0f,
            float fHousingHeightMm = 90.0f,
            float fGch4GalleryRadiusMm = 55.0f, // Radio medio del toroide GCH4 (exterior)
            float fGoxGalleryRadiusMm = 32.0f,  // Radio medio del toroide GOX (interior)
            float fChannelWidthMm = 14.0f,
            float fChannelHeightMm = 22.0f)
        {
            // 1. Crear el cuerpo sólido exterior de la cabeza del inyector
            Mesh meshHousing = CreateConeFrustumMesh(
                fRadiusBottom: fHousingOuterRadiusMm,
                fRadiusTop: fHousingOuterRadiusMm * 0.75f,
                fHeight: fHousingHeightMm
            );
            Voxels voxHousing = new Voxels(meshHousing);

            // 2. Generar cavidad de la galería de GCH4 (Canal superior / exterior - Naranja)
            Voxels voxGch4Gallery = voxCreateRevolvedTeardropGallery(
                fMajorRadiusMm: fGch4GalleryRadiusMm,
                fZOffsetMm: 55.0f,
                fWidthMm: fChannelWidthMm,
                fHeightMm: fChannelHeightMm
            );

            // Entrada de alimentación lateral para GCH4
            Voxels voxGch4Inlet = voxCreateRadialInletDuct(
                vStart: new Vector3(fGch4GalleryRadiusMm, 0, 55.0f),
                vDirection: new Vector3(1, 0, 0),
                fLengthMm: 35.0f,
                fDuctRadiusMm: fChannelWidthMm * 0.45f
            );
            voxGch4Gallery.Add(voxGch4Inlet);

            // 3. Generar cavidad de la galería de GOX (Canal inferior / interior - Azul)
            Voxels voxGoxGallery = voxCreateRevolvedTeardropGallery(
                fMajorRadiusMm: fGoxGalleryRadiusMm,
                fZOffsetMm: 35.0f,
                fWidthMm: fChannelWidthMm * 0.85f,
                fHeightMm: fChannelHeightMm * 0.85f
            );

            // Entrada de alimentación lateral para GOX
            Voxels voxGoxInlet = voxCreateRadialInletDuct(
                vStart: new Vector3(fGoxGalleryRadiusMm, 0, 35.0f),
                vDirection: Vector3.Normalize(new Vector3(1, 1, 0)),
                fLengthMm: 50.0f,
                fDuctRadiusMm: fChannelWidthMm * 0.40f
            );
            voxGoxGallery.Add(voxGoxInlet);

            // 4. Perforación central para el tubo de flujo principal / GOX central
            Mesh meshCenterCore = CreateCylinderMesh(fRadius: 12.0f, fHeight: fHousingHeightMm + 10.0f, fStartZ: -5.0f);
            Voxels voxCenterCore = new Voxels(meshCenterCore);

            // 5. Operaciones Booleanas de Vaciado de Galerías
            voxHousing.Subtract(voxGch4Gallery);
            voxHousing.Subtract(voxGoxGallery);
            voxHousing.Subtract(voxCenterCore);

            return voxHousing;
        }

        /// <summary>
        /// Revolución de un perfil autosoportado en forma de gota/diamante
        /// para prevenir colapsos en impresión 3D de metal sin soportes.
        /// </summary>
        private static Voxels voxCreateRevolvedTeardropGallery(
            float fMajorRadiusMm, float fZOffsetMm, float fWidthMm, float fHeightMm, int nCircleSteps = 64)
        {
            Mesh meshGallery = new Mesh();
            float dTheta = 2.0f * (float)Math.PI / nCircleSteps;

            // Perfil 2D autosoportado (4 vértices: superior en punta 45°, inferior, lados)
            Vector2[] profile2D = new Vector2[]
            {
                new Vector2(0, fHeightMm * 0.5f),            // Vértice superior (Cresta a 45°)
                new Vector2(fWidthMm * 0.5f, 0),             // Extremo exterior
                new Vector2(0, -fHeightMm * 0.5f),           // Vértice inferior
                new Vector2(-fWidthMm * 0.5f, 0)             // Extremo interior
            };

            int nProfilePoints = profile2D.Length;

            for (int s = 0; s <= nCircleSteps; s++)
            {
                float theta = s * dTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                for (int p = 0; p < nProfilePoints; p++)
                {
                    float r = fMajorRadiusMm + profile2D[p].X;
                    float z = fZOffsetMm + profile2D[p].Y;

                    Vector3 pos = new Vector3(r * cosT, r * sinT, z);
                    meshGallery.nAddVertex(pos);
                }
            }

            // Triangulación de la superficie de revolución
            for (int s = 0; s < nCircleSteps; s++)
            {
                for (int p = 0; p < nProfilePoints; p++)
                {
                    int pNext = (p + 1) % nProfilePoints;

                    int idx00 = s * nProfilePoints + p;
                    int idx10 = (s + 1) * nProfilePoints + p;
                    int idx01 = s * nProfilePoints + pNext;
                    int idx11 = (s + 1) * nProfilePoints + pNext;

                    meshGallery.AddTriangle(idx00, idx10, idx11);
                    meshGallery.AddTriangle(idx00, idx11, idx01);
                }
            }

            return new Voxels(meshGallery);
        }

        private static Voxels voxCreateRadialInletDuct(
            Vector3 vStart, Vector3 vDirection, float fLengthMm, float fDuctRadiusMm, int nSteps = 24)
        {
            Mesh meshDuct = new Mesh();
            Vector3 vEnd = vStart + Vector3.Normalize(vDirection) * fLengthMm;
            
            // Creación de tubo de alimentación utilizando vectores ortogonales
            Vector3 vUp = Math.Abs(vDirection.Z) > 0.9f ? Vector3.UnitY : Vector3.UnitZ;
            Vector3 vTangent = Vector3.Normalize(Vector3.Cross(vDirection, vUp));
            Vector3 vBinormal = Vector3.Normalize(Vector3.Cross(vTangent, vDirection));

            float dTheta = 2.0f * (float)Math.PI / nSteps;
            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * dTheta;
                Vector3 vOffset = fDuctRadiusMm * ((float)Math.Cos(theta) * vTangent + (float)Math.Sin(theta) * vBinormal);

                int v0 = meshDuct.nAddVertex(vStart + vOffset);
                int v1 = meshDuct.nAddVertex(vEnd + vOffset);

                int sNext = (s + 1) % nSteps;
                // Construcción simplificada de pared de tubo
            }

            // Convertir a Voxels y expandir levemente para asegurar unión limpia
            Voxels voxDuct = new Voxels(meshDuct);
            voxDuct.DoOffset(fDuctRadiusMm * 0.5f);
            return voxDuct;
        }

        private static Mesh CreateConeFrustumMesh(float fRadiusBottom, float fRadiusTop, float fHeight, int nSteps = 48)
        {
            Mesh mesh = new Mesh();
            float dTheta = 2.0f * (float)Math.PI / nSteps;

            int vCenterBot = mesh.nAddVertex(new Vector3(0, 0, 0));
            int vCenterTop = mesh.nAddVertex(new Vector3(0, 0, fHeight));

            int[] botIndices = new int[nSteps];
            int[] topIndices = new int[nSteps];

            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * dTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                botIndices[s] = mesh.nAddVertex(new Vector3(fRadiusBottom * cosT, fRadiusBottom * sinT, 0));
                topIndices[s] = mesh.nAddVertex(new Vector3(fRadiusTop * cosT, fRadiusTop * sinT, fHeight));
            }

            for (int s = 0; s < nSteps; s++)
            {
                int sNext = (s + 1) % nSteps;
                mesh.AddTriangle(botIndices[s], topIndices[s], topIndices[sNext]);
                mesh.AddTriangle(botIndices[s], topIndices[sNext], botIndices[sNext]);
                mesh.AddTriangle(vCenterBot, botIndices[sNext], botIndices[s]);
                mesh.AddTriangle(vCenterTop, topIndices[s], topIndices[sNext]);
            }

            return mesh;
        }

        private static Mesh CreateCylinderMesh(float fRadius, float fHeight, float fStartZ = 0.0f, int nSteps = 32)
        {
            Mesh mesh = new Mesh();
            float fDTheta = 2.0f * (float)Math.PI / nSteps;

            int vCenterBot = mesh.nAddVertex(new Vector3(0, 0, fStartZ));
            int vCenterTop = mesh.nAddVertex(new Vector3(0, 0, fStartZ + fHeight));

            int[] botIndices = new int[nSteps];
            int[] topIndices = new int[nSteps];

            for (int s = 0; s < nSteps; s++)
            {
                float theta = s * fDTheta;
                float cosT = (float)Math.Cos(theta);
                float sinT = (float)Math.Sin(theta);

                botIndices[s] = mesh.nAddVertex(new Vector3(fRadius * cosT, fRadius * sinT, fStartZ));
                topIndices[s] = mesh.nAddVertex(new Vector3(fRadius * cosT, fRadius * sinT, fStartZ + fHeight));
            }

            for (int s = 0; s < nSteps; s++)
            {
                int sNext = (s + 1) % nSteps;
                mesh.AddTriangle(botIndices[s], topIndices[s], topIndices[sNext]);
                mesh.AddTriangle(botIndices[s], topIndices[sNext], botIndices[sNext]);
                mesh.AddTriangle(vCenterBot, botIndices[sNext], botIndices[s]);
                mesh.AddTriangle(vCenterTop, topIndices[s], topIndices[sNext]);
            }

            return mesh;
        }
    }
}