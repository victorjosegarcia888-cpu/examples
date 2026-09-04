namespace RocketEngineDesign
{
    /// <summary>
    /// Parámetros de diseño del motor Noyron TKL-200.
    /// </summary>
    public static class TechnicalSpecs
    {
        // Resolución del kernel
        public const float VoxelSizeMm = 0.2f;

        // Dimensiones Principales de la Cámara y Tobera (mm)
        public const float TotalHeightMm = 220.0f;
        public const float ThroatZMm = 90.0f;
        public const float ChamberRadiusMm = 65.0f;
        public const float ThroatRadiusMm = 30.0f;
        public const float ExitRadiusMm = 95.0f;
        public const float LinerWallThicknessMm = 8.0f;

        // Canales Helicoidales (HelixHeatX)
        public const int ChannelCount = 72;
        public const float ChannelWidthMm = 2.2f;
        public const float ChannelDepthMm = 3.5f;
        public const float HelixTurns = 1.25f;

        // Colectores Toroidales
        public const float ManifoldPipeOuterRadiusMm = 7.0f;
        public const float ManifoldWallThicknessMm = 2.0f;
        public const int FeedPortCount = 2;
        public const float FeedPortLengthMm = 30.0f;
    }
}