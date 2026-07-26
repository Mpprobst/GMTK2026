using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        None,
        Shovel,
        Water,
        DigSite
    }

    [SerializeField]
    private InteractionType interactionType = InteractionType.None;
    [SerializeField]
    private GameObject holePrefab;

    private Collider collider;
    private MeterManager meterManager;
    private float waterAmount = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (interactionType == InteractionType.Water)
        {
            meterManager = FindFirstObjectByType<MenuManager>().meterManager;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            if (interactionType == InteractionType.Shovel)
            {
                Debug.Log("Shovel");
                playerController.PickUpShovel();
            }
            else if (interactionType == InteractionType.Water)
            {
                Debug.Log("Water");
                meterManager.AddToMeter(playerController, waterAmount);
            }
            else if (interactionType == InteractionType.DigSite)
            {
                Debug.Log("Dig Site");
                if (playerController.hasShovel)
                {
                    playerController.Dig();
                    Dig();
                }
                else
                {
                    return;
                }

            }

        }
        gameObject.SetActive(false);
    }

    private void Dig()
    {
        if (interactionType == InteractionType.DigSite)
        {
            if (holePrefab != null)
            {
                GameObject hole = Instantiate(holePrefab, transform.position, transform.rotation);
                hole.transform.localScale = Vector3.zero;
                hole.transform.DOScale(1f, .5f).SetEase(Ease.Linear);
            }
            gameObject.SetActive(false);
        }
    }
}
