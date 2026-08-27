using PicoGK;
using LEAP71.ShapeKernel;
using LEAP71.LatticeLibrary;

namespace RocketEngineDesign
{
    public static partial class EngineTasks
    {
        /// <summary>
        /// Genera una pared externa aligerada mediante estructuras reticulares TPMS (Gyroid)
        /// </summary>
        public static Voxels Task_GenerateLatticeReinforcedWall(
            Voxels voxOuterShell,
            float fLatticeCellSize = 4.0f,
            float fThickness = 1.2f)
        {
            // Definir campo de retícula periódica usando LatticeLibrary
            LatticeLatticePattern gyroidPattern = new GyroidPattern(fLatticeCellSize);
            
            // Generar vóxeles del entramado reticular dentro del dominio del cuerpo
            Voxels voxLattice = LatticeGenerator.VoxCreateLattice(
                gyroidPattern, 
                voxOuterShell.BBox(), 
                fThickness
            );

            // Intersección booleana para limitar la retícula al volumen de la pared
            voxLattice.Intersect(voxOuterShell);

            return voxLattice;
        }
    }
}