using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRigidbody;
    public float speed = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject;
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down
        Debug.Log("Move: " + moveX + ", " + moveZ);

        playerRigidbody.linearVelocity = new Vector3(moveX, 0, moveZ) * speed;
        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        Quaternion rotation = Quaternion.LookRotation(move);
        player.transform.rotation = rotation;
    }
}
