# Thermodynamics

## Chemical Equilibrium

LOX/CH4 combustion produces:

CO2 + 2H2O (stoichiometric)

With excess O2 (O/F = 3.6):
- Products: CO2, H2O, O2, CO, H2
- Tad depends on mixture ratio and chamber pressure

## Temperature Calculation

The adiabatic flame temperature is found by solving:

h_reactants(T_initial) = h_products(Tad)

Using:
- NASA polynomial Cp(T)
- Iterative solver (bisection or Newton-Raphson)

## Cp(T) Polynomials

Cp(T) = a0 + a1*T + a2*T^2 + a3*T^3 + a4*T^4

for T in [200, 1000] K and [1000, 6000] K.

## Gas Properties

| Species | M (g/mol) | Cp (J/mol-K) | gamma |
|---------|-----------|--------------|-------|
| O2      | 32.00     | ~900         | 1.40  |
| CH4     | 16.04     | ~2000        | 1.30  |
| CO2     | 44.01     | ~1000        | 1.28  |
| H2O     | 18.02     | ~2100        | 1.33  |
