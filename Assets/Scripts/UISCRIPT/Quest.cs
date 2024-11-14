using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private Button questIconButton;

    private void Start()
    {
        // Ensure the quest panel is initially hidden
        questPanel.SetActive(false);

        // Add a listener to the quest icon button
        questIconButton.onClick.AddListener(ToggleQuestPanel);
    }

    private void ToggleQuestPanel()
    {
        // Toggle the visibility of the quest panel
        questPanel.SetActive(!questPanel.activeSelf);
    }
}