//
// SPDX-License-Identifier: CC0-1.0
//
// Program.cs - Motor Hibrido Regenerativo
// Orquesta el flujo de modelado volumetrico con PicoGK.
// Genera STL y exporta usando el viewer oficial.
//
// Arquitectura basada en instrucciones de notas01.txt:
// - Ensamblaje de todas las piezas del motor hibrido
// - Filtros morfologicos: DoOffset(+0.003), DoOffset(-0.003), DoGaussianBlur(1.0)
// - Exportacion STL lista para impresion 3D
//

using PicoGK;
using AseRocketEngine.Tasks;
using System.Numerics;

Library.Go(0.25f, () =>
{
    // 1. Generar componentes del motor hibrido
    Voxels impeller = Task_GenerateFuelPumpImpeller.Build(
        new Vector3(-250, 0, 0));

    Voxels turbine = Task_GenerateVectorTurbine.Build(
        new Vector3(-250, 150, 0));

    Voxels preburner = Task_GeneratePreburnerToroid.Build(
        new Vector3(0, 0, 0));

    Voxels chamber = Task_GenerateMainChamberHybrid.Build(
        new Vector3(200, 0, 0));

    Voxels lattice = Task_GenerateAperiodicLattice.Build(
        new Vector3(200, 0, 0));

    // Canales regenerativos aplicados a la camara
    Voxels cooledChamber = Task_GenerateCoolingChannels.Build(chamber);

    // Ductos HP
    Voxels ducts = Task_GenerateHighPressureDucts.Build(
        new Vector3(0, 0, 0),
        new Vector3(-200, 0, 0),
        new Vector3(-200, 50, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, 50, 0));

    // 2. Ensamblar motor completo
    Voxels assembly = new Voxels();
    assembly += impeller;
    assembly += turbine;
    assembly += preburner;
    assembly = assembly - chamber + cooledChamber;
    assembly += lattice;
    assembly += ducts;

    // 3. Filtros morfologicos (DoOffset + GaussianBlur)
    float offsetRadius = 0.003f;
    float blurSigma = 1.0f;

    assembly.voxOffset(offsetRadius);
    assembly.voxOffset(-offsetRadius);
    assembly.Gaussian(blurSigma);

    // 4. Extraer malla
    Mesh mesh = new Mesh(assembly);

    // 5. Exportar STL
    string outputDir = "./ExportedModels";
    System.IO.Directory.CreateDirectory(outputDir);
    mesh.SaveToStlFile(
        System.IO.Path.Combine(outputDir, "HybridRocketEngine.stl"));

    // 6. Visualizar en viewer oficial
    Library.oViewer().Add(assembly);
});
