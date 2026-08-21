// VdbExporter.cs
//
// Exportador NanoVDB/OpenVDB para geometrias FFSC.
// Permite exportar Field3D a formato estandar para
// visualizacion externa (Blender, Houdini, etc.).
//
// Cita del PDF:
// "NanoVDB es un formato compacto para datos volumetricos
//  voxelizados, ideal para visualizacion cientifica."

using System.IO;
using PicoGK;

namespace FFSC_PicoGK.Models
{
    /// <summary>
    /// Exportador de campos volumetricos a VDB.
    /// </summary>
    public static class VdbExporter
    {
        /// <summary>
        /// Exporta un Field3D a archivo VDB.
        /// </summary>
        public static void Export(Field3D geom, string ruta)
        {
            string dir = Path.GetDirectoryName(ruta);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using var fs = new FileStream(ruta, FileMode.Create);
            geom.ExportVDB(fs);
        }

        /// <summary>
        /// Exporta un Field3D a OBJ para visualizacion de superficie.
        /// </summary>
        public static void ExportOBJ(Field3D geom, string ruta)
        {
            string dir = Path.GetDirectoryName(ruta);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using var writer = new StreamWriter(ruta);
            geom.ExportOBJ(writer);
        }
    }
}
