// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes an axis-aligned box in its local coordinate system.</summary>
/// <remarks>
/// The frame origin is the center of the bottom face. The box extends symmetrically about local
/// X and Y and from zero to <see cref="fSizeZ"/> along local positive Z.
/// </remarks>
public interface IBox3d : IPrimitive3d
{
    /// <summary>Box extent along local X.</summary>
    float fSizeX { get; }

    /// <summary>Box extent along local Y.</summary>
    float fSizeY { get; }

    /// <summary>Box extent along local Z.</summary>
    float fSizeZ { get; }
}
