// ChamberChemistry.cs
//
// Quimica de la camara de combustion para ciclo FFSC.
// Calcula temperaturas, composicion y parametros termoquimicos.

using System;

namespace EngineFFSC.CombustionChamber
{
    public class ChamberChemistry
    {
        public double ChamberPressure_Pa { get; set; }
        public double MixtureRatio { get; set; }
        public double ChamberTemp_K { get; set; }
        public double Gamma { get; set; }
        public double MolecularWeight { get; set; }
        public double Cstar { get; set; }
        public double Thrust { get; set; }

        public double ChamberPressure_bar
        {
            get => ChamberPressure_Pa / 1e5;
            set => ChamberPressure_Pa = value * 1e5;
        }

        public double Isp { get; set; }

        public static ChamberChemistry Compute(EngineParams p)
        {
            return new ChamberChemistry
            {
                ChamberPressure_Pa = p.ChamberPressure_Pa,
                MixtureRatio = p.MixtureRatio,
                ChamberTemp_K = p.ChamberTemp_K,
                Gamma = p.Gamma,
                MolecularWeight = p.MolecularWeight,
                Cstar = p.Cstar,
                Thrust = p.Thrust,
                Isp = p.Isp
            };
        }

        public double Density()
        {
            return ChamberPressure_Pa * MolecularWeight / (8.314 * ChamberTemp_K);
        }

        public double SoundSpeed()
        {
            return Math.Sqrt(Gamma * 8.314 * ChamberTemp_K / MolecularWeight);
        }

        public double MassFlowRate()
        {
            return Thrust / (Isp * 9.81);
        }

        public double OxidizerMassFlow()
        {
            double mdot = MassFlowRate();
            return mdot * MixtureRatio / (1.0 + MixtureRatio);
        }

        public double FuelMassFlow()
        {
            double mdot = MassFlowRate();
            return mdot / (1.0 + MixtureRatio);
        }

        public override string ToString()
        {
            return $"Pc={ChamberPressure_bar:F1} bar, OF={MixtureRatio:F1}, Tc={ChamberTemp_K:F0} K, c*={Cstar:F0} m/s, Isp={Isp:F0} s";
        }
    }

    public class EngineParams
    {
        public double Thrust { get; set; } = 2_500_000.0;
        public double ChamberPressure_bar { get; set; } = 350.0;
        public double ChamberPressure_Pa
        {
            get => ChamberPressure_bar * 1e5;
            set => ChamberPressure_bar = value / 1e5;
        }
        public double ExpansionRatio { get; set; } = 45.0;
        public double MixtureRatio { get; set; } = 3.6;
        public double Cstar { get; set; } = 2400.0;
        public double Isp { get; set; } = 380.0;
        public double ChamberTemp_K { get; set; } = 3600.0;
        public double Gamma { get; set; } = 1.3;
        public double MolecularWeight { get; set; } = 23.0;
    }
}
