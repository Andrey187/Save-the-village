using System;
using UnityEngine;
using UnityEngine.UI;

public class VillageHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int currentHealth = 0;
    public GameManager gM;
    public SFXmusic SFXmusic;

    public Text healthBarText;
    public GameObject gameOver;

    public event Action<float> HealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        
    }

    void Update()
    {
        HealthTextUpdate();
    }

    private void HealthTextUpdate()
    {
        healthBarText.text = "" + currentHealth;
    }

   
    public void ChangeHealth(int value)
    {
        currentHealth += value;
        
        if(currentHealth <= 0)
        {
            Death();
        }
        else
        {
            float currentHealthAsPercantage = (float)currentHealth / maxHealth;
            HealthChanged?.Invoke(currentHealthAsPercantage);
        }
    }

    public void Death()
    {
        SFXmusic.PlaySoundEffects(gM._audioEffectsClipMassive[1]);
        gameOver.SetActive(true);
        Time.timeScale = 0f;
    }
}
