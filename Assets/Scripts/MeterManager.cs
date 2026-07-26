using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MeterManager : MonoBehaviour
{
    public PlayerMeter playerOneMeter;
    public PlayerMeter playerTwoMeter;

    public PlayerController playerOne;
    public PlayerController playerTwo;
    private PlayerMeter activePlayer;
    public UnityEvent gameOverEvent;

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
        playerOne.meter = playerOneMeter;
        playerTwo.meter = playerTwoMeter;
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
        if (playerOne.isDead || playerTwo.isDead)
        {
            return;
        }
        activePlayer.PauseMeter(); // stop the current player's meter

        // swap the active player
        if (activePlayer == playerOneMeter)
        {
            activePlayer = playerTwoMeter;
            playerOne.enabled = false;
            playerTwo.enabled = true;
            if (CameraController.Instance != null)
            {
                CameraController.Instance.target = playerTwo.transform;
            }
        }
        else
        {
            activePlayer = playerOneMeter;
            playerOne.enabled = true;
            playerTwo.enabled = false;
            if (CameraController.Instance != null)
            {
                CameraController.Instance.target = playerTwo.transform;
            }
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

            TriggerDeath(playerOne);
        }
        else
        {
            TriggerDeath(playerTwo);
        }
    }

    public void TriggerDeath(PlayerController player)
    {
        StartCoroutine(WaitForDeathAnimation(player));
    }

    private IEnumerator WaitForDeathAnimation(PlayerController player)
    {
        yield return player.Die();
        player.enabled = false;
        TogglePlayersTurn();
        player.isDead = true;
    }

    private void CheckGameOver()
    {
        if (playerOne.isDead && playerTwo.isDead)
        {
            gameOverEvent.Invoke();
        }
    }
}
