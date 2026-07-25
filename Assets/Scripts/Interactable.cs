using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        None,
        Shovel,
        Water
    }

    [SerializeField]
    private InteractionType interactionType = InteractionType.None;
    private Collider collider;
    private MeterManager meterManager;
    private float waterAmount = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (interactionType == InteractionType.Water)
        {
            meterManager = FindFirstObjectByType<MeterManager>();
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

        }
        gameObject.SetActive(false);
    }
}
