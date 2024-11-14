using UnityEngine;

public class ActivatePanels : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject mainMenuPanel;

    // Method to open the options panel and hide the main menu panel
    public void ShowOptionsPanel()
    {
        optionsPanel.SetActive(true);       // Activate options panel
        mainMenuPanel.SetActive(false);     // Deactivate main menu panel
    }

    // Method to go back to the main menu, hiding the options panel
    public void GoBackToMainMenu()
    {
        optionsPanel.SetActive(false);      // Deactivate options panel
        mainMenuPanel.SetActive(true);      // Activate main menu panel
    }
}