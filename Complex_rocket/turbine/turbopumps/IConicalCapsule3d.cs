// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>
/// Describes a tapered capsule defined by two spheres and their connecting conical envelope.
/// </summary>
/// <remarks>
/// The frame origin is the midpoint between the sphere centers. The bottom and top sphere centers
/// lie at minus and plus half <see cref="fCenterlineLength"/> along local Z, respectively. When
/// both radii are equal, the geometry is equivalent to an <see cref="ICapsule3d"/> whose
/// <see cref="ICapsule3d.fCylinderHeight"/> equals <see cref="fCenterlineLength"/>.
/// </remarks>
public interface IConicalCapsule3d : IPrimitive3d
{
    /// <summary>Radius of the sphere at the local negative-Z end.</summary>
    float fBottomRadius { get; }

    /// <summary>Radius of the sphere at the local positive-Z end.</summary>
    float fTopRadius { get; }

    /// <summary>Distance between the bottom and top sphere centers.</summary>
    float fCenterlineLength { get; }
}
