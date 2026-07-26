using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;
public class PlayerMeter : MonoBehaviour
{
    private float maxHealth = 100;
    // private float currentHealth;
    public float PreviewHealth { get { return previewHealth; } }
    private float previewHealth;
    private bool isPaused = true;
    private Slider meterSlider;
    public float meterMultiplier = 6f;
    private float regenMultiplier = 0.5f;
    private float restMultiplier = 0.33f;
    public bool isPlayer = false;

    public UnityEvent OnMeterEmpty;
    public bool isMoving = false;
    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meterSlider = GetComponent<Slider>();
        previewHealth = maxHealth;
        // meterSlider.value = currentHealth;
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
        meterSlider.DOValue(adjustedHealth, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            previewHealth = adjustedHealth;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        if (isPaused)
            {
                // slow regeneration
                previewHealth += Time.deltaTime * meterMultiplier * regenMultiplier;
                if (previewHealth > maxHealth)
                {
                    previewHealth = maxHealth;
                }
                meterSlider.value = previewHealth;
                return;
            }
            else
            {
                if(isMoving)
                {
                    previewHealth -= Time.deltaTime * meterMultiplier;
                }
                else
                {
                    previewHealth -= Time.deltaTime * meterMultiplier * restMultiplier;
                }

                if (previewHealth <= 0)
                {
                    previewHealth = 0;
                    //player die
                    OnMeterEmpty.Invoke();
                    isDead = true;
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
