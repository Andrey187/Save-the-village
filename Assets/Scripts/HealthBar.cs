using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFilling;
    [SerializeField] VillageHealth health;

    private void Awake()
    {
        health.HealthChanged += OnHealthChanged;
    }
    private void OnHealthChanged(float valueAsPercantage)
    {
        healthBarFilling.fillAmount = valueAsPercantage;
    }

}
