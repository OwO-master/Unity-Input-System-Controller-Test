using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.LowLevel;
using TMPro;
using System;

public class ControllerTest : MonoBehaviour
{
    public TextMeshProUGUI Information;
    public TextMeshProUGUI Controller_Dpad;
    public TextMeshProUGUI Controller_Buttons;
    public TextMeshProUGUI Controller_LeftStick;
    public TextMeshProUGUI Controller_RightStick;
    public TextMeshProUGUI Controller_Shoulder_Trigger;
    public TextMeshProUGUI Controller_Device_Name;
    public TextMeshProUGUI Input_System;

    private void Awake()
    {
        Application.runInBackground = true;
    }
    void Update()
    {
        if (Gamepad.current != null)
        {
            Information.text = "Controller 1" + Environment.NewLine +
                               "Display Name: " + Gamepad.current.displayName + Environment.NewLine +
                               "Name: " + Gamepad.current.name + Environment.NewLine +
                               "Short Display Name: " + Gamepad.current.shortDisplayName + Environment.NewLine +
                               "Description: " + Gamepad.current.description + Environment.NewLine +
                               "Device: " + Gamepad.current.device + Environment.NewLine +
                               "ID: " + Gamepad.current.id + Environment.NewLine +
                               "Layout: " + Gamepad.current.layout + Environment.NewLine +
                               "Native: " + Gamepad.current.native + Environment.NewLine +
                               "Noisy: " + Gamepad.current.noisy + Environment.NewLine +
                               "Path: " + Gamepad.current.path + Environment.NewLine +
                               "Remote: " + Gamepad.current.remote + Environment.NewLine +
                               "StateBlock: " + Gamepad.current.stateBlock + Environment.NewLine +
                               "Synthetic: " + Gamepad.current.synthetic + Environment.NewLine +
                               "Variants: " + Gamepad.current.variants;
            Controller_Dpad.text = "UP: " + Gamepad.current[GamepadButton.DpadUp].isPressed + Environment.NewLine +
                                   "Down: " + Gamepad.current[GamepadButton.DpadDown].isPressed + Environment.NewLine +
                                   "Left: " + Gamepad.current[GamepadButton.DpadLeft].isPressed + Environment.NewLine +
                                   "Right: " + Gamepad.current[GamepadButton.DpadRight].isPressed + Environment.NewLine;
            Controller_Buttons.text = "A: " + Gamepad.current[GamepadButton.A].isPressed + Environment.NewLine +
                                      "B: " + Gamepad.current[GamepadButton.B].isPressed + Environment.NewLine +
                                      "X: " + Gamepad.current[GamepadButton.X].isPressed + Environment.NewLine +
                                      "Y: " + Gamepad.current[GamepadButton.Y].isPressed + Environment.NewLine +
                                      "O: " + Gamepad.current[GamepadButton.Circle].isPressed + Environment.NewLine +
                                      "X: " + Gamepad.current[GamepadButton.Cross].isPressed + Environment.NewLine +
                                      "/_\\: " + Gamepad.current[GamepadButton.Triangle].isPressed + Environment.NewLine +
                                      "[ ]: " + Gamepad.current[GamepadButton.Square].isPressed + Environment.NewLine +
                                      "North: " + Gamepad.current[GamepadButton.North].isPressed + Environment.NewLine +
                                      "South: " + Gamepad.current[GamepadButton.South].isPressed + Environment.NewLine +
                                      "East: " + Gamepad.current[GamepadButton.East].isPressed + Environment.NewLine +
                                      "West: " + Gamepad.current[GamepadButton.West].isPressed + Environment.NewLine;
            Controller_LeftStick.text = "Left Stick BTN: " + Gamepad.current[GamepadButton.LeftStick].isPressed + Environment.NewLine +
                                        "Left Stick X: " + Gamepad.current.leftStick.x.ReadValue() + Environment.NewLine +
                                        "Left Stick Y: " + Gamepad.current.leftStick.y.ReadValue() + Environment.NewLine +
                                        "Left Stick UP: " + Gamepad.current.leftStick.up.ReadValue() + Environment.NewLine +
                                        "Left Stick Down: " + Gamepad.current.leftStick.down.ReadValue() + Environment.NewLine +
                                        "Left Stick Left: " + Gamepad.current.leftStick.left.ReadValue() + Environment.NewLine +
                                        "Left Stick Right: " + Gamepad.current.leftStick.right.ReadValue() + Environment.NewLine;
            Controller_RightStick.text = "Right Stick BTN: " + Gamepad.current[GamepadButton.RightStick].isPressed + Environment.NewLine +
                                         "Right Stick X: " + Gamepad.current.rightStick.x.ReadValue() + Environment.NewLine +
                                         "Right Stick Y: " + Gamepad.current.rightStick.y.ReadValue() + Environment.NewLine +
                                         "Right Stick UP: " + Gamepad.current.rightStick.up.ReadValue() + Environment.NewLine +
                                         "Right Stick Down: " + Gamepad.current.rightStick.down.ReadValue() + Environment.NewLine +
                                         "Right Stick Left: " + Gamepad.current.rightStick.left.ReadValue() + Environment.NewLine +
                                         "Right Stick Right: " + Gamepad.current.rightStick.right.ReadValue() + Environment.NewLine;
            Controller_Shoulder_Trigger.text = "Left Shoulder: " + Gamepad.current[GamepadButton.LeftShoulder].isPressed + Environment.NewLine +
                                               "Right Shoulder: " + Gamepad.current[GamepadButton.RightShoulder].isPressed + Environment.NewLine +
                                               "Left Trigger: " + Gamepad.current.leftTrigger.ReadValue() + Environment.NewLine +
                                               "Right Trigger: " + Gamepad.current.rightTrigger.ReadValue() + Environment.NewLine +
                                               "Start: " + Gamepad.current[GamepadButton.Start].isPressed + Environment.NewLine +
                                               "Select: " + Gamepad.current[GamepadButton.Select].isPressed + Environment.NewLine;

            if (Gamepad.current.device.name.Contains("DualShockGamepadHID")) { Controller_Device_Name.text = "Sony Playstation DualShock"; }
            else
            if (Gamepad.current.device.name.Contains("XInputControllerWindows")) { Controller_Device_Name.text = "Xbox(Any Controller with XInput)"; }
            else
            if (Gamepad.current.device.name.Contains("Nintendo Wireless Gamepad")) { Controller_Device_Name.text = "Nintendo Switch Joycon/Pro"; }
        }
        else
        {
            Information.text = "NULL";
            Controller_Dpad.text = "NULL";
            Controller_Buttons.text = "NULL";
            Controller_LeftStick.text = "NULL";
            Controller_RightStick.text = "NULL";
            Controller_Shoulder_Trigger.text = "NULL";
            Controller_Device_Name.text = "NULL";
        }

        InputSystemUpdate();
    }

    void InputSystemUpdate()
    {
        Input_System.text = "Devices.Count: " + InputSystem.devices.Count.ToString() + Environment.NewLine +
                            "DisconnectedDevices.Count: " + InputSystem.disconnectedDevices.Count.ToString();
        //for (int i = 0; i < InputSystem.devices.Count; i++)
        //{
        //    Debug.Log(InputSystem.devices[i].layout);
        //}
        InputSystem.Update();
    }
}
