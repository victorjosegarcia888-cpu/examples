# Lattice Structures

## Overview

Adaptive lattice structures provide lightweight reinforcement based on stress/thermal fields.

## Types

### Gyroid (TPMS)

Triple Periodic Minimal Surface with high strength-to-weight ratio:

f(x,y,z) = sin(x)*sin(y) + sin(y)*sin(z) + sin(z)*sin(x) = 0

### Quasicrystal

Non-periodic pattern (Penrose-type) for isotropic properties:

f(x,y,z) = cos(x*s) + cos(y*s*phi) + sin(y*s) + sin(z*s*phi) where phi = (1+sqrt(5))/2

### Dual Layer

- Layer 1 (high stress): thick nodes, sparse
- Layer 2 (low stress): thin nodes, dense

## Interpolation

alpha(s) = 1 - exp(-k*s)

where s = normalized stress/threshold, k = decay constant

## Volume Fraction

Optimize relative density between 15-40% for structural applications.
