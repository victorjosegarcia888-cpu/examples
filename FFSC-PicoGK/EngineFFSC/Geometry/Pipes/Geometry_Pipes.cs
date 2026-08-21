// Geometry_Pipes.cs
//
// Geometria de tuberias de alta presion FFSC.
//
// Incluye:
// - Linea principal LOX
// - Linea principal CH4
// - Derivaciones laterales
// - Conexiones a turbobomba
//
// Teoria:
// - Diametro hidraulico: Dh = 4A/P
// - Perdida de carga: Darcy-Weisbach
// - Presion de operacion: 350 bar (LOX), 450 bar (CH4)

using PicoGK;

namespace FFSC_PicoGK.Geometry.Pipes
{
    /// <summary>
    /// Geometria de tuberias de alta presion FFSC.
    /// </summary>
    public static class Geometry_Pipes
    {
        /// <summary>
        /// Crea la red de tuberias principal.
        /// </summary>
        /// <param name="loxRadius">Radio de tuberia LOX [m]</param>
        /// <param name="ch4Radius">Radio de tuberia CH4 [m]</param>
        /// <param name="mainLength">Longitud tuberia principal [m]</param>
        /// <param name="branchLength">Longitud derivacion [m]</param>
        /// <returns>Field3D con la red de tuberias</returns>
        public static Field3D Create(
            double loxRadius = 0.03,
            double ch4Radius = 0.03,
            double mainLength = 0.80,
            double branchLength = 0.60)
        {
            // Tuberia principal LOX
            var loxMain = Field3D.Cylinder(loxRadius, mainLength)
                .Rotate(Math.PI / 2, 0, 0)
                .Translate(0.30, 0, 0);

            // Derivacion superior LOX
            var loxBranchTop = Field3D.Cylinder(loxRadius, branchLength)
                .Translate(0.30, 0.20, 0);

            // Derivacion inferior LOX
            var loxBranchBottom = Field3D.Cylinder(loxRadius, branchLength)
                .Translate(0.30, -0.20, 0);

            // Tuberia principal CH4
            var ch4Main = Field3D.Cylinder(ch4Radius, mainLength)
                .Rotate(Math.PI / 2, 0, 0)
                .Translate(-0.30, 0, 0);

            // Derivacion superior CH4
            var ch4BranchTop = Field3D.Cylinder(ch4Radius, branchLength)
                .Translate(-0.30, 0.20, 0);

            // Derivacion inferior CH4
            var ch4BranchBottom = Field3D.Cylinder(ch4Radius, branchLength)
                .Translate(-0.30, -0.20, 0);

            return Field3D.Combine(
                loxMain, loxBranchTop, loxBranchBottom,
                ch4Main, ch4BranchTop, ch4BranchBottom
            );
        }
    }
}
