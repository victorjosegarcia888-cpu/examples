// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Provides axis-aligned bounds for a three-dimensional object.</summary>
public interface IBounded3d
{
    /// <summary>Object bounds, which may be empty.</summary>
    Bounds3d oBounds { get; }
}
