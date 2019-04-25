#if UNITY_EDITOR
using UnityEngine.Experimental.Input.LowLevel;

namespace UnityEngine.Experimental.Input.Editor
{
    internal class InputDiagnostics : IInputDiagnostics
    {
        public void OnCannotFindDeviceForEvent(InputEventPtr eventPtr)
        {
            Debug.LogError("Cannot find device for input event: " + eventPtr);
        }

        public void OnEventTimestampOutdated(InputEventPtr eventPtr, InputDevice device)
        {
            Debug.LogError(
                $"'{eventPtr.type}' input event for device '{device}' is outdated (event time: {eventPtr.time}, device time: {device.lastUpdateTime})");
        }

        public void OnEventFormatMismatch(InputEventPtr eventPtr, InputDevice device)
        {
            Debug.LogError(
                $"'{eventPtr.type}' input event for device '{device}' has incorrect format (event format: '{eventPtr.type}', device format: '{device.stateBlock.format}')");
        }

        public void OnEventForDisabledDevice(InputEventPtr eventPtr, InputDevice device)
        {
            Debug.LogError($"Device '{device}' received input event '{eventPtr}' but the device is disabled");
        }
    }
}
#endif // UNITY_EDITOR
