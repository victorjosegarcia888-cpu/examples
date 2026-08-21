// VdbExporter.cs
//
// Exportador NanoVDB/OpenVDB para geometrias FFSC.

using System.IO;
using PicoGK;

namespace FFSC_PicoGK.Models
{
    public static class VdbExporter
    {
        public static void Export(Voxels geom, string ruta)
        {
            string? dir = Path.GetDirectoryName(ruta);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            geom.SaveToVdbFile(ruta);
        }
    }
}
