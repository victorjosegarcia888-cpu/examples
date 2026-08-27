// Frame2d.cs
//
// Minimal 2D frame for transforming bounds.
// Based on Frame3d pattern.

// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Numerics;

namespace PicoGK.Geometry;

/// <summary>Stores a 2D orthonormal basis and position as a rigid transformation.</summary>
[DebuggerDisplay("O=({vecPos.X:n3},{vecPos.Y:n3})")]
public readonly struct Frame2d : IEquatable<Frame2d>
{
    public static readonly Frame2d World = new(Vector2.Zero, Vector2.UnitX, Vector2.UnitY);

    public Vector2 vecPos { get; }
    public Vector2 vecLx { get; }
    public Vector2 vecLy { get; }

    public Frame2d(Vector2 vecPos, Vector2 vecLx, Vector2 vecLy)
    {
        ValidateFinite(vecPos, nameof(vecPos));
        Orthonormalize(vecLx, vecLy, out Vector2 vecX, out Vector2 vecY);
        this.vecPos = vecPos;
        this.vecLx = vecX;
        this.vecLy = vecY;
    }

    public Frame2d(Vector2 vecPos)
    {
        ValidateFinite(vecPos, nameof(vecPos));
        this.vecPos = vecPos;
        vecLx = Vector2.UnitX;
        vecLy = Vector2.UnitY;
    }

    public Vector2 vecPtToWorld(Vector2 vecLocal)
        => vecLocal.X * vecLx + vecLocal.Y * vecLy + vecPos;

    public Vector2 vecDirToWorld(Vector2 vecLocalDir)
        => vecLocalDir.X * vecLx + vecLocalDir.Y * vecLy;

    public Vector2 vecPtFromWorld(Vector2 vecWorld)
    {
        Vector2 vecR = vecWorld - vecPos;
        return new Vector2(Vector2.Dot(vecR, vecLx), Vector2.Dot(vecR, vecLy));
    }

    public bool Equals(Frame2d other)
        => vecPos.Equals(other.vecPos) && vecLx.Equals(other.vecLx) && vecLy.Equals(other.vecLy);

    public override bool Equals(object? obj) => obj is Frame2d other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(vecPos, vecLx, vecLy);

    static void Orthonormalize(in Vector2 vecInX, in Vector2 vecInY, out Vector2 vecX, out Vector2 vecY)
    {
        vecX = Vector2.Normalize(vecInX);
        vecY = new Vector2(-vecX.Y, vecX.X);
    }

    static void ValidateFinite(in Vector2 vec, string strParamName)
    {
        if (!float.IsFinite(vec.X) || !float.IsFinite(vec.Y))
            throw new ArgumentException("Vector components must be finite.", strParamName);
    }
}
