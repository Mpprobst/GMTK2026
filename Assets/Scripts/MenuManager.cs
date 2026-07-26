using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject gameMenu;

    [SerializeField]
    private GameObject pauseMenu;

    private bool isPaused = false;
    private bool isPlaying = false;
    public MeterManager meterManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        isPlaying = true;
        meterManager.TogglePlayersTurn();
        //CameraController.Instance.onCameraFinishZoom.AddListener(meterManager.TogglePlayersTurn);
        //CameraController.Instance.SetTarget(meterManager.playerOne.transform);
        CameraController.Instance.ZoomIn();
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            pauseMenu.SetActive(false);
            gameMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(true);
            gameMenu.SetActive(false);
        }
        isPaused = !isPaused;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
