using System;
using System.IO;
using PikoGK;

namespace RocketEngineDesign
{
    public static class SmoothingUtils
    {
        /// <summary>
        /// Aplica redondeos (fillets) y suavizado volumétrico al ensamblaje 
        /// para eliminar cantos vivos y reducir el factor de intensidad de esfuerzo (Kt).
        /// </summary>
        /// <param name="voxInput">Campo de vóxeles del ensamblaje rígido.</param>
        /// <param name="fFilletRadiusMm">Radio aproximado del redondeo (fillet) en mm.</param>
        /// <param name="fBlurSigmaMm">Intensidad del filtro gaussiano (típicamente 70-80% del radio).</param>
        /// <returns>Objeto Voxels con uniones suavizadas de curvatura continua.</returns>
        public static Voxels voxApplyStressReliefFillets(
            Voxels voxInput, 
            float fFilletRadiusMm = 1.5f,
            float fBlurSigmaMm = 1.0f)
        {
            // Duplicar el volumen original
            Voxels voxSmoothed = new Voxels(voxInput);

            // 1. DILATACIÓN (Offset Positivo):
            // Expande la superficie hacia afuera para que los rincones internos afilados
            // se desplacen a una zona accesible para el filtro espacial.
            voxSmoothed.DoOffset(fFilletRadiusMm);

            // 2. FILTRO GAUSSIANO (Blur):
            // Redondea las esquinas afiladas suavizando el campo de distancias implícito.
            voxSmoothed.DoGaussian(fBlurSigmaMm);

            // 3. EROSIÓN COMPENSATORIA (Offset Negativo):
            // Devuelve las paredes exteriores a su cota nominal, manteniendo 
            // los empalmes redondeados (fillets) en las raíces internas.
            voxSmoothed.DoOffset(-fFilletRadiusMm);

            return voxSmoothed;
        }
    }

    public class ProgramWithSmoothing
    {
        public static void Main(string[] args)
        {
            Library.Init(0.4f); // Mayor resolución (0.4mm) para capturar fillets finos

            string outputPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                "ASE_Preburner_Smoothed_Assembly.stl"
            );

            Console.WriteLine("Generando ensamblaje del prequemador...");
            
            // 1. Construir las piezas rígidas unidas en un solo objeto Voxels
            Voxels voxPreburner = PreburnerGenerator.voxCreateASEPreburner();
            Voxels voxFaceplate = InjectorPlateGenerator.voxCreateFaceplateWithInjectors();
            Voxels voxInjectors = CoaxialInjectorGenerator.voxCreate13CoaxialInjectors();

            Voxels voxRawAssembly = new Voxels();
            voxRawAssembly.Add(voxPreburner);
            voxRawAssembly.Add(voxFaceplate);
            voxRawAssembly.Add(voxInjectors);

            Console.WriteLine("Aplicando redondeos volumétricos (Stress-Relief Fillets)...");
            
            // 2. Aplicar el suavizado de uniones
            Voxels voxFinalAssembly = SmoothingUtils.voxApplyStressReliefFillets(
                voxRawAssembly, 
                fFilletRadiusMm: 1.2f, 
                fBlurSigmaMm: 0.9f
            );

            // 3. Exportar a STL
            Mesh meshFinal = new Mesh(voxFinalAssembly);
            meshFinal.SaveToStl(outputPath);

            Console.WriteLine($"Malla suavizada exportada en: {outputPath}");
        }
    }
}