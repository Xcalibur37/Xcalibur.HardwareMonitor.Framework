using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

/// <summary>
/// Name prefix
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class NamePrefixAttribute : Attribute
{
    /// <summary>
    /// Gets the prefix.
    /// </summary>
    /// <value>
    /// The prefix.
    /// </value>
    public string Prefix { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamePrefixAttribute"/> class.
    /// </summary>
    /// <param name="namePrefix">The name prefix.</param>
    public NamePrefixAttribute(string namePrefix)
    {
        Prefix = namePrefix;
    }
}