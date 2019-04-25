# Controller-Test
Unity Requirement: 2019.1+</br>
Input System: 0.2.8</br>
My website: www.valxe.idv.tw

-----
Known Issue:
1. Can't set rumble (tested on DualShock 4)
2. If connect the controller using Bluetooth, not dongle (ie: DualShock 4 -> CUH-ZWA1G) or USB cable. Input System can't detect whether or not the controller is disconnected by power off.
3. The "Supported Devices" has some bug. If you add "PS4 Controller (on PS4)" as supported device, Input System will BLOCK the DualShock. (Maybe it must pass through by dongle CUH-ZWA1G?).
4. Input System can detect Joycon, but it doesn't detect as Gamepad(???)
5. runInBackground doesn't work. Unity window must be focus. (because direct access??)

-----
Bugs need to be fixed & Something I want to add.</br>
"Buttons" on the DualShock image are not stable if you connect 2 or more controllers.(2019.04.26)</br>
Add Xbox / Switch PRO image and will switch image by current select. (ADD)

-----
2019.04.26</br>
Multiple Controller Support</br>

2019.04.25</br>
Project Created!</br>
