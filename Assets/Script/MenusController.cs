using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    [SerializeField] GameObject _creditos;
    public GameObject _pauseMenuUI;
    private bool _isPaused = false;
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        
               
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }
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
    }

    void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        _isPaused = true;
    }
}


