





using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class NamePrefixAttribute : Attribute
{
    public NamePrefixAttribute(string namePrefix)
    {
        Prefix = namePrefix;
    }

    public string Prefix { get; }
}