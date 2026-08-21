# Rocket Propulsion Theory

## FFSC Cycle

Full-Flow Staged Combustion (FFSC) is a power cycle where:

1. Both oxidizer (LOX) and fuel (CH4) pass through preburners
2. Preburner gases drive the turbopump turbine
3. All propellant flows through the main combustion chamber

### Advantages

- Higher efficiency than open cycle
- No fuel-rich turbine exhaust
- Better cooling with fuel-rich preburner gas
- Higher chamber pressure possible

## De Laval Nozzle

The convergent-divergent nozzle accelerates flow to supersonic speeds:

```
    ___
   /   \  Convergent
  /     \
 |  G   |  Throat (Mach = 1)
 |  a   |
 |  r   |  Divergent
 |  g   |/
 |  a   /
 |  n   /
 |  t   /
 |  a   /
  \   /
   \_/
```

### Isentropic Relations

- T/T* = (2/(gamma+1))
- P/P* = (2/(gamma+1))^(gamma/(gamma-1))
- A/A* = 1/M * [(2/(gamma+1))*(1 + (gamma-1)/2 * M^2)]^((gamma+1)/(2(gamma-1)))

## Characteristic Length L*

L* = Vc / At

Where:
- Vc = chamber volume [m^3]
- At = throat area [m^2]

Typical values: 0.8-2.0 m for LOX/CH4

## Rao Nozzle Contour

Rao optimization minimizes divergence loss while keeping nozzle length reasonable:

- Parabolic contour from throat to exit
- Exit angle: 12-15 degrees
- Wall pressure distribution optimized

## Bartz Heat Transfer

The Bartz correlation predicts heat flux in rocket nozzles:

```
hg = 0.026 * mu^0.2 * cp^0.6 * (Pc/C*)^0.8 *
     Dt^0.2 * Rc^-0.1 * (At/A)^0.9 * Pr^-0.6

Qw = hg * (Tg - Tw)
```

## Euler Turbomachinery

For turbopump design:

```
DeltaH = U2*Cu2 - U1*Cu1
Q = 2*pi*rm*h*Cm
```

Where:
- U = blade speed
- Cu = tangential velocity component
- Cm = meridional velocity
- rm = mean radius
- h = blade height
