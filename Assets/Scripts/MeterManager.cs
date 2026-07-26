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
    private PlayerController activePlayer;
    private PlayerMeter activeMeter;
    public UnityEvent gameOverEvent;

    public PlayerController humanPlayer2, cpuPlayer2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //activePlayer = playerOneMeter;
        //playerOneMeter.ResumeMeter();
        playerOneMeter.isPlayer = true;
        playerTwoMeter.isPlayer = true;
        playerOne.enabled = true;
        humanPlayer2.enabled = false;
        cpuPlayer2.enabled = false;
        playerOneMeter.OnMeterEmpty.AddListener(OnMeterEmpty);
        playerTwoMeter.OnMeterEmpty.AddListener(OnMeterEmpty);
        playerOne.meter = playerOneMeter;
        humanPlayer2.meter = playerTwoMeter;
        cpuPlayer2.meter = playerTwoMeter;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (activeMeter == playerTwoMeter && playerTwo != humanPlayer2)
                return;

            TogglePlayersTurn();
        }
    }

    public void TogglePlayersTurn(bool deathToggle = false)
    {
        if (playerOne.isDead || playerTwo.isDead)
        {
            return;
        }
        if (activeMeter)
        {
            activeMeter.PauseMeter(); // stop the current player's meter
        }

        // swap the active player
        if (activeMeter == playerOneMeter)
        {
            activeMeter = playerTwoMeter;
            activePlayer = playerTwo;
            if (!deathToggle)
            {
                playerOne.Idle();
            }
            playerOne.enabled = false;
            //playerTwo.enabled = true;
            playerOne.EndTurn();
            playerTwo.StartTurn();
            CameraController.Instance.onCameraFinishZoom.AddListener(EnableMovement);
            CameraController.Instance.SetTarget(playerTwo.transform);
        }
        else
        {
            activeMeter = playerOneMeter;
            activePlayer = playerOne;
            playerTwo.EndTurn();
            playerOne.StartTurn();
            //playerOne.enabled = true;
            if (!deathToggle)
            {
                playerTwo.Idle();
            }
            playerTwo.enabled = false;
            CameraController.Instance.onCameraFinishZoom.AddListener(EnableMovement);
            CameraController.Instance.SetTarget(playerOne.transform);
        }

        //activePlayer.ResumeMeter();
    }

    private void EnableMovement()
    {
        activeMeter.ResumeMeter();
        activePlayer.enabled = true;
    }

    public void AddToMeter(PlayerController playerController, float amount)
    {
        if (playerController != null)
        {
            activeMeter.AddToMeter(amount);
        }
    }

    public void OnMeterEmpty()
    {
        if (activeMeter == playerOneMeter)
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
        TogglePlayersTurn(true);
        player.isDead = true;

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (playerOne.isDead && playerTwo.isDead)
        {
            gameOverEvent.Invoke();
        }
    }
}
