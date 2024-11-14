using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private Button questIconButton;
    [SerializeField] private Text questTitleText;
    [SerializeField] private Text questDescriptionText;
    [SerializeField] private Image[] fragmentBoxes; // Array for fragment progress boxes

    private int fragmentsCollected = 0; // Tracks collected fragments
    private const int totalFragments = 3; // Total fragments to collect

    private void Start()
    {
        // Ensure the quest panel is initially hidden
        questPanel.SetActive(false);

        // Set initial quest display
        questTitleText.text = "Quest 1: Collect Relic Fragments";
        questDescriptionText.text = "Collect 3 relic fragments to create a cure for the zombie virus.";

        // Initialize fragment progress boxes as unchecked
        foreach (Image box in fragmentBoxes)
        {
            box.color = Color.red; // Use red to indicate not collected
        }

        // Add a listener to the quest icon button
        questIconButton.onClick.AddListener(ToggleQuestPanel);
    }

    private void ToggleQuestPanel()
    {
        // Toggle the visibility of the quest panel
        questPanel.SetActive(!questPanel.activeSelf);
    }

    // Call this method when a fragment is collected
    public void CollectFragment()
    {
        if (fragmentsCollected < totalFragments)
        {
            fragmentsCollected++;
            fragmentBoxes[fragmentsCollected - 1].color = Color.green; // Mark fragment as collected
        }

        // Check if quest is complete
        if (fragmentsCollected == totalFragments)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        questDescriptionText.text = "Quest 1 Complete!";
        // Here, you could add code to signal Quest 2 to begin (e.g., set a flag or call another script)
        
    }
}