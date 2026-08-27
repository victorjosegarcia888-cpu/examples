// SPDX-License-Identifier: Apache-2.0

using System.Runtime.InteropServices;

namespace PicoGK.Geometry;

/// <summary>Triangle defined by three zero-based unsigned vertex indices.</summary>
/// <remarks>
/// The sequential layout is part of the interoperability contract. Degenerate triangles are
/// representable; validity relative to a vertex buffer is the responsibility of the mesh source.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public readonly struct Triangle : IEquatable<Triangle>
{
    /// <summary>First vertex index.</summary>
    public readonly uint A;

    /// <summary>Second vertex index.</summary>
    public readonly uint B;

    /// <summary>Third vertex index.</summary>
    public readonly uint C;

    public Triangle(uint A, uint B, uint C)
    {
        this.A = A;
        this.B = B;
        this.C = C;
    }

    /// <inheritdoc/>
    public bool Equals(Triangle tri) => A == tri.A && B == tri.B && C == tri.C;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Triangle tri && Equals(tri);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(A, B, C);

    public static bool operator ==(Triangle triLeft, Triangle triRight) => triLeft.Equals(triRight);
    public static bool operator !=(Triangle triLeft, Triangle triRight) => !triLeft.Equals(triRight);

    /// <inheritdoc/>
    public override string ToString() => $"({A}, {B}, {C})";
}
