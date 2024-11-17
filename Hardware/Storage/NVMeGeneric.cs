




using System.Collections.Generic;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

public sealed class NVMeGeneric : AbstractStorage
{
    private const ulong Scale = 1000000;
    private const ulong Units = 512;
    private readonly NVMeInfo _info;
    private readonly List<NVMeSensor> _sensors = new();

    /// <summary>
    /// Gets the SMART data.
    /// </summary>
    public NVMeSmart Smart { get; }

    private NVMeGeneric(StorageInfo storageInfo, NVMeInfo info, int index, ISettings settings)
        : base(storageInfo, info.Model, info.Revision, "nvme", index, settings)
    {
        Smart = new NVMeSmart(storageInfo);
        _info = info;
        CreateSensors();
    }

    private static NVMeInfo GetDeviceInfo(StorageInfo storageInfo)
    {
        var smart = new NVMeSmart(storageInfo);
        return smart.GetInfo();
    }

    internal static AbstractStorage CreateInstance(StorageInfo storageInfo, ISettings settings)
    {
        NVMeInfo nvmeInfo = GetDeviceInfo(storageInfo);
        return nvmeInfo == null ? null : new NVMeGeneric(storageInfo, nvmeInfo, storageInfo.Index, settings);
    }

    protected override void CreateSensors()
    {
        NVMeHealthInfo log = Smart.GetHealthInfo();
        if (log != null)
        {
            AddSensor("Temperature", 0, false, SensorType.Temperature, health => health.Temperature);
            AddSensor("Available Spare", 1, false, SensorType.Level, health => health.AvailableSpare);
            AddSensor("Available Spare Threshold", 2, false, SensorType.Level, health => health.AvailableSpareThreshold);
            AddSensor("Percentage Used", 3, false, SensorType.Level, health => health.PercentageUsed);
            AddSensor("Data Read", 4, false, SensorType.Data, health => UnitsToData(health.DataUnitRead));
            AddSensor("Data Written", 5, false, SensorType.Data, health => UnitsToData(health.DataUnitWritten));

            int sensorIdx = 6;
            for (int i = 0; i < log.TemperatureSensors.Length; i++)
            {
                int idx = i;
                if (log.TemperatureSensors[idx] > short.MinValue)
                {
                    AddSensor("Temperature " + (idx + 1), sensorIdx, false, SensorType.Temperature, health => health.TemperatureSensors[idx]);
                    sensorIdx++;
                }
            }
        }

        base.CreateSensors();
    }

    private void AddSensor(string name, int index, bool defaultHidden, SensorType sensorType, GetSensorValue getValue)
    {
        var sensor = new NVMeSensor(name, index, defaultHidden, sensorType, this, Settings, getValue)
        {
            Value = 0
        };
        ActivateSensor(sensor);
        _sensors.Add(sensor);
    }

    private static float UnitsToData(ulong u)
    {
        // one unit is 512 * 1000 bytes, return in GB (not GiB)
        return Units * u / Scale;
    }

    protected override void UpdateSensors()
    {
        NVMeHealthInfo health = Smart.GetHealthInfo();
        if (health == null)
            return;

        foreach (NVMeSensor sensor in _sensors)
            sensor.Update(health);
    }

    public override void Close()
    {
        Smart?.Close();

        base.Close();
    }

    private delegate float GetSensorValue(NVMeHealthInfo health);

    private class NVMeSensor : Sensor
    {
        private readonly GetSensorValue _getValue;

        public NVMeSensor(string name, int index, bool defaultHidden, SensorType sensorType, Hardware hardware, ISettings settings, GetSensorValue getValue)
            : base(name, index, defaultHidden, sensorType, hardware, null, settings)
        {
            _getValue = getValue;
        }

        public void Update(NVMeHealthInfo health)
        {
            float v = _getValue(health);
            if (SensorType == SensorType.Temperature && v is < -1000 or > 1000)
                return;

            Value = v;
        }
    }
}
