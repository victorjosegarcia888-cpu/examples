// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Provides axis-aligned bounds for a two-dimensional object.</summary>
public interface IBounded2d
{
    /// <summary>Object bounds, which may be empty.</summary>
    Bounds2d oBounds { get; }
}
