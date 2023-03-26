using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerDemo : MonoBehaviour
{
    public PlayerInput playerInput;

    public TextMeshProUGUI actionMapText;
    public TextMeshProUGUI controlSchemeText;
    public TextMeshProUGUI playerIndexText;

    public TextMeshProUGUI movementText;
    public TextMeshProUGUI abilitiesText;
    public TextMeshProUGUI abilitiesTwoText;
    public TextMeshProUGUI abilitiesThreeText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI knockbackText;
    public TextMeshProUGUI pauseText;


    private void Start()
    {
        UpdatePlayerInputInfo();

    }

    private void UpdatePlayerInputInfo()
    {
        playerIndexText.text = "Player Index: " + playerInput.playerIndex;
        controlSchemeText.text = "Control Scheme: " + playerInput.currentControlScheme;
        actionMapText.text = "Action Map: " + playerInput.currentActionMap.name;
    }

    private void OnControlsChanged(PlayerInput playerInput)
    {
        UpdatePlayerInputInfo();
    }

    private void OnDeviceLost(PlayerInput playerInput)
    {
        UpdatePlayerInputInfo();
    }
    private void OnDeviceRegained(PlayerInput playerInput)
    {
        UpdatePlayerInputInfo();
    }

    private void OnMovement(InputValue value)
    {
        Vector2 moveValue = value.Get<Vector2>();
        Debug.Log("Movement: " + moveValue);
        movementText.text = "Movement: " + moveValue.ToString();

    }

    private void OnDash(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Dash: " + isPressed);
        dashText.text = "Dash: " + isPressed.ToString();
        if (isPressed == false)
            StartCoroutine(ClearText(dashText));

    }

    private void OnKnockback(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Knockback: " + isPressed);
        knockbackText.text = "Knockback: " + isPressed.ToString();
        if (isPressed == false)
            StartCoroutine(ClearText(knockbackText));

    }

    private void OnSkillOne(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Skill One: " + isPressed);
        abilitiesText.text = "Skill One: " + isPressed.ToString();

        if (isPressed == false)
            StartCoroutine(ClearText(abilitiesText));
    }

    private void OnSkillTwo(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Skill Two: " + isPressed);
        abilitiesTwoText.text = "Skill Two: " + isPressed.ToString();

        if (isPressed == false)
            StartCoroutine(ClearText(abilitiesTwoText));
    }

    private void OnSkillThree(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Skill Three: " + isPressed);
        abilitiesThreeText.text = "Skill Three: " + isPressed.ToString();

        if (isPressed == false)
            StartCoroutine(ClearText(abilitiesThreeText));
    }
    private void OnPause(InputValue value)
    {
        bool isPressed = value.isPressed;
        Debug.Log("Pause: " + isPressed);
        pauseText.text = "Pause: " + isPressed.ToString();

        StartCoroutine(ClearText(pauseText));
    }


    IEnumerator ClearText(TextMeshProUGUI textPanel)
    {
        yield return new WaitForSeconds(1);
        string[] newText = textPanel.text.Split(':');
        textPanel.text = newText[0];
    }
}
