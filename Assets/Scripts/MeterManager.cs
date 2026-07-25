using UnityEngine;

public class MeterManager : MonoBehaviour
{
    public PlayerMeter playerMeter;
    public PlayerMeter enemyMeter;
    private bool isPlayersTurn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPlayersTurn = true;
        playerMeter.ResumeMeter();
        playerMeter.isPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlayersTurn)
            {
                isPlayersTurn = false;
                playerMeter.PauseMeter();
                enemyMeter.ResumeMeter();
            }
            else
            {
                isPlayersTurn = true;
                playerMeter.ResumeMeter();
                enemyMeter.PauseMeter();
            }
        }
    }

    public void AddToMeter(PlayerController playerController, float amount)
    {
        if (playerController != null)
        {
            playerMeter.AddToMeter(amount);
        }
        else
        {
            enemyMeter.AddToMeter(amount);
        }
    }
}
