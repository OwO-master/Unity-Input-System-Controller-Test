using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.LowLevel;
using UnityEngine.UI;

public class XBOX : MonoBehaviour
{
    public RawImage B_BTN;
    public RawImage A_BTN;
    public RawImage Y_BTN;
    public RawImage X_BTN;
    public RawImage Up;
    public RawImage Down;
    public RawImage Left;
    public RawImage Right;
    public RawImage LeftStickBTN;
    public GameObject LeftStick;
    public RawImage LeftShoulder;
    public RawImage LeftTrigger;
    public TextMeshProUGUI LeftTriggerText;
    public RawImage RightStickBTN;
    public GameObject RightStick;
    public RawImage RightShoulder;
    public RawImage RightTrigger;
    public TextMeshProUGUI RightTriggerText;
    public RawImage StartBTN;
    public RawImage SelectBTN;

    ControllerTest CT = new ControllerTest();

    float LeftStickHalf;
    float RightStickHalf;

    Color Press = new Color(1f, 0, 0, 0.5f);
    Color NotPress = new Color(0, 0, 0, 0.5f);

    private void Start()
    {
        CT = GameObject.FindWithTag("CT").GetComponent<ControllerTest>();
        LeftStickHalf = LeftStickBTN.rectTransform.sizeDelta.x / 2;
        RightStickHalf = RightStickBTN.rectTransform.sizeDelta.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all[CT.Controller_Dropdown.value] != null)
        {
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.Circle].isPressed) { B_BTN.color = Press; } else { B_BTN.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.Cross].isPressed) { A_BTN.color = Press; } else { A_BTN.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.Triangle].isPressed) { Y_BTN.color = Press; } else { Y_BTN.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.Square].isPressed) { X_BTN.color = Press; } else { X_BTN.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.DpadUp].isPressed) { Up.color = Press; } else { Up.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.DpadDown].isPressed) { Down.color = Press; } else { Down.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.DpadLeft].isPressed) { Left.color = Press; } else { Left.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.DpadRight].isPressed) { Right.color = Press; } else { Right.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.LeftStick].isPressed) { LeftStickBTN.color = Press; } else { LeftStickBTN.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.LeftShoulder].isPressed) { LeftShoulder.color = Press; } else { LeftShoulder.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.RightStick].isPressed) { RightStickBTN.color = Press; } else { RightStickBTN.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.RightShoulder].isPressed) { RightShoulder.color = Press; } else { RightShoulder.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.Start].isPressed) { StartBTN.color = Press; } else { StartBTN.color = NotPress; }
            if (Gamepad.all[CT.Controller_Dropdown.value][GamepadButton.Select].isPressed) { SelectBTN.color = Press; } else { SelectBTN.color = NotPress; }

            LeftStick.transform.localPosition = new Vector3(LeftStickHalf * Gamepad.all[CT.Controller_Dropdown.value].leftStick.x.ReadValue(), LeftStickHalf * Gamepad.all[CT.Controller_Dropdown.value].leftStick.y.ReadValue(), 0);
            LeftTrigger.color = new Color(Gamepad.all[CT.Controller_Dropdown.value].leftTrigger.ReadValue(), 0, 0, 0.5f);
            LeftTriggerText.text = Mathf.Round(Gamepad.all[CT.Controller_Dropdown.value].leftTrigger.ReadValue() * 100) + "%";

            RightStick.transform.localPosition = new Vector3(RightStickHalf * Gamepad.all[CT.Controller_Dropdown.value].rightStick.x.ReadValue(), RightStickHalf * Gamepad.all[CT.Controller_Dropdown.value].rightStick.y.ReadValue(), 0);
            RightTrigger.color = new Color(Gamepad.all[CT.Controller_Dropdown.value].rightTrigger.ReadValue(), 0, 0, 0.5f);
            RightTriggerText.text = Mathf.Round(Gamepad.all[CT.Controller_Dropdown.value].rightTrigger.ReadValue() * 100) + "%";
        }
    }
}
