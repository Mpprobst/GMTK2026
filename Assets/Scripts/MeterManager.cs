using UnityEngine;
using UnityEngine.UI;

public class MeterManager : MonoBehaviour
{
    public PlayerMeter playerOneMeter;
    public PlayerMeter playerTwoMeter;

    public PlayerController playerOne;
    public PlayerController playerTwo;
    private PlayerController activePlayer;
    private PlayerMeter activeMeter;

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
        if (activeMeter)
            activeMeter.PauseMeter(); // stop the current player's meter

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
            playerOne.Die();
        }
        else
        {
            playerTwo.Die();
        }
    }
}
