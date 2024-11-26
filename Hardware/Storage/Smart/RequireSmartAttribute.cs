using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart;

/// <summary>
/// Require Smart Attribute
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class RequireSmartAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequireSmartAttribute"/> class.
    /// </summary>
    /// <param name="attributeId">The attribute identifier.</param>
    public RequireSmartAttribute(byte attributeId)
    {
        AttributeId = attributeId;
    }

    /// <summary>
    /// Gets the attribute identifier.
    /// </summary>
    /// <value>
    /// The attribute identifier.
    /// </value>
    public byte AttributeId { get; }
}