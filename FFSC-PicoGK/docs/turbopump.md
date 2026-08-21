# Turbopump Design

## Overview

The turbopump is the heart of the FFSC feed system. It consists of:
- Centrifugal pump (LOX)
- Centrifugal turbine (hot gas from preburner)
- Common shaft
- Bearings and seals

## Design Equations

### Euler Equation

DeltaH = U2*Cu2 - U1*Cu1

### Continuity

Q = 2*pi*rm*h*Cm

### Blade Speed

U = omega * r

where omega = 2*pi*N/60 (rad/s)

### Specific Speed

Ns = omega * sqrt(Q) / (DeltaH)^(3/4)

## NACA Blade Profiles

The blades use NACA 4-digit airfoil profiles for optimal hydrodynamic performance:

- NACA 65xx for pump blades
- NACA 16xx for turbine blades

## Materials

- Shaft: Inconel 718 or Maraging steel
- Blades: Ti-6Al-4V or Inconel 718
- Housing: Al 7075 or steel

## Bearing Types

- Angular contact bearings
- Magnetic bearings (advanced)
- Hydrodynamic bearings

## Seals

- Labyrinth seals (gas side)
- Mechanical seals (liquid side)
