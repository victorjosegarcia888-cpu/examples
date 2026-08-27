using System;
using System.IO;
using PikoGK;

namespace RocketEngineDesign
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Inicialización de PikoGK con tamaño de vóxel de 0.5 mm
            Library.Init(0.5f);

            string outputPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                "ASE_Preburner_Assembly.stl"
            );

            Console.WriteLine("Generando ensamblaje completo del prequemador...");
            ExportPreburnerAssemblyToStl(outputPath);

            Console.WriteLine($"Ensamblaje exportado exitosamente en: {outputPath}");
        }

        public static void ExportPreburnerAssemblyToStl(string filePath)
        {
            // Parámetros de interfaz compartidos
            float fPlateRadiusMm = 80.0f;
            float fPlateThicknessMm = 12.0f;
            float fInjectorHoleRadiusMm = 6.5f;
            float fRingRadiusMm = 48.0f;
            float fFaceplateZ = 0.0f;

            // 1. Generación de la estructura principal (Cámara + Ducto + Manifold)
            Voxels voxPreburner = PreburnerGenerator.voxCreateASEPreburner(
                fHousingRadiusMm: 85.0f,
                fWallThicknessMm: 5.0f,
                fChamberLengthMm: 90.0f,
                fDuctLengthMm: 120.0f,
                fDuctExitRadiusMm: 45.0f,
                fManifoldMajorRadiusMm: 95.0f,
                fManifoldMinorRadiusMm: 22.0f
            );

            // 2. Generación de la placa inyectora con perforaciones
            Voxels voxFaceplate = InjectorPlateGenerator.voxCreateFaceplateWithInjectors(
                fPlateRadiusMm,
                fPlateThicknessMm,
                fInjectorHoleRadiusMm,
                fRingRadiusMm,
                fFaceplateZ
            );

            // 3. Generación del arreglo de 13 inyectores coaxiales
            Voxels voxInjectors = CoaxialInjectorGenerator.voxCreate13CoaxialInjectors(
                fInnerPostInnerRadiusMm: 2.5f,
                fInnerPostOuterRadiusMm: 3.5f,
                fOuterSleeveOuterRadiusMm: 6.4f,
                fInjectorLengthMm: 35.0f,
                fRingRadiusMm: fRingRadiusMm,
                fCenterZ: fFaceplateZ - 5.0f
            );

            // 4. Fusión CSG Voxelizada de todos los componentes
            Voxels voxAssembly = new Voxels();
            voxAssembly.Add(voxPreburner);
            voxAssembly.Add(voxFaceplate);
            voxAssembly.Add(voxInjectors);

            // 5. Mapeo a malla triangular y exportación a STL
            Mesh meshAssembly = new Mesh(voxAssembly);
            meshAssembly.SaveToStl(filePath);
        }
    }
}