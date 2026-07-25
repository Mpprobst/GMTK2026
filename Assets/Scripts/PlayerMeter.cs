using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMeter : MonoBehaviour
{

    private float maxHealth = 100;
    private float currentHealth;
    private bool isPaused = true;

    private Slider meterSlider;

    private float meterMultiplier = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meterSlider = GetComponent<Slider>();
        currentHealth = maxHealth;
        meterSlider.value = currentHealth;
    }

    public void PauseMeter()
    {
        isPaused = true;
    }

    public void ResumeMeter()
    {
        isPaused = false;
    }

    public void AddToMeter(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            return;
        }
        else
        {
            currentHealth -= Time.deltaTime * meterMultiplier;
            Debug.Log(currentHealth);
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            meterSlider.value = currentHealth;
        }
    }
}
