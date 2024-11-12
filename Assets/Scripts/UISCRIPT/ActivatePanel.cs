using UnityEngine;

public class ActivatePhonePanel : MonoBehaviour
{
    public GameObject phonePanel;

    public void TogglePhonePanel()
    {
        // Toggle the phone panel's active state
        phonePanel.SetActive(!phonePanel.activeSelf);
    }

    
}
