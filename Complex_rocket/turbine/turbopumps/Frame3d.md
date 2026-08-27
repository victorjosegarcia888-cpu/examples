# `Frame3d` design and performance contract

`Frame3d` is a performance-critical rigid transformation from local coordinates to world coordinates.

## Representation

The frame stores four `Vector3` values directly:

- world-space position;
- local X axis in world coordinates;
- local Y axis in world coordinates;
- local Z axis in world coordinates.

The basis is right-handed and orthonormal when constructed and used according to this contract. It is stored explicitly because PicoGK geometry workloads frequently transform points and access individual axes. Quaternion interpolation is comparatively uncommon and often cannot express the required application-specific alignment policy by itself.

`default(Frame3d)` contains a zero basis and is invalid. Use `Frame3d.World` for the identity transformation. The type deliberately does not branch in hot operations to reinterpret the default value as identity.

## Construction boundaries

Public construction from approximate Z and X directions:

1. rejects non-finite input;
2. rejects zero-length directions;
3. rejects parallel Z and X directions;
4. normalizes Z;
5. projects and normalizes X perpendicular to Z;
6. derives Y as `Z × X`.

This validation and orthonormalization happens once at the external construction boundary. A private trusted construction path is used when an operation already has a rigid basis.

## Hot-path operations

Point and direction transformations perform direct basis arithmetic. They do not validate or normalize their arguments or results.

Operations that mathematically preserve an existing rigid basis do not re-orthonormalize it:

- translation and repositioning;
- inversion;
- frame composition;
- world-space rotation;
- interpolation after construction of the resulting orientation.

Debug builds assert the private trusted-constructor preconditions. Release builds do not add validation to that path. Callers must pass finite values to transformation operations.

Direction transformations preserve vector magnitude. They do not implicitly convert vectors to unit length. Callers requiring a unit vector must normalize explicitly.

## Composition

`frmA * frmB` applies `frmB` first and `frmA` second:

```csharp
(frmA * frmB).vecPtToWorld(vecPoint)
== frmA.vecPtToWorld(frmB.vecPtToWorld(vecPoint));
```

## Numerical drift

Repeated floating-point composition or rotation can accumulate small orthonormality errors. `frmReorthonormalized()` performs an explicit repair when an algorithm requires it. Routine operations intentionally avoid unconditional repair.

Algorithms with long transformation chains must choose an appropriate repair policy rather than making every `Frame3d` operation pay that cost.

## Interpolation

`frmInterpolate` is a generic convenience operation:

- position is interpolated linearly;
- orientation uses shortest-path quaternion spherical interpolation;
- the interpolation parameter is clamped to `[0,1]`.

Geometry algorithms that require policies such as maintaining a preferred Z direction should implement those constraints directly using the stored basis rather than relying on generic quaternion interpolation.
