using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class ARMaintenance : MonoBehaviour
{
    // three maintenance types with their respective time limits
    public enum MaintenanceType
    {
        Type1 = 180,
        Type2 = 240,
        Type3 = 300
    }

    // variables for storing the start time and selected maintenance type
    private float startTime;
    private MaintenanceType selectedType;

    // reference variables for the maintenance type images and the timer text
    public GameObject maintenanceTypeUI;
    public Image[] maintenanceTypeImages;
    public Text timerText;

    // variable for storing the success/failure message
    public Text messageText;

    private string successMessage = "Maintenance completed successfully!";
    private string failureMessage = "Maintenance time exceeded the limit!";
    public GameObject maintenanceInstructionsUI;
    public Text instructionsText;
    public Button completeButton;

    // Use the Start method to initialize the AR session and show the maintenance type UI
   private void Start()
    {

        // Start the AR session
        ARCoreSessionConfig sessionConfig = new ARCoreSessionConfig();
        sessionConfig.LightEstimationMode = LightEstimationMode.AmbientIntensity;
        ARSession.Create(sessionConfig);

        ShowMaintenanceTypeUI();
    }

    // Use the Update method to update the timer text
    private void Update()
    {
        // Check if a maintenance type has been selected
        if (selectedType != 0)
        {
            float elapsedTime = Time.time - startTime;
            timerText.text = "Time: " + (int)elapsedTime + "s";

            // Check if the elapsed time is greater than or equal to the selected maintenance type's time limit
            if (elapsedTime >= (int)selectedType)
            {
                messageText.text = failureMessage;
                completeButton.interactable = false;
            }
        }
    }

    private void ShowMaintenanceTypeUI()
    {
        maintenanceTypeUI.SetActive(true);

        for (int i = 0; i < maintenanceTypeImages.Length; i++)
        {
            // Show the maintenance type image
            maintenanceTypeImages[i].gameObject.SetActive(true);

            int index = i;
            maintenanceTypeImages[i].GetComponent<Button>().onClick.AddListener(() => SelectMaintenanceType(index));
        }
    }


    private void SelectMaintenanceType(int index)
    {
        // Get the selected maintenance type from the index
        selectedType = (MaintenanceType)index;

        // Start the maintenance by setting the start time
        startTime = Time.time;

        maintenanceTypeUI.SetActive(false);
        for (int i = 0; i < maintenanceTypeImages.Length; i++)
        {
            maintenanceTypeImages[i].gameObject.SetActive(false);
        }

        timerText.gameObject.SetActive(true);
        messageText.gameObject.SetActive(true);
    }

    public void CompleteMaintenance()
    {
        float elapsedTime = Time.time - startTime;

        // Check if the elapsed time is less than the selected maintenance type's time limit
        if (elapsedTime < (int)selectedType)
        {
            messageText.text = successMessage;
        }
        else
        {
            messageText.text = failureMessage;
        }

        timerText.gameObject.SetActive(false);
    }

    public void ResetMaintenance()
    {
        // Reset the selected maintenance type
        selectedType = 0;

        messageText.gameObject.SetActive(false);
        ShowMaintenanceTypeUI();
    }
}