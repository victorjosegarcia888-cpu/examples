//
// SPDX-License-Identifier: CC0-1.0
//
// Program.cs - Motor ASE/Ursa Mayor
// Orquesta el flujo de modelado volumetrico con PicoGK.
// Genera STL y exporta usando el viewer oficial.
//

using PicoGK;
using AseRocketEngine.Tasks;
using System.Numerics;

Library.Go(0.25f, () =>
{
    // 1. Generar componentes
    Voxels impeller = Task_GenerateFuelPumpImpeller.Build();
    Voxels turbineRotor = Task_GenerateTurbineRotor.Build();
    Voxels preburner = Task_GeneratePreburnerAssembly.Build();
    Voxels chamber = Task_GenerateMainChamberAndNozzle.Build();
    Voxels ducts = Task_GenerateHighPressureDucts.Build(
        new Vector3(-200, 0, 0),
        new Vector3(-200, 50, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, 50, 0));
    Voxels cooledChamber = Task_GenerateCoolingChannels.Build(chamber);

    // 2. Ensamblar
    Voxels assembly = new Voxels();
    assembly += impeller + new Vector3(-250, 0, 0);
    assembly += turbineRotor + new Vector3(-250, 150, 0);
    assembly += preburner + new Vector3(0, 0, 0);
    assembly += cooledChamber + new Vector3(200, 0, 0);
    assembly += ducts;

    // 3. Filtros morfologicos
    float filletRadius = 1.2f;
    float blurSigma = 0.9f;

    assembly.Offset(filletRadius);
    assembly.Offset(-filletRadius);
    assembly.Gaussian(blurSigma);

    // 4. Extraer malla
    Mesh mesh = assembly.mshAsMesh();

    // 5. Exportar STL
    string outputDir = "./ExportedModels";
    System.IO.Directory.CreateDirectory(outputDir);
    mesh.SaveToStlFile(
        System.IO.Path.Combine(outputDir, "ASE_RocketEngine.stl"),
        PicoGK.EStlUnit.Millimeter,
        null,
        0.0f);

    // 6. Visualizar en viewer oficial
    Library.oViewer().Add(assembly);
});
