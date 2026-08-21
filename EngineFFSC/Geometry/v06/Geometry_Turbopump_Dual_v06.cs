// Geometry_Turbopump_Dual_v06.cs
//
// Geometria del turbobomba dual LOX/CH4 FFSC v06.
// Usando PicoGK Voxels API: voxSphere para rotores, eje y volutas.
//
// Teoria:
// - Ecuacion de Euler en turbomaquinas: Delta h = U * Delta Vtheta
// - Triangulos de velocidades (absoluta, relativa, tangencial)
// - Diseno de voluta dual (dual volute) para LOX y CH4
// - Eje comun (common shaft) con rodamientos

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Turbopump_Dual_v06
    {
        public static Voxels Create(double rotorRadius = 0.16, int bladeCount = 10, double bladeChord = 0.04, double bladeHeight = 0.06)
        {
            Voxels turbopump = new Voxels();

            float Rrotor = (float)rotorRadius;
            float chord = (float)bladeChord;
            float height = (float)bladeHeight;
            float hubRadius = Rrotor * 0.3f;

            // Rotor LOX (parte superior del eje)
            float loxCenter = height * 2.0f;
            
            // Disco del rotor LOX
            turbopump += Voxels.voxSphere(new Vector3(0, 0, loxCenter), Rrotor);
            
            // Cubo del rotor LOX
            turbopump += Voxels.voxSphere(new Vector3(0, 0, loxCenter), hubRadius);

            // Palas del rotor LOX
            for (int i = 0; i < bladeCount; i++)
            {
                float angle = (float)(i * 2.0 * Math.PI / bladeCount);
                float px = Rrotor * 0.7f * (float)Math.Cos(angle);
                float py = Rrotor * 0.7f * (float)Math.Sin(angle);
                
                turbopump += Voxels.voxSphere(new Vector3(px, py, loxCenter), chord * 0.6f);
            }

            // Eje comun
            float shaftLength = height * 5.0f;
            float shaftRadius = hubRadius * 0.8f;
            float shaftCenter = loxCenter - shaftLength * 0.5f;
            
            for (float z = shaftCenter - shaftLength * 0.5f; z <= shaftCenter + shaftLength * 0.5f; z += shaftRadius * 0.5f)
            {
                turbopump += Voxels.voxSphere(new Vector3(0, 0, z), shaftRadius);
            }

            // Rotor CH4 (parte inferior del eje)
            float ch4Center = loxCenter - shaftLength;
            
            // Disco del rotor CH4
            turbopump += Voxels.voxSphere(new Vector3(0, 0, ch4Center), Rrotor);
            
            // Cubo del rotor CH4
            turbopump += Voxels.voxSphere(new Vector3(0, 0, ch4Center), hubRadius);

            // Palas del rotor CH4
            for (int i = 0; i < bladeCount; i++)
            {
                float angle = (float)(i * 2.0 * Math.PI / bladeCount + Math.PI / bladeCount);
                float px = Rrotor * 0.7f * (float)Math.Cos(angle);
                float py = Rrotor * 0.7f * (float)Math.Sin(angle);
                
                turbopump += Voxels.voxSphere(new Vector3(px, py, ch4Center), chord * 0.6f);
            }

            // Voluta dual LOX ( Housing )
            float voluteRadius = Rrotor * 1.8f;
            for (int i = 0; i < 36; i++)
            {
                float theta = (float)(i * 2.0 * Math.PI / 36);
                float vx = voluteRadius * (float)Math.Cos(theta);
                float vy = voluteRadius * (float)Math.Sin(theta);
                
                turbopump += Voxels.voxSphere(new Vector3(vx, vy, loxCenter), chord * 0.8f);
            }

            // Voluta dual CH4
            for (int i = 0; i < 36; i++)
            {
                float theta = (float)(i * 2.0 * Math.PI / 36);
                float vx = voluteRadius * (float)Math.Cos(theta);
                float vy = voluteRadius * (float)Math.Sin(theta);
                
                turbopump += Voxels.voxSphere(new Vector3(vx, vy, ch4Center), chord * 0.8f);
            }

            return turbopump;
        }
    }
}
