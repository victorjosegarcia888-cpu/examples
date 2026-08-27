// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes an ellipsoid aligned with its local coordinate axes.</summary>
/// <remarks>The frame origin is the ellipsoid center.</remarks>
public interface IEllipsoid3d : IPrimitive3d
{
    /// <summary>Ellipsoid radius along local X.</summary>
    float fRadiusX { get; }

    /// <summary>Ellipsoid radius along local Y.</summary>
    float fRadiusY { get; }

    /// <summary>Ellipsoid radius along local Z.</summary>
    float fRadiusZ { get; }
}
