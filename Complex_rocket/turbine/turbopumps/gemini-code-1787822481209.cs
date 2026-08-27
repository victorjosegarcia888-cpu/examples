using System;
using System.IO;
using PikoGK;

namespace RocketEngineDesign
{
    public static class InjectorExporterAndSmoother
    {
        /// <summary>
        /// Aplica filtros de suavizado morfológico a la geometría de vóxeles,
        /// extrae la malla iso-superficial y exporta a archivos STL y 3MF.
        /// </summary>
        /// <param name="voxInput">Geometría en vóxeles del inyector completo.</param>
        /// <param name="strOutputFolder">Directorio de destino para los archivos.</param>
        /// <param name="strBaseFileName">Nombre base para los archivos exportados.</param>
        /// <param name="fBlurRadiusMm">Radio del filtro de suavizado Gaussiano (mm).</param>
        /// <param name="fFilletRadiusMm">Radio de redondeo de aristas cóncavas/convexas (mm).</param>
        public static void ProcessAndExportInjector(
            Voxels voxInput,
            string strOutputFolder = "./Output",
            string strBaseFileName = "HotGas_Injector_Assembly",
            float fBlurRadiusMm = 0.4f,
            float fFilletRadiusMm = 0.8f)
        {
            Console.WriteLine("[PikoGK] Iniciando procesamiento de suavizado morfológico...");

            // 1. Crear una copia de los vóxeles para no alterar la fuente original
            Voxels voxSmoothed = new Voxels(voxInput);

            // 2. Redondeo de bordes cóncavos/convexos mediante OverOffset Morfológico
            // Un OverOffset hacia afuera (+) seguido de un Offset hacia adentro (-) redondea aristas cóncavas (Filleting)
            if (fFilletRadiusMm > 0.0f)
            {
                voxSmoothed.DoOffset(fFilletRadiusMm);
                voxSmoothed.DoOffset(-fFilletRadiusMm);

                // Un OverOffset hacia adentro (-) seguido de uno hacia afuera (+) redondea aristas convexas
                voxSmoothed.DoOffset(-fFilletRadiusMm * 0.5f);
                voxSmoothed.DoOffset(fFilletRadiusMm * 0.5f);
            }

            // 3. Aplicar Filtro Gaussiano para suavizado de alta frecuencia a nivel sub-voxel
            if (fBlurRadiusMm > 0.0f)
            {
                voxSmoothed.DoGaussianBlur(fBlurRadiusMm);
            }

            Console.WriteLine("[PikoGK] Extrayendo malla iso-superficial desde el campo de vóxeles...");

            // 4. Conversión del espacio de vóxeles a Malla Poligonal (Mesh triangulation)
            Mesh meshFinal = new Mesh(voxSmoothed);

            // Asegurar que el directorio de salida existe
            if (!Directory.Exists(strOutputFolder))
            {
                Directory.CreateDirectory(strOutputFolder);
            }

            string strStlFilePath = Path.Combine(strOutputFolder, $"{strBaseFileName}.stl");
            string str3mfFilePath = Path.Combine(strOutputFolder, $"{strBaseFileName}.3mf");

            // 5. Exportación a archivos STL y 3MF
            Console.WriteLine($"[PikoGK] Exportando archivo STL: {strStlFilePath}");
            Library.ExportMeshToStl(meshFinal, strStlFilePath);

            Console.WriteLine($"[PikoGK] Exportando archivo 3MF: {str3mfFilePath}");
            Library.ExportMeshTo3MF(meshFinal, str3mfFilePath);

            Console.WriteLine("[PikoGK] Proceso de exportación finalizado exitosamente.");
        }

        /// <summary>
        /// Punto de entrada del pipeline de generación y exportación.
        /// </summary>
        public static void RunExportPipeline()
        {
            // Inicializar PikoGK con resolución de vóxel de 0.25 mm
            float fVoxelSizeMm = 0.25f;
            
            Library.Go(fVoxelSizeMm, () =>
            {
                // Generar la geometría completa ensamblada
                Voxels voxAssembly = FullInjectorAssemblyGenerator.voxCreateFullInjectorAssembly();

                // Procesar filtrado y exportar
                ProcessAndExportInjector(
                    voxInput: voxAssembly,
                    strOutputFolder: "./ExportedModels",
                    strBaseFileName: "RaptorStyle_HotGas_Injector",
                    fBlurRadiusMm: 0.35f,
                    fFilletRadiusMm: 0.75f
                );
            });
        }
    }
}