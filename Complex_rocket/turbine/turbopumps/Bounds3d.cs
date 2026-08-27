// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Numerics;

namespace PicoGK.Geometry;

/// <summary>Immutable axis-aligned bounds in three-dimensional space.</summary>
/// <remarks>
/// The default value is empty. A zero-size bounds containing one point is not empty.
/// Geometric properties throw when accessed on an empty bounds.
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct Bounds3d : IEquatable<Bounds3d>
{
    readonly Vector3 m_vecMin;
    readonly Vector3 m_vecMax;
    readonly bool m_bHasValue;

    /// <summary>The canonical empty bounds.</summary>
    public static readonly Bounds3d Empty = default;

    /// <summary>Creates non-empty bounds from ordered finite extrema.</summary>
    /// <exception cref="ArgumentException">
    /// An extremum is non-finite or a minimum component exceeds its corresponding maximum.
    /// </exception>
    public Bounds3d(Vector3 vecMin, Vector3 vecMax)
    {
        ValidateFinite(vecMin, nameof(vecMin));
        ValidateFinite(vecMax, nameof(vecMax));
        if (vecMin.X > vecMax.X || vecMin.Y > vecMax.Y || vecMin.Z > vecMax.Z)
            throw new ArgumentException("Minimum components must not exceed maximum components.", nameof(vecMin));

        m_vecMin = vecMin;
        m_vecMax = vecMax;
        m_bHasValue = true;
    }

    /// <summary>Whether these bounds contain no points.</summary>
    public bool bIsEmpty => !m_bHasValue;

    /// <summary>Minimum coordinate.</summary>
    /// <exception cref="InvalidOperationException">The bounds are empty.</exception>
    public Vector3 vecMin => m_bHasValue ? m_vecMin : throw EmptyException();

    /// <summary>Maximum coordinate.</summary>
    /// <exception cref="InvalidOperationException">The bounds are empty.</exception>
    public Vector3 vecMax => m_bHasValue ? m_vecMax : throw EmptyException();

    /// <summary>Extent along each axis.</summary>
    /// <exception cref="InvalidOperationException">The bounds are empty.</exception>
    public Vector3 vecSize => m_bHasValue ? m_vecMax - m_vecMin : throw EmptyException();

    /// <summary>Center point.</summary>
    /// <exception cref="InvalidOperationException">The bounds are empty.</exception>
    public Vector3 vecCenter => m_bHasValue ? m_vecMin + (m_vecMax - m_vecMin) * 0.5f : throw EmptyException();

    /// <summary>Creates zero-size bounds containing one finite point.</summary>
    public static Bounds3d oFromPoint(Vector3 vecPoint) => new(vecPoint, vecPoint);

    /// <summary>Creates bounds containing all supplied points, or empty bounds for an empty span.</summary>
    public static Bounds3d oFromPoints(ReadOnlySpan<Vector3> avecPoints)
    {
        Bounds3d oBounds = Empty;
        foreach (Vector3 vecPoint in avecPoints)
            oBounds = oBounds.oIncluded(vecPoint);
        return oBounds;
    }

    /// <summary>Whether the bounds contain the point, including their boundary.</summary>
    public bool bContains(Vector3 vecPoint)
        => m_bHasValue
        && vecPoint.X >= m_vecMin.X && vecPoint.X <= m_vecMax.X
        && vecPoint.Y >= m_vecMin.Y && vecPoint.Y <= m_vecMax.Y
        && vecPoint.Z >= m_vecMin.Z && vecPoint.Z <= m_vecMax.Z;

    /// <summary>Whether these bounds completely contain the other bounds.</summary>
    public bool bContains(in Bounds3d oOther)
        => oOther.bIsEmpty
        || (m_bHasValue
            && oOther.m_vecMin.X >= m_vecMin.X && oOther.m_vecMax.X <= m_vecMax.X
            && oOther.m_vecMin.Y >= m_vecMin.Y && oOther.m_vecMax.Y <= m_vecMax.Y
            && oOther.m_vecMin.Z >= m_vecMin.Z && oOther.m_vecMax.Z <= m_vecMax.Z);

    /// <summary>Whether these bounds intersect or touch the other bounds.</summary>
    public bool bIntersects(in Bounds3d oOther)
        => m_bHasValue && oOther.m_bHasValue
        && m_vecMin.X <= oOther.m_vecMax.X && m_vecMax.X >= oOther.m_vecMin.X
        && m_vecMin.Y <= oOther.m_vecMax.Y && m_vecMax.Y >= oOther.m_vecMin.Y
        && m_vecMin.Z <= oOther.m_vecMax.Z && m_vecMax.Z >= oOther.m_vecMin.Z;

    /// <summary>Returns bounds that additionally contain the supplied point.</summary>
    public Bounds3d oIncluded(Vector3 vecPoint)
    {
        ValidateFinite(vecPoint, nameof(vecPoint));
        return !m_bHasValue
            ? oFromPoint(vecPoint)
            : new(Vector3.Min(m_vecMin, vecPoint), Vector3.Max(m_vecMax, vecPoint));
    }

    /// <summary>Returns the union of these and the other bounds.</summary>
    public Bounds3d oIncluded(in Bounds3d oOther)
    {
        if (!oOther.m_bHasValue)
            return this;
        if (!m_bHasValue)
            return oOther;
        return new(Vector3.Min(m_vecMin, oOther.m_vecMin), Vector3.Max(m_vecMax, oOther.m_vecMax));
    }

    /// <summary>Returns the intersection, or empty bounds when there is no intersection.</summary>
    public Bounds3d oIntersection(in Bounds3d oOther)
        => bIntersects(oOther)
            ? new(Vector3.Max(m_vecMin, oOther.m_vecMin), Vector3.Min(m_vecMax, oOther.m_vecMax))
            : Empty;

    /// <summary>Returns bounds expanded by the distance on every side.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The distance is negative or non-finite.</exception>
    public Bounds3d oExpanded(float fDistance)
    {
        if (!(fDistance >= 0f) || !float.IsFinite(fDistance))
            throw new ArgumentOutOfRangeException(nameof(fDistance), "Expansion distance must be finite and non-negative.");
        if (!m_bHasValue)
            return Empty;
        Vector3 vecDistance = new(fDistance);
        return new(m_vecMin - vecDistance, m_vecMax + vecDistance);
    }

    /// <summary>Returns bounds translated by a finite displacement.</summary>
    public Bounds3d oTranslated(Vector3 vecDistance)
    {
        ValidateFinite(vecDistance, nameof(vecDistance));
        return m_bHasValue ? new(m_vecMin + vecDistance, m_vecMax + vecDistance) : Empty;
    }

    /// <summary>Returns axis-aligned bounds containing these bounds transformed by the frame.</summary>
    public Bounds3d oTransformed(in Frame3d frm)
    {
        if (!m_bHasValue)
            return Empty;

        Bounds3d oResult = Empty;
        for (int nCorner = 0; nCorner < 8; nCorner++)
            oResult = oResult.oIncluded(frm.vecPtToWorld(vecCorner(nCorner)));
        return oResult;
    }

    /// <summary>
    /// Returns a corner selected by the low three bits: bits 0, 1, and 2 select X, Y, and Z max.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside [0,7].</exception>
    /// <exception cref="InvalidOperationException">The bounds are empty.</exception>
    public Vector3 vecCorner(int nIndex)
    {
        if ((uint)nIndex >= 8u)
            throw new ArgumentOutOfRangeException(nameof(nIndex));
        if (!m_bHasValue)
            throw EmptyException();
        return new(
            (nIndex & 1) == 0 ? m_vecMin.X : m_vecMax.X,
            (nIndex & 2) == 0 ? m_vecMin.Y : m_vecMax.Y,
            (nIndex & 4) == 0 ? m_vecMin.Z : m_vecMax.Z);
    }

    /// <inheritdoc/>
    public bool Equals(Bounds3d oOther)
        => m_bHasValue == oOther.m_bHasValue
        && (!m_bHasValue || (m_vecMin.Equals(oOther.m_vecMin) && m_vecMax.Equals(oOther.m_vecMax)));

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Bounds3d oOther && Equals(oOther);

    /// <inheritdoc/>
    public override int GetHashCode() => m_bHasValue ? HashCode.Combine(m_vecMin, m_vecMax) : 0;

    public static bool operator ==(Bounds3d oLeft, Bounds3d oRight) => oLeft.Equals(oRight);
    public static bool operator !=(Bounds3d oLeft, Bounds3d oRight) => !oLeft.Equals(oRight);

    /// <inheritdoc/>
    public override string ToString() => m_bHasValue ? $"[{m_vecMin} .. {m_vecMax}]" : "Empty";

    static InvalidOperationException EmptyException() => new("Empty bounds have no geometric extrema.");

    static void ValidateFinite(Vector3 vec, string strParamName)
    {
        if (!float.IsFinite(vec.X) || !float.IsFinite(vec.Y) || !float.IsFinite(vec.Z))
            throw new ArgumentException("Vector components must be finite.", strParamName);
    }
}
