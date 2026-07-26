using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Animator animator;
    public float speed = 2f;
    public float moveThreshold = 0.01f;
    public bool isDead = false;

    public PlayerMeter meter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // enemy needs to choose a direction to move in
        // pick a point on the map, move towards it
        Vector3 destination = PickDestination();
        rigidbody.MovePosition(destination);
        rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * speed;
        animator.SetFloat("Speed", rigidbody.linearVelocity.magnitude);
        meter.isMoving = true;

        //TODO: doesn't need to move in update. when the enemy starts its turn, pick a destiation.
        // while it hasn't reached the destination, wait.
        // when it reaches the destination, turn ends.
    }

    private Vector3 PickDestination()
    {
        // TODO: pick a random point on the map
        return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }
}
