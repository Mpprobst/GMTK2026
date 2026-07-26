using UnityEngine;
using UnityEngine.UI;

public class MeterManager : MonoBehaviour
{
    public PlayerMeter playerOneMeter;
    public PlayerMeter playerTwoMeter;

    public PlayerController playerOne;
    public PlayerController playerTwo;
    private PlayerMeter activePlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activePlayer = playerOneMeter;
        playerOneMeter.ResumeMeter();
        playerOneMeter.isPlayer = true;
        playerTwoMeter.isPlayer = true;
        playerOne.enabled = true;
        playerTwo.enabled = false;
        playerOneMeter.OnMeterEmpty.AddListener(OnMeterEmpty);
        playerTwoMeter.OnMeterEmpty.AddListener(OnMeterEmpty);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePlayersTurn();
        }
    }

    void TogglePlayersTurn()
    {
        activePlayer.PauseMeter(); // stop the current player's meter

        // swap the active player
        if (activePlayer == playerOneMeter)
        {
            activePlayer = playerTwoMeter;
            playerOne.enabled = false;
            playerTwo.enabled = true;
        }
        else
        {
            activePlayer = playerOneMeter;
            playerOne.enabled = true;
            playerTwo.enabled = false;
        }

        activePlayer.ResumeMeter();
    }

    public void AddToMeter(PlayerController playerController, float amount)
    {
        if (playerController != null)
        {
            activePlayer.AddToMeter(amount);
        }
    }

    public void OnMeterEmpty()
    {
        if (activePlayer == playerOneMeter)
        {
            playerOne.Die();
        }
        else
        {
            playerTwo.Die();
        }
    }
}
