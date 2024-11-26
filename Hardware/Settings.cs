namespace Xcalibur.HardwareMonitor.Framework.Hardware
{
    /// <summary>
    /// <see cref="Computer" /> specific additional settings passed to its <see cref="IHardware" />.
    /// </summary>
    public class Settings : ISettings
    {
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="name">Key to which the setting value is assigned.</param>
        /// <returns></returns>
        public bool Contains(string name) => false;

        /// <summary>
        /// Assigns a setting option to a given key.
        /// </summary>
        /// <param name="name">Key to which the setting value is assigned.</param>
        /// <param name="value">Text setting value.</param>
        public void SetValue(string name, string value) { }

        /// <summary>
        /// Gets a setting option assigned to the given key.
        /// </summary>
        /// <param name="name">Key to which the setting value is assigned.</param>
        /// <param name="value">Default value.</param>
        /// <returns></returns>
        public string GetValue(string name, string value) => value;

        /// <summary>
        /// Removes a setting with the specified key from the settings collection.
        /// </summary>
        /// <param name="name">Key to which the setting value is assigned.</param>
        public void Remove(string name)
        { }
    }
}
