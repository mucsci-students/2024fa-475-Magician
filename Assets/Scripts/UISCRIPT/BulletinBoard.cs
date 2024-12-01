using UnityEngine;
using TMPro; // Use this if you're using TextMeshPro

public class BulletinBoard : MonoBehaviour
{
    [SerializeField] private GameObject bulletinBoardUI; // Reference to the bulletin board UI (Pop-Up Panel)
    [SerializeField] private GameObject storyScrollView; // Scroll View for the Story
    [SerializeField] private GameObject tutorialsScrollView; // Scroll View for the Tutorials
    [SerializeField] private GameObject tutorialsScrollView_2; // Scroll View for the Tutorials
    [SerializeField] private TMP_Text storyText;         // Reference to the Text component for the story
    [SerializeField] private TMP_Text tutorialsText;     // Reference to the Text component for the tutorials
    [SerializeField] private TMP_Text tutorialsText_2;     // Reference to the Text component for the tutorials

    private bool playerIsNear = false; // Track if the player is near the bulletin board

    public static bool isUsingBulletinBoard = false;

    void Start()
    {
        // Ensure the bulletin board UI and all scroll views are hidden at the start
        bulletinBoardUI.SetActive(false);
        storyScrollView.SetActive(false);
        tutorialsScrollView.SetActive(false);
    }

    void Update()
    {
        // Check if the player is near and presses the "E" key
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 0f;
            ButtonManager.GameIsPaused = true;
            ShowBulletinBoardUI();
            isUsingBulletinBoard = true;
        }
    }

    // Method to show the bulletin board UI
    private void ShowBulletinBoardUI()
    {
        bulletinBoardUI.SetActive(true); // Show the bulletin board UI
    }

    // Method to close the bulletin board UI
    public void CloseBulletinBoard()
    {
        bulletinBoardUI.SetActive(false);      // Hide the bulletin board UI
        storyScrollView.SetActive(false);      // Hide the story scroll view
        tutorialsScrollView.SetActive(false);  // Hide the tutorials scroll view
        Time.timeScale = 1f;                   // Resume the game
        ButtonManager.GameIsPaused = false;
        isUsingBulletinBoard = false;
    }

    // Method to display the story
    public void ShowStory()
    {
        storyScrollView.SetActive(true);       // Show the story scroll view
        tutorialsScrollView.SetActive(false);  // Hide the tutorials scroll view
        tutorialsScrollView_2.SetActive(false);
        storyText.text = "In a post-apocalyptic world ravaged by a deadly disease unleashed by a secretive government experiment, " +
        "humanity teeters on the brink of extinction. The virus, a result of a failed scientific endeavor, has turned people into mindless, " +
        "flesh-eating zombies, leaving only a few survivors struggling to stay alive. " +
        "You are one of these survivors, determined to uncover the truth behind the outbreak and put an end to the nightmare. " +
        "Your journey will lead you through desolate cities, abandoned government labs, and fortified research facilities underground, " +
        "where you’ll battle hordes of zombies and confront monstrous bosses. " +
        "Your ultimate mission is to find and defeat the final zombie boss—the former chief scientist who holds the secret to the cure but has succumbed to the disease himself. " +
        "Only by defeating him and recovering the cure can you save humanity from its impending doom.";

    }

    // Method to display the tutorials
    public void ShowTutorials()
    {
        tutorialsScrollView.SetActive(true);  // Show the tutorials scroll view
        tutorialsScrollView_2.SetActive(true);
        storyScrollView.SetActive(false);     // Hide the story scroll view
        // Set the main tutorials text
        tutorialsText.text = "Tutorials\n\n" +
            "Press ESC key to pause the game or click on the pause button on top right of the screen!\n\n" +
            "Movement\n" +
            "- Use WASD keys to move your character in different directions.\n\n" +
            "Shooting\n" +
            "- Use the Left Mouse Button to shoot your currently equipped weapon.\n\n" +
            "Weapons and Items\n" +
            "- You have 5 slots for different weapons and a health pack:\n" +
            "  1. Health Pack\n" +
            "  2. Pistol (unlimited ammo)\n" +
            "  3. Shotgun (bullet will spread)\n" +
            "  4. Rocket Launcher (has area damage, we can lure multiple enemies and shoot all of them)\n" +
            "  5. Auto-Rifle\n";

        // Set additional tutorials text
        tutorialsText_2.text =
            "Switching Items\n" +
            "- Press 1, 2, 3, 4, or 5 to select the corresponding weapon or health pack.\n\n" +
            "Health Recovery\n" +
            "- To recover health, press 1 to use a health pack. Each health pack restores 15 HP.\n\n" +
            "Melee Attack\n" +
            "- Press Space to perform a melee attack, useful when enemies are too close or if you run out of ammo.\n\n" +
            "Level Progression\n" +
            "- There are 3 levels in the game:\n" +
            "  1. In Level 1 (Go to North Gate), defeat the first boss to obtain the Shotgun.\n" +
            "  2. In Level 2 (Go to East Gate), defeat the second boss to acquire the Rocket Launcher.\n" +
            "  3. In the Final Level (Go to South Gate), explore the map to locate the Auto-Rifle and prepare to face the ultimate zombie boss.\n\n" +
            "GOOD LUCK!";
    }

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
