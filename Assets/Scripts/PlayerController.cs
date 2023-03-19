using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;


    private void OnControlsChanged(PlayerInput newPlayerInput)
    {
        Debug.Log("Controls Changed : " + newPlayerInput.currentControlScheme);
    }
    private void OnDeviceLost(PlayerInput newPlayerInput)
    {
        Debug.Log("Device Lost : " + newPlayerInput.devices);
    }
    private void OnDeviceRegained(PlayerInput newPlayerInput)
    {
        Debug.Log("Device Regained : " + newPlayerInput.devices);

    }

    private void OnMovement(InputValue value)
    {
        Vector2 moveValue = value.Get<Vector2>();
        Debug.Log("Movement: " + moveValue);

    }

    private void OnDash(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Dash: " + isPressed);

    }

    private void OnKnockback(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Knockback: " + isPressed);

    }

    private void OnSkillOne(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Skill One: " + isPressed);
    }

    private void OnSkillTwo(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Skill Two: " + isPressed);
    }

    private void OnSkillThree(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Skill Three: " + isPressed);
    }
    private void OnPause(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Pause: " + isPressed);
    }

    
}
