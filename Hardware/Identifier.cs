using System;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware;

/// <summary>
/// Represents a unique <see cref="ISensor" />/<see cref="IHardware" /> identifier in text format with a / separator.
/// </summary>
public class Identifier : IComparable<Identifier>
{
    #region Fields

    private const char Separator = '/';
    private readonly string _identifier;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Identifier"/> class.
    /// </summary>
    /// <param name="identifiers">The identifiers.</param>
    public Identifier(params string[] identifiers)
    {
        CoerceIdentifiers(identifiers);
        StringBuilder s = new();
        foreach (var id in identifiers)
        {
            s.Append(Separator);
            s.Append(id);
        }

        _identifier = s.ToString();
    }

    /// <summary>
    /// Creates a new identifier instance based on the base <see cref="Identifier" /> and additional elements.
    /// </summary>
    /// <param name="identifier">Base identifier being the beginning of the new one.</param>
    /// <param name="extensions">Additional parts by which the base <see cref="Identifier" /> will be extended.</param>
    public Identifier(Identifier identifier, params string[] extensions)
    {
        CoerceIdentifiers(extensions);
        StringBuilder s = new();
        s.Append(identifier);
        foreach (var id in extensions)
        {
            s.Append(Separator);
            s.Append(id);
        }

        _identifier = s.ToString();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Coerces the identifiers.
    /// </summary>
    /// <param name="identifiers">The identifiers.</param>
    private static void CoerceIdentifiers(string[] identifiers)
    {
        for (int i = 0; i < identifiers.Length; i++)
        {
            string s = identifiers[i];
            if (s.IndexOf(' ') < 0) continue;
            identifiers[i] = s.Replace(' ', '-');
        }
    }

    /// <inheritdoc />
    public int CompareTo(Identifier other) => other == null
        ? 1
        : string.Compare(_identifier, other._identifier, StringComparison.Ordinal);

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        var id = obj as Identifier;
        if (id == null) return false;

        return _identifier == id._identifier;
    }

    /// <inheritdoc />
    public override int GetHashCode() => _identifier.GetHashCode();

    /// <inheritdoc />
    public override string ToString() => _identifier;

    #endregion

    #region Operators

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="id1">The id1.</param>
    /// <param name="id2">The id2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator ==(Identifier id1, Identifier id2) => id1 is null && id2 is null || id1 is not null && id1.Equals(id2);

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="id1">The id1.</param>
    /// <param name="id2">The id2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator !=(Identifier id1, Identifier id2) => !(id1 == id2);

    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="id1">The id1.</param>
    /// <param name="id2">The id2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <(Identifier id1, Identifier id2) => id1 == null ? id2 != null : id1.CompareTo(id2) < 0;

    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="id1">The id1.</param>
    /// <param name="id2">The id2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >(Identifier id1, Identifier id2) => id1 != null && id1.CompareTo(id2) > 0;

    #endregion
}