using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
public class PlayerMeter : MonoBehaviour
{
    private float maxHealth = 100;
    private float currentHealth;
    private float previewHealth;
    private bool isPaused = true;
    private Slider meterSlider;
    private float meterMultiplier = 6f;
    public bool isPlayer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meterSlider = GetComponent<Slider>();
        previewHealth = maxHealth;
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
        float adjustedHealth = previewHealth + amount;
        if (adjustedHealth > maxHealth)
        {
            adjustedHealth = maxHealth;
        }
        meterSlider.DOValue(adjustedHealth, 1f).SetEase(Ease.InOutBack);
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
            previewHealth -= Time.deltaTime * meterMultiplier;
            if (previewHealth <= 0)
            {
                previewHealth = 0;
                //player die
                if (isPlayer)
                {
                    FindFirstObjectByType<PlayerController>().Die();
                }
            }
            meterSlider.value = previewHealth;
        }
    }

    public void ShowAdjustedHealth()
    {

    }

    // previewHealth is the half-opaque health that is trickling down.
    // currentHealth is the full-opaque health that is currently displayed, and turns into previewHealth when the meter is paused.
    // TODO: control showing the previewHealth and currentHealth on the same slider
}
