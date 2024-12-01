namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    /// <summary>
    /// ADL Status
    /// </summary>
    internal enum AdlStatus
    {
        /// <summary>
        /// All OK, but need to wait.
        /// </summary>
        AdlOkWait = 4,

        /// <summary>
        /// All OK, but need restart.
        /// </summary>
        AdlOkRestart = 3,

        /// <summary>
        /// All OK but need mode change.
        /// </summary>
        AdlOkModeChange = 2,

        /// <summary>
        /// All OK, but with warning.
        /// </summary>
        AdlOkWarning = 1,

        /// <summary>
        /// ADL function completed successfully.
        /// </summary>
        AdlOk = 0,

        /// <summary>
        /// Generic Error. Most likely one or more of the Escape calls to the driver
        /// failed!
        /// </summary>
        AdlErr = -1,

        /// <summary>
        /// ADL not initialized.
        /// </summary>
        AdlErrNotInit = -2,

        /// <summary>
        /// One of the parameter passed is invalid.
        /// </summary>
        AdlErrInvalidParam = -3,

        /// <summary>
        /// One of the parameter size is invalid.
        /// </summary>
        AdlErrInvalidParamSize = -4,

        /// <summary>
        /// Invalid ADL index passed.
        /// </summary>
        AdlErrInvalidAdlIdx = -5,

        /// <summary>
        /// Invalid controller index passed.
        /// </summary>
        AdlErrInvalidControllerIdx = -6,

        /// <summary>
        /// Invalid display index passed.
        /// </summary>
        AdlErrInvalidDiplayIdx = -7,

        /// <summary>
        /// Function not supported by the driver.
        /// </summary>
        AdlErrNotSupported = -8,

        /// <summary>
        /// Null Pointer error.
        /// </summary>
        AdlErrNullPointer = -9,

        /// <summary>
        /// Call can't be made due to disabled adapter.
        /// </summary>
        AdlErrDisabledAdapter = -10,

        /// <summary>
        /// Invalid Callback.
        /// </summary>
        AdlErrInvalidCallback = -11,

        /// <summary>
        /// Display Resource conflict.
        /// </summary>
        AdlErrResourceConflict = -12,

        /// <summary>
        /// Failed to update some of the values. Can be returned by set request that
        /// include multiple values if not all values were successfully committed.
        /// </summary>
        AdlErrSetIncomplete = -20,

        /// <summary>
        /// There's no Linux XDisplay in Linux Console environment.
        /// </summary>
        AdlErrNoXdisplay = -21
    }
}
