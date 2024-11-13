using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public PlayerStats stats;
    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        healthBar = GetComponent<Slider>();
        healthBar.maxValue = stats.GetPlayerHealth();
        healthBar.value = stats.GetPlayerHealth();
    }
    private void Update()
    {
        healthBar.value = stats.GetPlayerHealth();
    }
}
