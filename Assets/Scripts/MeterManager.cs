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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //activePlayer = playerOneMeter;
        //playerOneMeter.ResumeMeter();
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

    public void TogglePlayersTurn()
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
            playerOne.enabled = false;
            //playerTwo.enabled = true;
            CameraController.Instance.onCameraFinishZoom.AddListener(EnableMovement);
            CameraController.Instance.SetTarget(playerTwo.transform);
        }
        else
        {
            activeMeter = playerOneMeter;
            activePlayer = playerOne;
            //playerOne.enabled = true;
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
        CheckGameOver();
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
