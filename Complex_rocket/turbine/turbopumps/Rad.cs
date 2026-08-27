// SPDX-License-Identifier: Apache-2.0

using System.Globalization;
using System.Numerics;

namespace PicoGK.Geometry;

/// <summary>Encapsulates an angle in radians.</summary>
public readonly struct Rad : IComparable<Rad>, IEquatable<Rad>
{
    readonly float m_fRad;

    /// <summary>Default absolute tolerance used by approximate comparisons.</summary>
    public const float fToleranceDefault = 1e-6f;

    public static readonly Rad Zero = new(0f);
    public static readonly Rad Deg45 = rFromDeg(45f);
    public static readonly Rad Deg90 = rFromDeg(90f);
    public static readonly Rad Deg180 = rFromDeg(180f);
    public static readonly Rad Deg360 = rFromDeg(360f);

    /// <summary>Initializes an angle from a value in radians.</summary>
    public Rad(float fRad)
    {
        m_fRad = fRad;
    }

    public static Rad rFromRad(float fRad) => new(fRad);

    public static Rad rFromDeg(float fDegrees) => new(fDegrees * float.Tau / 360f);

    /// <summary>Maps a value in [0,1] to [0,2π], clamping values outside the input range.</summary>
    public static Rad rFromNormalized(float fNormalized)
    {
        if (fNormalized <= 0f)
            return Zero;
        if (fNormalized >= 1f)
            return Deg360;
        return Deg360 * fNormalized;
    }

    public float fRad => m_fRad;
    public float fDeg => m_fRad * 360f / float.Tau;

    /// <summary>Normalizes the angle to [-π,+π].</summary>
    public Rad rNormalizedSigned()
    {
        float f = MathF.IEEERemainder(m_fRad, float.Tau);
        return f == 0f ? Zero : new Rad(f);
    }

    /// <summary>Normalizes the angle to [0,2π).</summary>
    public Rad rNormalizedPositive()
    {
        float f = m_fRad % float.Tau;
        if (f < 0f)
            f += float.Tau;
        return f == 0f ? Zero : new Rad(f);
    }

    public static implicit operator float(Rad r) => r.m_fRad;
    public static explicit operator Rad(float fRad) => new(fRad);

    public bool bAlmostEqual(Rad other, float fToleranceRad = fToleranceDefault)
        => float.Abs(m_fRad - other.m_fRad) <= fToleranceRad;

    public bool bAlmostEqualPeriodic(Rad other, float fToleranceRad = fToleranceDefault)
        => float.Abs((this - other).rNormalizedSigned().m_fRad) <= fToleranceRad;

    public bool bIsFinite() => float.IsFinite(m_fRad);
    public float fSin() => float.Sin(m_fRad);
    public float fCos() => float.Cos(m_fRad);
    public float fTan() => float.Tan(m_fRad);

    public static Rad rAtan2(Vector2 vec) => rAtan2(vec.Y, vec.X);
    public static Rad rAtan2(float fY, float fX) => new(float.Atan2(fY, fX));
    public static Rad rAtan(float f) => new(float.Atan(f));
    public static Rad rAcos(float fValue) => new(float.Acos(fValue));
    public static Rad rAcosClamped(float fValue) => new(float.Acos(float.Clamp(fValue, -1f, 1f)));
    public static Rad rAsin(float fValue) => new(float.Asin(fValue));
    public static Rad rAsinClamped(float fValue) => new(float.Asin(float.Clamp(fValue, -1f, 1f)));

    public static Rad operator +(Rad a, Rad b) => new(a.m_fRad + b.m_fRad);
    public static Rad operator -(Rad a, Rad b) => new(a.m_fRad - b.m_fRad);
    public static Rad operator *(Rad r, float fScale) => new(r.m_fRad * fScale);
    public static Rad operator *(float fScale, Rad r) => new(fScale * r.m_fRad);
    public static Rad operator /(Rad r, float fScale) => new(r.m_fRad / fScale);
    public static float operator /(Rad a, Rad b) => a.m_fRad / b.m_fRad;
    public static Rad operator +(Rad r) => r;
    public static Rad operator -(Rad r) => new(-r.m_fRad);

    public override string ToString() => $"{fDeg.ToString("0.#", CultureInfo.InvariantCulture)}º";
    public int CompareTo(Rad other) => m_fRad.CompareTo(other.m_fRad);
    public bool Equals(Rad other) => m_fRad.Equals(other.m_fRad);
    public override bool Equals(object? obj) => obj is Rad other && Equals(other);
    public override int GetHashCode() => m_fRad.GetHashCode();

    public static bool operator <(Rad left, Rad right) => left.m_fRad < right.m_fRad;
    public static bool operator >(Rad left, Rad right) => left.m_fRad > right.m_fRad;
    public static bool operator <=(Rad left, Rad right) => left.m_fRad <= right.m_fRad;
    public static bool operator >=(Rad left, Rad right) => left.m_fRad >= right.m_fRad;
    public static bool operator ==(Rad left, Rad right) => left.Equals(right);
    public static bool operator !=(Rad left, Rad right) => !left.Equals(right);
}
