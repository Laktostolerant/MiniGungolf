using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserPreferences
{
    // User Defined Keybinds.
    public static InputAction Button_Switch_Previous;
    public static InputAction Button_Switch_Next;
    public static InputAction Button_Shoot;

    // User Defined Settings.
    public static float Rotation_Sensitivity;

    // Initialisation.
    public static void Initialise()
    {
        // This function doesn't need to be used yet, it's arbitary.

        Rotation_Sensitivity = 120.0f;
    }
}
