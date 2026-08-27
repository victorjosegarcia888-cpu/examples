Console.WriteLine("[HelixHeatX] Generando canales de refrigeración helicoidales entrelazados...");
Voxels voxHelixCoolant = EngineTasks.Task_GenerateAdvancedHelixHeatXChannels(
    fChamberRadiusMm: 70.0f,
    fThroatRadiusMm:  32.0f,
    fExitRadiusMm:    110.0f,
    fTotalLengthMm:   220.0f,
    nHelixTurns:      3,        // N° de vueltas alrededor de la tobera
    nFluidChannels:   80,       // Canales helicoidales entrelazados
    fChannelDepthMm:  3.5f,     // Profundidad de entrada del refrigerante
    fFinThicknessMm:  0.5f      // Espesor de aletas micro-estructurales
);

// Operación Booleana Implícita de PicoGK: Sustraer el volumen de fluido helicoidal
Console.WriteLine("[Assembly] Sustrayendo volumen de fluido HelixHeatX de la liner...");
voxNozzle.Subtract(voxHelixCoolant);