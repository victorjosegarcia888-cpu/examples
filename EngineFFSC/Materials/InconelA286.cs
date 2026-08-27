// InconelA286.cs
//
// Material Inconel A286 para componentes de motor FFSC.
// Propiedades tipicas a temperatura ambiente y operacion.

using System;

namespace EngineFFSC.Materials
{
    public class InconelA286
    {
        public string Name { get; set; } = "Inconel_A286";
        public double Density_kg_m3 { get; set; } = 7870.0;
        public double YieldStrength_Pa { get; set; } = 690e6;
        public double TensileStrength_Pa { get; set; } = 930e6;
        public double YoungsModulus_Pa { get; set; } = 201e9;
        public double PoissonRatio { get; set; } = 0.3;
        public double ThermalConductivity_W_mK { get; set; } = 16.5;
        public double SpecificHeat_J_kgK { get; set; } = 450.0;
        public double ThermalExpansion_1_K { get; set; } = 11.5e-6;
        public double MaxServiceTemp_C { get; set; } = 650.0;
        public double MeltingPoint_C { get; set; } = 1390.0;
        public double Hardness_HB { get; set; } = 248.0;
        public double FatigueStrength_Pa { get; set; } = 345e6;
        public double ElongationPercent { get; set; } = 25.0;
        public double ReductionOfAreaPercent { get; set; } = 40.0;
    }
}
