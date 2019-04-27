# Controller-Test
Unity Requirement: 2019.1+</br>
Input System: 0.2.8</br>
by Valxe</br>
My website: www.valxe.idv.tw

-----
#### Download:
Unity Standalone: [v0.2](https://github.com/kculwpvalxe/Unity-Input-System-0.2.8-Controller-Test/releases)

-----
#### Known Issue:Known Issue:
1. Can't set rumble (tested on DualShock 4)
2. If connect the controller using Bluetooth, not dongle (ie: DualShock 4 -> CUH-ZWA1G) or USB cable. Input System can't detect whether or not the controller is disconnected by power off.
3. The "Supported Devices" has some bug. If you add "PS4 Controller (on PS4)" as supported device, Input System will BLOCK the DualShock. (Maybe it must pass through by dongle CUH-ZWA1G?).
4. Input System can detect Joycon, but it doesn't detect as Gamepad(???)
5. runInBackground doesn't work. Unity window must be focus. (because direct access??)

-----
#### Bugs need to be fixed & Something I want to add.

Add Switch PRO image and switch image by current select. (ADD)

-----
#### Update
###### 2019.04.28
Delete junk code</br>
Fix unstable indecator button</br>
Add Xbox controller indecator image</br>

###### 2019.04.26
Multiple Controller Support

###### 2019.04.25
Project Created!
