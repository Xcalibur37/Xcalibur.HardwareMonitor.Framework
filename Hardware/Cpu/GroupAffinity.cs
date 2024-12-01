namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu;

/// <summary>
/// This structure describes a group-specific affinity.
/// </summary>
public readonly struct GroupAffinity
{
    #region Fields

    /// <summary>
    /// The undefined
    /// </summary>
    public static GroupAffinity Undefined = new(ushort.MaxValue, 0);

    #endregion

    #region Properties

    /// <summary>
    /// Gets the group.
    /// </summary>
    public ushort Group { get; }

    /// <summary>
    /// Gets the mask.
    /// </summary>
    public ulong Mask { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAffinity" /> struct.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <param name="mask">The mask.</param>
    public GroupAffinity(ushort group, ulong mask)
    {
        Group = group;
        Mask = mask;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="o">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object o)
    {
        if (o == null || GetType() != o.GetType()) return false;
        var a = (GroupAffinity)o;
        return (Group == a.Group) && (Mask == a.Mask);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode() => Group.GetHashCode() ^ Mask.GetHashCode();

    /// <summary>
    /// Gets a single group affinity.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <param name="index">The index.</param>
    /// <returns><see cref="GroupAffinity" />.</returns>
    public static GroupAffinity Single(ushort group, int index) => new(group, 1UL << index);

    #endregion

    #region Operators

    /// <summary>
    /// Implements the == operator.
    /// </summary>
    /// <param name="a1">The a1.</param>
    /// <param name="a2">The a2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(GroupAffinity a1, GroupAffinity a2) => (a1.Group == a2.Group) && (a1.Mask == a2.Mask);

    /// <summary>
    /// Implements the != operator.
    /// </summary>
    /// <param name="a1">The a1.</param>
    /// <param name="a2">The a2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(GroupAffinity a1, GroupAffinity a2) => (a1.Group != a2.Group) || (a1.Mask != a2.Mask);

    #endregion
}