// Geometry_Chamber.cs
//
// Geometria completa de la camara de combustion FFSC.
//
// Incluye:
// - Seccion esferica (domo inyector)
// - Seccion cilindrica
// - Seccion convergente (garganta)
// - Contraction ratio optimizada
//
// Teoria:
// - L* = Vc / At (longitud caracteristica)
// - Vc = volumen camara
// - Ac = At * CR (ratio de contraccion)
// - De Laval nozzle: Mach = 1 en garganta
//
// Cita PDF:
// "L* tipicamente entre 0.8m y 2.0m para combustion LOX/CH4.
//  Ratio de contraccion optimo: 4-8."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Chamber
{
    /// <summary>
    /// Geometria de camara de combustion FFSC completa.
    /// </summary>
    public static class Geometry_Chamber
    {
        /// <summary>
        /// Crea la camara de combustion completa.
        /// </summary>
        /// <param name="p">Parametros del motor</param>
        /// <returns>Field3D con la geometria de la camara</returns>
        public static Field3D Create(FFSC_PicoGK.Models.EngineParams p)
        {
            double Rc = p.ChamberRadius;
            double Lc = p.ChamberLength;
            double Rt = p.ThroatRadius;
            double At = p.At;
            double Lstar = p.Lstar;
            double CR = p.ContractionRatio;

            // Volumen requerido de camara Vc = L* * At
            double Vc = Lstar * At;
            // Area de seccion transversal de camara
            double Ac = CR * At;
            double Rchamber = Math.Sqrt(Ac / Math.PI);

            // Geometria: esfera + cilindro + cono convergente
            var domo = Field3D.Sphere(Rchamber * 0.95).Translate(0, 0, Lc * 0.5);
            var cilindro = Field3D.Cylinder(Rchamber, Lc * 0.5).Translate(0, 0, Lc * 0.75);
            var convergente = Field3D.Cone(Rchamber, Rt, Lc * 0.5).Translate(0, 0, Lc);

            var chamber = Field3D.Combine(domo, cilindro, convergente);

            return chamber;
        }

        /// <summary>
        /// Crea la camara con parametros directos.
        /// </summary>
        public static Field3D Create(
            double chamberRadius = 0.35,
            double chamberLength = 0.50,
            double throatRadius = 0.12,
            double lstar = 1.2,
            double contractionRatio = 6.0)
        {
            var p = new FFSC_PicoGK.Models.EngineParams
            {
                ChamberRadius = chamberRadius,
                ChamberLength = chamberLength,
                ThroatRadius = throatRadius,
                Lstar = lstar,
                ContractionRatio = contractionRatio
            };
            return Create(p);
        }
    }
}
