// AlloyDatabase.cs
//
// Base de datos de aleaciones para el motor FFSC.

using System.Collections.Generic;

namespace EngineFFSC.Materials
{
    public static class AlloyDatabase
    {
        public static Dictionary<string, InconelA286> GetAlloys()
        {
            return new Dictionary<string, InconelA286>
            {
                ["Inconel_A286"] = new InconelA286
                {
                    Name = "Inconel_A286",
                    Density_kg_m3 = 7870.0,
                    YieldStrength_Pa = 690e6,
                    TensileStrength_Pa = 930e6,
                    YoungsModulus_Pa = 201e9,
                    PoissonRatio = 0.3,
                    ThermalConductivity_W_mK = 16.5,
                    SpecificHeat_J_kgK = 450.0,
                    ThermalExpansion_1_K = 11.5e-6,
                    MaxServiceTemp_C = 650.0,
                    MeltingPoint_C = 1390.0,
                    Hardness_HB = 248.0,
                    FatigueStrength_Pa = 345e6,
                    ElongationPercent = 25.0,
                    ReductionOfAreaPercent = 40.0
                },
                ["Inconel_718"] = new InconelA286
                {
                    Name = "Inconel_718",
                    Density_kg_m3 = 8190.0,
                    YieldStrength_Pa = 1030e6,
                    TensileStrength_Pa = 1240e6,
                    YoungsModulus_Pa = 200e9,
                    PoissonRatio = 0.3,
                    ThermalConductivity_W_mK = 11.4,
                    SpecificHeat_J_kgK = 435.0,
                    ThermalExpansion_1_K = 13.0e-6,
                    MaxServiceTemp_C = 700.0,
                    MeltingPoint_C = 1336.0,
                    Hardness_HB = 380.0,
                    FatigueStrength_Pa = 380e6,
                    ElongationPercent = 30.0,
                    ReductionOfAreaPercent = 45.0
                },
                ["Waspaloy"] = new InconelA286
                {
                    Name = "Waspaloy",
                    Density_kg_m3 = 8270.0,
                    YieldStrength_Pa = 1030e6,
                    TensileStrength_Pa = 1275e6,
                    YoungsModulus_Pa = 214e9,
                    PoissonRatio = 0.3,
                    ThermalConductivity_W_mK = 11.0,
                    SpecificHeat_J_kgK = 420.0,
                    ThermalExpansion_1_K = 12.8e-6,
                    MaxServiceTemp_C = 760.0,
                    MeltingPoint_C = 1330.0,
                    Hardness_HB = 380.0,
                    FatigueStrength_Pa = 400e6,
                    ElongationPercent = 25.0,
                    ReductionOfAreaPercent = 35.0
                }
            };
        }

        public static InconelA286 Get(string name)
        {
            return GetAlloys().TryGetValue(name, out var alloy) ? alloy : new InconelA286();
        }
    }
}
