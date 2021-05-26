using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Controls : MonoBehaviour
{
    public Text rightInput;
    public Button rightButton;
    public InputActionReference rightAction;

    public Text leftInput;
    public Button leftButton;
    public InputActionReference leftAction;

    public Text jumpInput;
    public Button jumpButton;
    public InputActionReference jumpAction;

    public Text slideInput;
    public Button slideButton;
    public InputActionReference slideAction;

    public Text useInput;
    public Button useButton;
    public InputActionReference useAction;

    public Text acelerateInput;
    public Button acelerateButton;
    public InputActionReference acelerateAction;

    public Text attackInput;
    public Button attackButton;
    public InputActionReference attackAction;

    public Text inventoryInput;
    public Button inventoryButton;
    public InputActionReference inventoryAction;

    public Text pauseInput;
    public Button pauseButton;
    public InputActionReference pauseAction;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public PlayerInput playerInput;
    public string rebinds;

    public void Awake()
    {
        rebinds = loadData();

        playerInput.actions.Disable();
    }

    public void Start()
    {
        if (string.IsNullOrEmpty(rebinds)) { return; }

        playerInput.actions.LoadBindingOverridesFromJson(rebinds);

        rightInput.text = InputControlPath.ToHumanReadableString(
            rightAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        leftInput.text = InputControlPath.ToHumanReadableString(
            leftAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        jumpInput.text = InputControlPath.ToHumanReadableString(
            jumpAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        slideInput.text = InputControlPath.ToHumanReadableString(
            slideAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        useInput.text = InputControlPath.ToHumanReadableString(
            useAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        acelerateInput.text = InputControlPath.ToHumanReadableString(
            acelerateAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        attackInput.text = InputControlPath.ToHumanReadableString(
            attackAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        inventoryInput.text = InputControlPath.ToHumanReadableString(
            inventoryAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        pauseInput.text = InputControlPath.ToHumanReadableString(
            pauseAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

    }


    public void menu()
    {
        rebinds = playerInput.actions.SaveBindingOverridesAsJson();

        saveData(rebinds);

        SceneManager.LoadScene("Menu");
    }

    public void saveData(string rebinds)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/controls.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, rebinds);
        stream.Close();
    }

    public string loadData()
    {
        string res = "";
        string path = Application.persistentDataPath + "/controls.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            res = (string)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }

        return res;
    }

    public void startRebindingRight()
    {
        rightInput.text = "";
        rightButton.interactable = false;

        rebindingOperation = rightAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompleteRight())
            .Start();
    }
    
    private void rebindCompleteRight()
    { 
        rightInput.text = InputControlPath.ToHumanReadableString(
            rightAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        rebindingOperation.Dispose();
        rightButton.interactable = true;
    }

    public void startRebindingLeft()
    {
        leftInput.text = "";
        leftButton.interactable = false;

        rebindingOperation = leftAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompleteLeft())
            .Start();
    }

    private void rebindCompleteLeft()
    {
        rebindingOperation.Dispose();
        leftInput.text = InputControlPath.ToHumanReadableString(
            leftAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        leftButton.interactable = true;
    }

    public void startRebindingJump()
    {
        jumpInput.text = "";
        jumpButton.interactable = false;

        rebindingOperation = jumpAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompleteJump())
            .Start();
    }

    private void rebindCompleteJump()
    {
        rebindingOperation.Dispose();
        jumpInput.text = InputControlPath.ToHumanReadableString(
            jumpAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        jumpButton.interactable = true;
    }

    public void startRebindingSlide()
    {
        slideInput.text = "";
        slideButton.interactable = false;

        rebindingOperation = slideAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompleteSlide())
            .Start();
    }

    private void rebindCompleteSlide()
    {
        rebindingOperation.Dispose();
        slideInput.text = InputControlPath.ToHumanReadableString(
            slideAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        slideButton.interactable = true;
    }

    public void startRebindingUse()
    {
        useInput.text = "";
        useButton.interactable = false;

        rebindingOperation = useAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompleteUse())
            .Start();
    }

    private void rebindCompleteUse()
    {
        rebindingOperation.Dispose();
        useInput.text = InputControlPath.ToHumanReadableString(
            useAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        useButton.interactable = true;
    }

    public void startRebindingAcelerate()
    {
        acelerateInput.text = "";
        acelerateButton.interactable = false;

        rebindingOperation = acelerateAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompleteAcelerate())
            .Start();
    }

    private void rebindCompleteAcelerate()
    {
        rebindingOperation.Dispose();
        acelerateInput.text = InputControlPath.ToHumanReadableString(
            acelerateAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        acelerateButton.interactable = true;
    }

    public void startRebindingAttack()
    {
        attackInput.text = "";
        attackButton.interactable = false;

        rebindingOperation = attackAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompleteAttack())
            .Start();
    }

    private void rebindCompleteAttack()
    {
        rebindingOperation.Dispose();
        attackInput.text = InputControlPath.ToHumanReadableString(
            attackAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        attackButton.interactable = true;
    }

    public void startRebindingInventory()
    {
        inventoryInput.text = "";
        inventoryButton.interactable = false;

        rebindingOperation = inventoryAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompleteInventory())
            .Start();
    }

    private void rebindCompleteInventory()
    {
        rebindingOperation.Dispose();
        inventoryInput.text = InputControlPath.ToHumanReadableString(
            inventoryAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        inventoryButton.interactable = true;
    }

    public void startRebindingPause()
    {
        pauseInput.text = "";
        pauseButton.interactable = false;

        rebindingOperation = pauseAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindCompletePause())
            .Start();
    }

    private void rebindCompletePause()
    {
        rebindingOperation.Dispose();
        pauseInput.text = InputControlPath.ToHumanReadableString(
            pauseAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        pauseButton.interactable = true;
    }
}
