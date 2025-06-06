// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Stride.Core.Mathematics;

/// <summary>
/// Represents a color in the form of Hue, Saturation, Value, Alpha.
/// </summary>
[DataContract("ColorHSV")]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct ColorHSV : IEquatable<ColorHSV>, IFormattable
{
    private const string ToStringFormat = "Hue:{0} Saturation:{1} Value:{2} Alpha:{3}";

    /// <summary>
    /// The Hue of the color.
    /// </summary>
    [DataMember(0)]
    public float H;

    /// <summary>
    /// The Saturation of the color.
    /// </summary>
    [DataMember(1)]
    public float S;

    /// <summary>
    /// The Value of the color.
    /// </summary>
    [DataMember(2)]
    public float V;

    /// <summary>
    /// The alpha component of the color.
    /// </summary>
    [DataMember(3)]
    public float A;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorHSV"/> struct.
    /// </summary>
    /// <param name="h">The h.</param>
    /// <param name="s">The s.</param>
    /// <param name="v">The v.</param>
    /// <param name="a">A.</param>
    public ColorHSV(float h, float s, float v, float a)
    {
        H = h;
        S = s;
        V = v;
        A = a;
    }

    /// <summary>
    /// Converts the color into a three component vector.
    /// </summary>
    /// <returns>A three component vector containing the red, green, and blue components of the color.</returns>
    public readonly Color4 ToColor()
    {
        float hdiv = H / 60;
        int hi = (int)hdiv;
        float f = hdiv - hi;

        float p = V * (1 - S);
        float q = V * (1 - (S * f));
        float t = V * (1 - (S * (1 - f)));

        return hi switch
        {
            0 => new Color4(V, t, p, A),
            1 => new Color4(q, V, p, A),
            2 => new Color4(p, V, t, A),
            3 => new Color4(p, q, V, A),
            4 => new Color4(t, p, V, A),
            _ => new Color4(V, p, q, A),
        };
    }

    /// <summary>
    /// Converts the color into a HSV color.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>A HSV color</returns>
    public static ColorHSV FromColor(Color4 color)
    {
        float max = MathF.Max(color.R, MathF.Max(color.G, color.B));
        float min = MathF.Min(color.R, MathF.Min(color.G, color.B));

        float delta = max - min;
        float h = 0.0f;

        if (delta > 0.0f)
        {
            if (color.R >= max)
                h = (color.G - color.B) / delta;
            else if (color.G >= max)
                h = ((color.B - color.R) / delta) + 2.0f;
            else
                h = ((color.R - color.G) / delta) + 4.0f;
            h *= 60.0f;

            if (h < 0)
                h += 360f;
        }

        float s = MathUtil.IsZero(max) ? 0.0f : delta / max;

        return new ColorHSV(h, s, max, color.A);
    }

    /// <inheritdoc/>
    public readonly bool Equals(ColorHSV other)
    {
        return other.H.Equals(H) && other.S.Equals(S) && other.V.Equals(V) && other.A.Equals(A);
    }

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is ColorHSV color && Equals(color);
    }

    /// <inheritdoc/>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(H, S, V, A);
    }

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> that represents this instance.
    /// </returns>
    public override readonly string ToString()
    {
        return ToString(CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>
    /// A <see cref="string"/> that represents this instance.
    /// </returns>
    public readonly string ToString(string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>
    /// A <see cref="string"/> that represents this instance.
    /// </returns>
    public readonly string ToString(IFormatProvider? formatProvider)
    {
        return string.Format(formatProvider, ToStringFormat, H, S, V, A);
    }

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>
    /// A <see cref="string"/> that represents this instance.
    /// </returns>
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (format == null)
            return ToString(formatProvider);

        return string.Format(formatProvider, ToStringFormat,
                             H.ToString(format, formatProvider),
                             S.ToString(format, formatProvider),
                             V.ToString(format, formatProvider),
                             A.ToString(format, formatProvider));
    }

    /// <summary>
    /// Deconstructs the vector's components into named variables.
    /// </summary>
    /// <param name="h">The H component</param>
    /// <param name="s">The S component</param>
    /// <param name="v">The V component</param>
    /// <param name="a">The A component</param>
    public readonly void Deconstruct(out float h, out float s, out float v, out float a)
    {
        h = H;
        s = S;
        v = V;
        a = A;
    }
}
