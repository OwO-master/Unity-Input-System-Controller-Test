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
    public TextMeshProUGUI Controller_Devices_List;
    public TMP_Dropdown Controller_Dropdown;
    List<Gamepad> Devices_List = new List<Gamepad>();

    [Header("Controllers")]
    public GameObject DualShock_Controller;
    public GameObject XBOX_Controller;

    private void Awake()
    {
        Application.runInBackground = true;
    }

    void Update()
    {
        if (Gamepad.all[Controller_Dropdown.value] != null)
        {
            Information.text = "Controller 1" + Environment.NewLine +
                               "Display Name: " + Gamepad.all[Controller_Dropdown.value].displayName + Environment.NewLine +
                               "Name: " + Gamepad.all[Controller_Dropdown.value].name + Environment.NewLine +
                               "Short Display Name: " + Gamepad.all[Controller_Dropdown.value].shortDisplayName + Environment.NewLine +
                               "Description: " + Gamepad.all[Controller_Dropdown.value].description + Environment.NewLine +
                               "Device: " + Gamepad.all[Controller_Dropdown.value].device + Environment.NewLine +
                               "ID: " + Gamepad.all[Controller_Dropdown.value].id + Environment.NewLine +
                               "Layout: " + Gamepad.all[Controller_Dropdown.value].layout + Environment.NewLine +
                               "Native: " + Gamepad.all[Controller_Dropdown.value].native + Environment.NewLine +
                               "Noisy: " + Gamepad.all[Controller_Dropdown.value].noisy + Environment.NewLine +
                               "Path: " + Gamepad.all[Controller_Dropdown.value].path + Environment.NewLine +
                               "Remote: " + Gamepad.all[Controller_Dropdown.value].remote + Environment.NewLine +
                               "Synthetic: " + Gamepad.all[Controller_Dropdown.value].synthetic + Environment.NewLine +
                               "Variants: " + Gamepad.all[Controller_Dropdown.value].variants;
            Controller_Dpad.text = "UP: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.DpadUp].isPressed + Environment.NewLine +
                                   "Down: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.DpadDown].isPressed + Environment.NewLine +
                                   "Left: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.DpadLeft].isPressed + Environment.NewLine +
                                   "Right: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.DpadRight].isPressed + Environment.NewLine;
            Controller_Buttons.text = "A: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.A].isPressed + Environment.NewLine +
                                      "B: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.B].isPressed + Environment.NewLine +
                                      "X: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.X].isPressed + Environment.NewLine +
                                      "Y: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.Y].isPressed + Environment.NewLine +
                                      "O: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.Circle].isPressed + Environment.NewLine +
                                      "X: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.Cross].isPressed + Environment.NewLine +
                                      "/_\\: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.Triangle].isPressed + Environment.NewLine +
                                      "[ ]: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.Square].isPressed + Environment.NewLine +
                                      "North: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.North].isPressed + Environment.NewLine +
                                      "South: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.South].isPressed + Environment.NewLine +
                                      "East: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.East].isPressed + Environment.NewLine +
                                      "West: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.West].isPressed + Environment.NewLine;
            Controller_LeftStick.text = "Left Stick BTN: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.LeftStick].isPressed + Environment.NewLine +
                                        "Left Stick X: " + Gamepad.all[Controller_Dropdown.value].leftStick.x.ReadValue() + Environment.NewLine +
                                        "Left Stick Y: " + Gamepad.all[Controller_Dropdown.value].leftStick.y.ReadValue() + Environment.NewLine +
                                        "Left Stick UP: " + Gamepad.all[Controller_Dropdown.value].leftStick.up.ReadValue() + Environment.NewLine +
                                        "Left Stick Down: " + Gamepad.all[Controller_Dropdown.value].leftStick.down.ReadValue() + Environment.NewLine +
                                        "Left Stick Left: " + Gamepad.all[Controller_Dropdown.value].leftStick.left.ReadValue() + Environment.NewLine +
                                        "Left Stick Right: " + Gamepad.all[Controller_Dropdown.value].leftStick.right.ReadValue() + Environment.NewLine;
            Controller_RightStick.text = "Right Stick BTN: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.RightStick].isPressed + Environment.NewLine +
                                         "Right Stick X: " + Gamepad.all[Controller_Dropdown.value].rightStick.x.ReadValue() + Environment.NewLine +
                                         "Right Stick Y: " + Gamepad.all[Controller_Dropdown.value].rightStick.y.ReadValue() + Environment.NewLine +
                                         "Right Stick UP: " + Gamepad.all[Controller_Dropdown.value].rightStick.up.ReadValue() + Environment.NewLine +
                                         "Right Stick Down: " + Gamepad.all[Controller_Dropdown.value].rightStick.down.ReadValue() + Environment.NewLine +
                                         "Right Stick Left: " + Gamepad.all[Controller_Dropdown.value].rightStick.left.ReadValue() + Environment.NewLine +
                                         "Right Stick Right: " + Gamepad.all[Controller_Dropdown.value].rightStick.right.ReadValue() + Environment.NewLine;
            Controller_Shoulder_Trigger.text = "Left Shoulder: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.LeftShoulder].isPressed + Environment.NewLine +
                                               "Right Shoulder: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.RightShoulder].isPressed + Environment.NewLine +
                                               "Left Trigger: " + Gamepad.all[Controller_Dropdown.value].leftTrigger.ReadValue() + Environment.NewLine +
                                               "Right Trigger: " + Gamepad.all[Controller_Dropdown.value].rightTrigger.ReadValue() + Environment.NewLine +
                                               "Start: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.Start].isPressed + Environment.NewLine +
                                               "Select: " + Gamepad.all[Controller_Dropdown.value][GamepadButton.Select].isPressed + Environment.NewLine;

            if (Gamepad.all[Controller_Dropdown.value].device.name.Contains("DualShockGamepadHID"))
            {
                Controller_Device_Name.text = "Sony Playstation DualShock";
                DualShock_Controller.SetActive(true);
                XBOX_Controller.SetActive(false);
            }
            else
            if (Gamepad.all[Controller_Dropdown.value].device.name.Contains("XInputControllerWindows"))
            {
                Controller_Device_Name.text = "Xbox(Any Controller with XInput)";
                DualShock_Controller.SetActive(false);
                XBOX_Controller.SetActive(true);
            }
            else
            if (Gamepad.all[Controller_Dropdown.value].device.name.Contains("Nintendo Wireless Gamepad"))
            {
                Controller_Device_Name.text = "Nintendo Switch Joycon/Pro";
                DualShock_Controller.SetActive(false);
                XBOX_Controller.SetActive(false);
            }
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

        Devices_List.RemoveRange(0, Devices_List.Count);
        Controller_Dropdown.options.RemoveRange(0, Controller_Dropdown.options.Count);
        Controller_Devices_List.text = "";
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            Devices_List.Add(Gamepad.all[i]);
            Controller_Devices_List.text += Gamepad.all[i].id + ": " + Gamepad.all[i].name + Environment.NewLine;
            TMP_Dropdown.OptionData temp = new TMP_Dropdown.OptionData(Gamepad.all[i].id.ToString());
            Controller_Dropdown.options.Add(temp);
        }
        Controller_Dropdown.RefreshShownValue();

        InputSystem.pollingFrequency = 120;
        InputSystem.Update();
    }
}
