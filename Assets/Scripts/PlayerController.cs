using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    public float speed = 2f;
    public float moveThreshold = 0.01f;

    public bool hasShovel = false;
    public AudioClip digSound;
    public AudioSource audioSource;

    [SerializeField]
    private GameObject dustParticle;

    [SerializeField]
    private GameObject shovel;

    public PlayerMeter meter;
    private bool isDead = false;
    public string playerName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject;
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>();
        // audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
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
            dustParticle.SetActive(true);
            meter.isMoving = true;
        }
        else
        {
            dustParticle.SetActive(false);
            if (meter != null)
            {
                meter.isMoving = false;
            }
        }

        playerAnimator.SetFloat("Speed", currentSpeed);
    }

    public void Die()
    {
        playerAnimator.SetTrigger("Die");
        isDead = true;
    }

    public void PickUpShovel()
    {
        shovel.SetActive(true);
        hasShovel = true;
    }

    public void Dig()
    {
        if (!hasShovel)
        {
            return;
        }

        Debug.Log("Digging");
        playerAnimator.SetTrigger("Dig");
        audioSource.PlayOneShot(digSound);
    }
}
