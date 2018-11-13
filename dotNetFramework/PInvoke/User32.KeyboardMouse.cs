﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XJK.PInvoke
{
    public static partial class User32
    {
        [DllImport("user32.dll")]
        public static extern bool BlockInput(bool fBlockIt);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        /// <summary>
        /// Defines a system-wide hot key.
        /// </summary>
        /// <param name="hWnd">A handle to the window that will receive WM_HOTKEY messages generated by the hot key. 
        /// If this parameter is NULL, WM_HOTKEY messages are posted to the message queue of the calling thread and must be processed in the message loop.</param>
        /// <param name="id">The identifier of the hot key. If the hWnd parameter is NULL, then the hot key is associated with the current thread rather than with a particular window. 
        /// If a hot key already exists with the same hWnd and id parameters, see Remarks for the action taken.</param>
        /// <param name="fsModifiers">The keys that must be pressed in combination with the key specified by the uVirtKey parameter in order to generate the WM_HOTKEY message.</param>
        /// <param name="vk">The virtual-key code of the hot key.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, Modifiers fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Copies the status of the 256 virtual keys to the specified buffer.
        /// </summary>
        /// <param name="lpKeyState">The 256-byte array that receives the status data for each virtual key.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// Copies an array of keyboard key states into the calling thread's keyboard input-state table. This is the same table accessed by the GetKeyboardState and GetKeyState functions. 
        /// Changes made to this table do not affect keyboard input to any other thread.
        /// </summary>
        /// <param name="lpKeyState">A pointer to a 256-byte array that contains keyboard key states.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll")]
        public static extern bool SetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// Retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled (on, off—alternating each time the key is pressed).
        /// </summary>
        /// <param name="nVirtKey">
        /// A virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9), nVirtKey must be set to the ASCII value of that character. 
        /// For other keys, it must be a virtual-key code.
        /// If a non-English keyboard layout is used, virtual keys with values in the range ASCII A through Z and 0 through 9 are used to specify most of the character keys.
        /// For example, for the German keyboard layout, the virtual key of value ASCII O(0x4F) refers to the "o" key, whereas VK_OEM_1 refers to the "o with umlaut" key.
        /// </param>
        /// <returns>
        /// The return value specifies the status of the specified virtual key, as follows:
        /// If the high-order bit is 1, the key is down; otherwise, it is up.
        /// If the low-order bit is 1, the key is toggled.A key, such as the CAPS LOCK key, is toggled if it is turned on.
        /// The key is off and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
        /// </returns>
        /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getkeystate</remarks>
        [DllImport("USER32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        /// <summary>
        /// Determines whether a key is up or down at the time the function is called, and whether the key was pressed after a previous call to GetAsyncKeyState.
        /// </summary>
        /// <param name="vKey">The virtual-key code. For more information, see Virtual Key Codes.
        /// You can use left- and right-distinguishing constants to specify certain keys.See the Remarks section for further information.</param>
        /// <returns>
        /// If the function succeeds, the return value specifies whether the key was pressed since the last call to GetAsyncKeyState, and whether the key is currently up or down. 
        /// If the most significant bit is set, the key is down, and if the least significant bit is set, the key was pressed after the previous call to GetAsyncKeyState. 
        /// However, you should not rely on this last behavior; for more information, see the Remarks.
        /// The return value is zero for the following cases:
		/// The current desktop is not the active desktop
		/// The foreground thread belongs to another process and the desktop does not allow the hook or the journal record.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKeyA(uint uCode, MapVirtualKeyMapTypes uMapType);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKeyExA(uint uCode, MapVirtualKeyMapTypes uMapType, IntPtr dwhkl);

        /// <summary>
        /// The ToAscii function translates the specified virtual-key code and keyboard 
        /// state to the corresponding character or characters. The function translates the code 
        /// using the input language and physical keyboard layout identified by the keyboard layout handle.
        /// </summary>
        /// <param name="uVirtKey">
        /// [in] Specifies the virtual-key code to be translated. 
        /// </param>
        /// <param name="uScanCode">
        /// [in] Specifies the hardware scan code of the key to be translated. 
        /// The high-order bit of this value is set if the key is up (not pressed). 
        /// </param>
        /// <param name="lpbKeyState">
        /// [in] Pointer to a 256-byte array that contains the current keyboard state. 
        /// Each element (byte) in the array contains the state of one key. 
        /// If the high-order bit of a byte is set, the key is down (pressed). 
        /// The low bit, if set, indicates that the key is toggled on. In this function, 
        /// only the toggle bit of the CAPS LOCK key is relevant. The toggle state 
        /// of the NUM LOCK and SCROLL LOCK keys is ignored.
        /// </param>
        /// <param name="lpwTransKey">
        /// [out] Pointer to the buffer that receives the translated character or characters. 
        /// </param>
        /// <param name="fuState">
        /// [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise. 
        /// </param>
        /// <returns>
        /// If the specified key is a dead key, the return value is negative. Otherwise, it is one of the following values. 
        /// Value Meaning 
        /// 0 The specified virtual key has no translation for the current state of the keyboard. 
        /// 1 One character was copied to the buffer. 
        /// 2 Two characters were copied to the buffer. This usually happens when a dead-key character 
        /// (accent or diacritic) stored in the keyboard layout cannot be composed with the specified 
        /// virtual key to form a single character. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
        /// </remarks>
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);
    }

    [Flags]
    public enum Modifiers : uint
    {
        None = 0,
        /// <summary>
        /// Either ALT key must be held down.
        /// </summary>
        ALT = 0x0001,
        /// <summary>
        /// Either CTRL key must be held down.
        /// </summary>
        CONTROL = 0x0002,
        /// <summary>
        /// Either SHIFT key must be held down.
        /// </summary>
        SHIFT = 0x0004,
        /// <summary>
        /// Either WINDOWS key was held down. These keys are labeled with the Windows logo. Keyboard shortcuts that involve the WINDOWS key are reserved for use by the operating system.
        /// </summary>
        WIN = 0x0008,
        /// <summary>
        /// Changes the hotkey behavior so that the keyboard auto-repeat does not yield multiple hotkey notifications.
        /// Windows Vista:  This flag is not supported.
        /// </summary>
        NOREPEAT = 0x4000,
    }

    public static class ModifiersExtension
    {
        public static int Count(this Modifiers modifiers)
        {
            int m = (int)modifiers;
            int sum = 0;
            for (int i = 0; i < 4; i++)
            {
                sum += (m >> i) & 1;
            }
            return sum;
        }

        public static IEnumerable<VirtualKeys> ToVirtualKeysList(this Modifiers modifiers, bool LeftSide = true)
        {
            var list = new List<VirtualKeys>();
            int offset = LeftSide ? 0 : 1;
            if (modifiers.HasFlag(Modifiers.ALT))
            {
                list.Add(VirtualKeys.LeftMenu + 1);
            }
            if (modifiers.HasFlag(Modifiers.CONTROL))
            {
                list.Add(VirtualKeys.LeftControl + 1);
            }
            if (modifiers.HasFlag(Modifiers.SHIFT))
            {
                list.Add(VirtualKeys.LeftShift+ 1);
            }
            if (modifiers.HasFlag(Modifiers.WIN))
            {
                list.Add(VirtualKeys.LeftWindows+ 1);
            }
            return list;
        }
    }
    
    /// <summary>
    /// The set of valid MapTypes used in MapVirtualKey
    /// </summary>
    public enum MapVirtualKeyMapTypes : uint
    {
        /// <summary>
        /// uCode is a virtual-key code and is translated into a scan code.
        /// If it is a virtual-key code that does not distinguish between left- and
        /// right-hand keys, the left-hand scan code is returned.
        /// If there is no translation, the function returns 0.
        /// </summary>
        MAPVK_VK_TO_VSC = 0x00,

        /// <summary>
        /// uCode is a scan code and is translated into a virtual-key code that
        /// does not distinguish between left- and right-hand keys. If there is no
        /// translation, the function returns 0.
        /// </summary>
        MAPVK_VSC_TO_VK = 0x01,

        /// <summary>
        /// uCode is a virtual-key code and is translated into an unshifted
        /// character value in the low-order word of the return value. Dead keys (diacritics)
        /// are indicated by setting the top bit of the return value. If there is no
        /// translation, the function returns 0.
        /// </summary>
        MAPVK_VK_TO_CHAR = 0x02,

        /// <summary>
        /// Windows NT/2000/XP: uCode is a scan code and is translated into a
        /// virtual-key code that distinguishes between left- and right-hand keys. If
        /// there is no translation, the function returns 0.
        /// </summary>
        MAPVK_VSC_TO_VK_EX = 0x03,

        /// <summary>
        /// Not currently documented
        /// </summary>
        MAPVK_VK_TO_VSC_EX = 0x04
    }

    /// <summary>
    /// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardLLHookStruct
    {
        /// <summary>
        /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
        /// </summary>
        public int VirtualKeyCode;
        /// <summary>
        /// Specifies a hardware scan code for the key. 
        /// </summary>
        public int ScanCode;
        /// <summary>
        /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
        /// </summary>
        public int Flags;
        /// <summary>
        /// Specifies the Time stamp for this message.
        /// </summary>
        public int Time;
        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public int ExtraInfo;
    }
    
    /// <summary>
    /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseLLHookStruct
    {
        /// <summary>
        /// Specifies a Point structure that contains the X- and Y-coordinates of the cursor, in screen coordinates. 
        /// </summary>
        public POINT Point;
        /// <summary>
        /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. 
        /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward, 
        /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user. 
        /// One wheel click is defined as WHEEL_DELTA, which is 120. 
        ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
        /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
        /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, MouseData is not used. 
        ///XBUTTON1
        ///The first X button was pressed or released.
        ///XBUTTON2
        ///The second X button was pressed or released.
        /// </summary>
        public int MouseData;
        /// <summary>
        /// Specifies the event-injected flag. An application can use the following value to test the mouse Flags. Value Purpose 
        ///LLMHF_INJECTED Test the event-injected flag.  
        ///0
        ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
        ///1-15
        ///Reserved.
        /// </summary>
        public int Flags;
        /// <summary>
        /// Specifies the Time stamp for this message.
        /// </summary>
        public int Time;
        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public int ExtraInfo;
    }
}