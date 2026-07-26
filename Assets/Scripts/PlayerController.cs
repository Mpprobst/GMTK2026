using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

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
    public bool isDead = true;
    public string playerName;

    private MenuManager menuManager;
    protected bool isTurn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        player = gameObject;
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>();
        menuManager = FindFirstObjectByType<MenuManager>();
        // audioSource = GetComponent<AudioSource>();
    }

    public virtual void Initialize()
    {
        isDead = false;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isTurn || isDead || menuManager.isPaused)
        {
            return;
        }

        Vector2 input = GetInput();
        Move(input.x, input.y);      
    }

    public virtual void StartTurn()
    {
        isTurn = true;
    }

    public virtual void EndTurn()
    {
        isTurn = false;
    }

    protected virtual Vector2 GetInput()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        // float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(input);
        return new Vector2(skewedInput.x, skewedInput.z);
    }

    public void Move(float x, float y)
    {
        playerRigidbody.linearVelocity = new Vector3(x, 0, y) * speed - Vector3.up;
        Vector3 move = new Vector3(x, 0f, y).normalized;
        float currentSpeed = move.magnitude;

        if (currentSpeed >= moveThreshold)
        {
            Quaternion rotation = Quaternion.LookRotation(move);
            player.transform.rotation = rotation;
            dustParticle.SetActive(true);
            if (meter != null)
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

    public IEnumerator Die()
    {
        playerAnimator.SetTrigger("Die");
        dustParticle.SetActive(false);
        yield return new WaitForSeconds(1.5f);
    }

    public void Idle()
    {
        if (isDead)
        {
            return;
        }
        dustParticle.SetActive(false);
        playerAnimator.SetFloat("Speed", 0);
        playerAnimator.Play("idle");
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

        playerAnimator.SetTrigger("Dig");
        audioSource.PlayOneShot(digSound);
    }
    
    public virtual void CollectWater(float amount)
    {
        if (meter)
            meter.AddToMeter(amount);
    }
}
