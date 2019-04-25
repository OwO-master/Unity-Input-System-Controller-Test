using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.LowLevel;
using UnityEngine.UI;

public class DualShock : MonoBehaviour
{
    public RawImage Circle;
    public RawImage Cross;
    public RawImage Triangle;
    public RawImage Square;
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

    float LeftStickHalf;
    float RightStickHalf;

    Color Press = new Color(1f, 0, 0, 0.5f);
    Color NotPress = new Color(0, 0, 0, 0.5f);

    private void Start()
    {
        LeftStickHalf = LeftStickBTN.rectTransform.sizeDelta.x / 2;
        RightStickHalf = RightStickBTN.rectTransform.sizeDelta.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            //"Controller 1" + Environment.NewLine +
            //"Display Name: " + Gamepad.current.displayName + Environment.NewLine +
            //"Name: " + Gamepad.current.name + Environment.NewLine +
            //"Short Display Name: " + Gamepad.current.shortDisplayName + Environment.NewLine +
            //"Description: " + Gamepad.current.description + Environment.NewLine +
            //"Device: " + Gamepad.current.device + Environment.NewLine +
            //"ID: " + Gamepad.current.id + Environment.NewLine +
            //"Layout: " + Gamepad.current.layout + Environment.NewLine +
            //"Native: " + Gamepad.current.native + Environment.NewLine +
            //"Noisy: " + Gamepad.current.noisy + Environment.NewLine +
            //"Path: " + Gamepad.current.path + Environment.NewLine +
            //"Remote: " + Gamepad.current.remote + Environment.NewLine +
            //"StateBlock: " + Gamepad.current.stateBlock + Environment.NewLine +
            //"Synthetic: " + Gamepad.current.synthetic + Environment.NewLine +
            //"Variants: " + Gamepad.current.variants;
            if (Gamepad.current[GamepadButton.Circle].isPressed) { Circle.color = Press; } else { Circle.color = NotPress; }
            if (Gamepad.current[GamepadButton.Cross].isPressed) { Cross.color = Press; } else { Cross.color = NotPress; }
            if (Gamepad.current[GamepadButton.Triangle].isPressed) { Triangle.color = Press; } else { Triangle.color = NotPress; }
            if (Gamepad.current[GamepadButton.Square].isPressed) { Square.color = Press; } else { Square.color = NotPress; }
            if (Gamepad.current[GamepadButton.DpadUp].isPressed) { Up.color = Press; } else { Up.color = NotPress; }
            if (Gamepad.current[GamepadButton.DpadDown].isPressed) { Down.color = Press; } else { Down.color = NotPress; }
            if (Gamepad.current[GamepadButton.DpadLeft].isPressed) { Left.color = Press; } else { Left.color = NotPress; }
            if (Gamepad.current[GamepadButton.DpadRight].isPressed) { Right.color = Press; } else { Right.color = NotPress; }
            if (Gamepad.current[GamepadButton.LeftStick].isPressed) { LeftStickBTN.color = Press; } else { LeftStickBTN.color = NotPress; }
            if (Gamepad.current[GamepadButton.LeftShoulder].isPressed) { LeftShoulder.color = Press; } else { LeftShoulder.color = NotPress; }
            if (Gamepad.current[GamepadButton.RightStick].isPressed) { RightStickBTN.color = Press; } else { RightStickBTN.color = NotPress; }
            if (Gamepad.current[GamepadButton.RightShoulder].isPressed) { RightShoulder.color = Press; } else { RightShoulder.color = NotPress; }
            if (Gamepad.current[GamepadButton.Start].isPressed) { StartBTN.color = Press; } else { StartBTN.color = NotPress; }
            if (Gamepad.current[GamepadButton.Select].isPressed) { SelectBTN.color = Press; } else { SelectBTN.color = NotPress; }

            LeftStick.transform.localPosition = new Vector3(LeftStickHalf * Gamepad.current.leftStick.x.ReadValue(), LeftStickHalf * Gamepad.current.leftStick.y.ReadValue(), 0);
            LeftTrigger.color = new Color(Gamepad.current.leftTrigger.ReadValue(), 0, 0, 0.5f);
            LeftTriggerText.text = Mathf.Round(Gamepad.current.leftTrigger.ReadValue() * 100) + "%";

            RightStick.transform.localPosition = new Vector3(RightStickHalf * Gamepad.current.rightStick.x.ReadValue(), RightStickHalf * Gamepad.current.rightStick.y.ReadValue(), 0);
            RightTrigger.color = new Color(Gamepad.current.rightTrigger.ReadValue(), 0, 0, 0.5f);
            RightTriggerText.text = Mathf.Round(Gamepad.current.rightTrigger.ReadValue() * 100) + "%";
        }
    }
}
