// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PicoGK.Geometry;

/// <summary>
/// Stores a right-handed orthonormal basis and position as a rigid transformation
/// from local coordinates to world coordinates.
/// </summary>
/// <remarks>
/// Public construction from approximate axes validates and orthonormalizes the
/// input once. Operations that preserve rigidity reuse their known-valid bases
/// without repeated normalization. The default value is not a valid frame; use
/// <see cref="World"/> for the identity frame.
/// </remarks>
[DebuggerDisplay("O=({vecPos.X:n3},{vecPos.Y:n3},{vecPos.Z:n3})")]
public readonly struct Frame3d : IEquatable<Frame3d>
{
    /// <summary>The identity frame at the world origin.</summary>
    public static readonly Frame3d World = new(
        Vector3.Zero,
        Vector3.UnitX,
        Vector3.UnitY,
        Vector3.UnitZ,
        true);

    /// <summary>Frame origin in world coordinates.</summary>
    public Vector3 vecPos { get; }

    /// <summary>Local positive X axis expressed in world coordinates.</summary>
    public Vector3 vecLx { get; }

    /// <summary>Local positive Y axis expressed in world coordinates.</summary>
    public Vector3 vecLy { get; }

    /// <summary>Local positive Z axis expressed in world coordinates.</summary>
    public Vector3 vecLz { get; }

    /// <summary>Creates a world-aligned frame at the supplied position.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Frame3d frmFromPos(Vector3 vecPos) => new(vecPos);

    /// <summary>Creates a frame from its position and approximate Z and X axes.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Frame3d frmFromZX(Vector3 vecPos, Vector3 vecApproxZ, Vector3 vecApproxX)
        => new(vecPos, vecApproxZ, vecApproxX);

    /// <summary>Creates a world-aligned frame at the supplied position.</summary>
    /// <exception cref="ArgumentException">The position contains a non-finite component.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d(Vector3 vecPos)
    {
        ValidateFinite(vecPos, nameof(vecPos));
        this.vecPos = vecPos;
        vecLx = Vector3.UnitX;
        vecLy = Vector3.UnitY;
        vecLz = Vector3.UnitZ;
    }

    /// <summary>
    /// Creates a frame from approximate Z and X directions, enforcing
    /// orthonormality and right-handedness.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// The origin or either direction contains a non-finite component, either
    /// direction has zero length, or the directions are parallel.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d(Vector3 vecOrigin, Vector3 vecApproxZ, Vector3 vecApproxX)
    {
        ValidateFinite(vecOrigin, nameof(vecOrigin));
        Orthonormalize(vecApproxZ, vecApproxX, out Vector3 vecZ, out Vector3 vecX, out Vector3 vecY);
        vecPos = vecOrigin;
        vecLz = vecZ;
        vecLx = vecX;
        vecLy = vecY;
#if DEBUG
        AssertOrthonormal(vecZ, vecX, vecY);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Frame3d(Vector3 vecPos, Vector3 vecLx, Vector3 vecLy, Vector3 vecLz, bool bTrustedBasis)
    {
        Debug.Assert(bTrustedBasis);
        this.vecPos = vecPos;
        this.vecLx = vecLx;
        this.vecLy = vecLy;
        this.vecLz = vecLz;
#if DEBUG
        Debug.Assert(bIsFinite(vecPos));
        AssertOrthonormal(vecLz, vecLx, vecLy);
#endif
    }

    /// <summary>
    /// Creates a frame from a <see cref="Matrix4x4"/> row-vector transform whose
    /// basis and translation occupy the matrix rows. The X and Z rows are treated
    /// as approximate directions and orthonormalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Frame3d frmFromMatrix4x4(in Matrix4x4 mat)
    {
        Vector3 vecX = new(mat.M11, mat.M12, mat.M13);
        Vector3 vecZ = new(mat.M31, mat.M32, mat.M33);
        Vector3 vecP = new(mat.M41, mat.M42, mat.M43);
        return new Frame3d(vecP, vecZ, vecX);
    }

    /// <summary>Transforms a local point to world coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 vecPtToWorld(Vector3 vecLocal)
        => vecLocal.X * vecLx + vecLocal.Y * vecLy + vecLocal.Z * vecLz + vecPos;

    /// <summary>Transforms a local XY-plane point to world coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 vecPtToWorld(Vector2 vecLocal) => vecPtToWorld(new Vector3(vecLocal, 0f));

    /// <summary>
    /// Rotates a local direction into world coordinates without changing its magnitude.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 vecDirToWorld(Vector3 vecLocalDir)
        => vecLocalDir.X * vecLx + vecLocalDir.Y * vecLy + vecLocalDir.Z * vecLz;

    /// <summary>
    /// Rotates a local XY-plane direction into world coordinates without changing its magnitude.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 vecDirToWorld(Vector2 vecLocalDir)
        => vecLocalDir.X * vecLx + vecLocalDir.Y * vecLy;

    /// <summary>Transforms a world point to local coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 vecPtFromWorld(Vector3 vecWorld)
    {
        Vector3 vecR = vecWorld - vecPos;
        return new(Vector3.Dot(vecR, vecLx), Vector3.Dot(vecR, vecLy), Vector3.Dot(vecR, vecLz));
    }

    /// <summary>
    /// Rotates a world direction into local coordinates without changing its magnitude.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 vecDirFromWorld(Vector3 vecWorldDir)
        => new(
            Vector3.Dot(vecWorldDir, vecLx),
            Vector3.Dot(vecWorldDir, vecLy),
            Vector3.Dot(vecWorldDir, vecLz));

    /// <summary>
    /// Composes this frame with another frame. The other transformation is
    /// applied first and this transformation second.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmCompose(in Frame3d frmOther)
        => new(
            vecPtToWorld(frmOther.vecPos),
            vecDirToWorld(frmOther.vecLx),
            vecDirToWorld(frmOther.vecLy),
            vecDirToWorld(frmOther.vecLz),
            true);

    /// <summary>Returns the inverse rigid transformation.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmInverse()
    {
        Vector3 vecInversePos = new(
            -Vector3.Dot(vecLx, vecPos),
            -Vector3.Dot(vecLy, vecPos),
            -Vector3.Dot(vecLz, vecPos));
        Vector3 vecInverseX = new(vecLx.X, vecLy.X, vecLz.X);
        Vector3 vecInverseY = new(vecLx.Y, vecLy.Y, vecLz.Y);
        Vector3 vecInverseZ = new(vecLx.Z, vecLy.Z, vecLz.Z);
        return new(vecInversePos, vecInverseX, vecInverseY, vecInverseZ, true);
    }

    /// <summary>Returns a copy translated by a local-coordinate displacement.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmMovedLocal(Vector3 vecDistance)
        => new(vecPtToWorld(vecDistance), vecLx, vecLy, vecLz, true);

    /// <summary>Returns a copy translated along its local X axis.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmMovedLocalX(float fDistanceX)
        => new(vecPos + fDistanceX * vecLx, vecLx, vecLy, vecLz, true);

    /// <summary>Returns a copy translated along its local Y axis.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmMovedLocalY(float fDistanceY)
        => new(vecPos + fDistanceY * vecLy, vecLx, vecLy, vecLz, true);

    /// <summary>Returns a copy translated along its local Z axis.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmMovedLocalZ(float fDistanceZ)
        => new(vecPos + fDistanceZ * vecLz, vecLx, vecLy, vecLz, true);

    /// <summary>
    /// Returns a copy whose orientation is rotated around a world-space axis
    /// through the frame origin. The position is unchanged.
    /// </summary>
    /// <exception cref="ArgumentException">The axis is zero length or non-finite.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The angle is non-finite.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmRotatedWorld(Vector3 vecAxis, Rad rAngle)
    {
        Vector3 vecUnitAxis = vecNormalizedChecked(vecAxis, nameof(vecAxis));
        if (!rAngle.bIsFinite())
            throw new ArgumentOutOfRangeException(nameof(rAngle), "Rotation angle must be finite.");

        Quaternion q = Quaternion.CreateFromAxisAngle(vecUnitAxis, rAngle.fRad);
        return new(
            vecPos,
            Vector3.Transform(vecLx, q),
            Vector3.Transform(vecLy, q),
            Vector3.Transform(vecLz, q),
            true);
    }

    /// <summary>Returns a copy translated by a world-coordinate displacement.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmMovedWorld(Vector3 vecDistance)
        => new(vecPos + vecDistance, vecLx, vecLy, vecLz, true);

    /// <summary>Returns a copy translated along the world X axis.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmMovedWorldX(float fDistanceX)
        => new(vecPos + new Vector3(fDistanceX, 0f, 0f), vecLx, vecLy, vecLz, true);

    /// <summary>Returns a copy translated along the world Y axis.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmMovedWorldY(float fDistanceY)
        => new(vecPos + new Vector3(0f, fDistanceY, 0f), vecLx, vecLy, vecLz, true);

    /// <summary>Returns a copy translated along the world Z axis.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmMovedWorldZ(float fDistanceZ)
        => new(vecPos + new Vector3(0f, 0f, fDistanceZ), vecLx, vecLy, vecLz, true);

    /// <summary>Returns the equivalent <see cref="Matrix4x4"/> row-vector rigid transform.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Matrix4x4 matAsMatrix4x4()
        => new(
            vecLx.X, vecLx.Y, vecLx.Z, 0f,
            vecLy.X, vecLy.Y, vecLy.Z, 0f,
            vecLz.X, vecLz.Y, vecLz.Z, 0f,
            vecPos.X, vecPos.Y, vecPos.Z, 1f);

    /// <summary>Returns a copy at a new world position with unchanged orientation.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmRepositioned(Vector3 vecNewPos)
        => new(vecNewPos, vecLx, vecLy, vecLz, true);

    /// <summary>
    /// Returns a copy whose current Z and X axes have been explicitly
    /// re-orthonormalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Frame3d frmReorthonormalized() => new(vecPos, vecLz, vecLx);

    /// <summary>Returns this frame as a quaternion rotation and world-space origin.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AsRigid(out Quaternion q, out Vector3 vecOrigin)
    {
        q = Quaternion.CreateFromRotationMatrix(matAsMatrix4x4());
        vecOrigin = vecPos;
    }

    /// <summary>Composes local-axis scale with this rigid transformation.</summary>
    public Matrix4x4 matComposeWithScale(in Vector3 vecScale)
        => Matrix4x4.CreateScale(vecScale) * matAsMatrix4x4();

    /// <summary>Transforms a local point to world coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(in Frame3d frm, Vector3 vecLocal) => frm.vecPtToWorld(vecLocal);

    /// <summary>Composes two frames, applying the right operand first.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Frame3d operator *(in Frame3d frmA, in Frame3d frmB) => frmA.frmCompose(frmB);

    /// <summary>
    /// Interpolates position linearly and orientation by shortest-path spherical
    /// interpolation. The interpolation parameter is clamped to [0,1].
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The interpolation parameter is non-finite.</exception>
    public static Frame3d frmInterpolate(in Frame3d frm0, in Frame3d frm1, float t)
    {
        if (!float.IsFinite(t))
            throw new ArgumentOutOfRangeException(nameof(t), "Interpolation parameter must be finite.");

        t = float.Clamp(t, 0f, 1f);
        frm0.AsRigid(out Quaternion q0, out _);
        frm1.AsRigid(out Quaternion q1, out _);

        if (Quaternion.Dot(q0, q1) < 0f)
            q1 = new(-q1.X, -q1.Y, -q1.Z, -q1.W);

        Quaternion q = Quaternion.Slerp(q0, q1, t);
        return new(
            Vector3.Lerp(frm0.vecPos, frm1.vecPos, t),
            Vector3.Transform(Vector3.UnitX, q),
            Vector3.Transform(Vector3.UnitY, q),
            Vector3.Transform(Vector3.UnitZ, q),
            true);
    }

    /// <inheritdoc/>
    public bool Equals(Frame3d frm)
        => vecPos.Equals(frm.vecPos)
        && vecLx.Equals(frm.vecLx)
        && vecLy.Equals(frm.vecLy)
        && vecLz.Equals(frm.vecLz);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Frame3d frm && Equals(frm);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(vecPos, vecLx, vecLy, vecLz);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void Orthonormalize(
        in Vector3 vecInZ,
        in Vector3 vecInX,
        out Vector3 vecZ,
        out Vector3 vecX,
        out Vector3 vecY)
    {
        vecZ = vecNormalizedChecked(vecInZ, nameof(vecInZ));
        ValidateFinite(vecInX, nameof(vecInX));

        Vector3 vecProjectedX = vecInX - Vector3.Dot(vecInX, vecZ) * vecZ;
        vecX = vecNormalizedChecked(
            vecProjectedX,
            nameof(vecInX),
            "Approximate X direction must not be zero or parallel to the approximate Z direction.");
        vecY = Vector3.Cross(vecZ, vecX);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static Vector3 vecNormalizedChecked(Vector3 vec, string strParamName, string? strMessage = null)
    {
        ValidateFinite(vec, strParamName);
        float fLengthSquared = vec.LengthSquared();
        if (!(fLengthSquared > 0f) || !float.IsFinite(fLengthSquared))
            throw new ArgumentException(strMessage ?? "Direction must have finite non-zero length.", strParamName);

        return vec / float.Sqrt(fLengthSquared);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void ValidateFinite(in Vector3 vec, string strParamName)
    {
        if (!bIsFinite(vec))
            throw new ArgumentException("Vector components must be finite.", strParamName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool bIsFinite(in Vector3 vec)
        => float.IsFinite(vec.X) && float.IsFinite(vec.Y) && float.IsFinite(vec.Z);

    [Conditional("DEBUG")]
    static void AssertOrthonormal(in Vector3 vecZ, in Vector3 vecX, in Vector3 vecY)
    {
        const float c_fUnitTolerance = 1e-5f;
        const float c_fOrthogonalTolerance = 1e-4f;
        const float c_fHandednessToleranceSquared = 1e-6f;

        bool bUnit = float.Abs(vecZ.LengthSquared() - 1f) < c_fUnitTolerance
            && float.Abs(vecX.LengthSquared() - 1f) < c_fUnitTolerance
            && float.Abs(vecY.LengthSquared() - 1f) < c_fUnitTolerance;
        bool bOrthogonal = float.Abs(Vector3.Dot(vecZ, vecX)) < c_fOrthogonalTolerance
            && float.Abs(Vector3.Dot(vecZ, vecY)) < c_fOrthogonalTolerance
            && float.Abs(Vector3.Dot(vecX, vecY)) < c_fOrthogonalTolerance;
        bool bRightHanded = Vector3.DistanceSquared(
            Vector3.Normalize(Vector3.Cross(vecZ, vecX)), vecY) < c_fHandednessToleranceSquared;

        Debug.Assert(bUnit && bOrthogonal && bRightHanded);
    }
}
