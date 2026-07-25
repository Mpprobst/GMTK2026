using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    public float speed = 2f;
    public float moveThreshold = 0.01f;

    public bool hasShovel = false;

    [SerializeField]
    private GameObject shovel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject;
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        // float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(input);
        float moveX = skewedInput.x;
        float moveZ = skewedInput.z;

        playerRigidbody.linearVelocity = new Vector3(moveX, 0, moveZ) * speed;
        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        float currentSpeed = move.magnitude;

        if (currentSpeed >= moveThreshold)
        {
            Quaternion rotation = Quaternion.LookRotation(move);
            player.transform.rotation = rotation;
        }

        playerAnimator.SetFloat("Speed", currentSpeed);

    }

    public void PickUpShovel()
    {
        shovel.SetActive(true);
        hasShovel = true;
    }

    public void Dig()
    {
        if(!hasShovel)
        {
            return;
        }
        
        Debug.Log("Digging");
        playerAnimator.SetTrigger("Dig");
    }
}
