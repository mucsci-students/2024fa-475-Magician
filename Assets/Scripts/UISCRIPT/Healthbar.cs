using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _healthSlider;
    [SerializeField] PlayerStats _playerStats;

    private void Start()
    {
        // Initialize the slider max value to the player's starting health
        _healthSlider.maxValue = _playerStats.GetPlayerHealth();
        _healthSlider.value = _playerStats.GetPlayerHealth();
    }

    private void Update()
    {
        // Update the slider value to reflect the player's current health
        _healthSlider.value = _playerStats.GetPlayerHealth();
    }
}
