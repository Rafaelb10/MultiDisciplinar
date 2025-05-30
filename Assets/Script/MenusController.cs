using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    [SerializeField] GameObject _creditos;
    [SerializeField] private GameObject _pauseMenuUI;
    private bool _isPaused = false;

    private void Start()
    {
        _pauseMenuUI.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        
               
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Resume();
            }

            else
            {
                Cursor.lockState = CursorLockMode.None;
                Pause();
            }

        }
    }
    public  void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Creditos()
    {
        _creditos.SetActive(true);

        
    }
   
    public void CloseCreditos()
    {
        _creditos.SetActive(false);
    }
    

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        _isPaused = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause()
    {
        
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        _isPaused = true;
    }
}


