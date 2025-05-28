using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _creditos;
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _creditos.SetActive(false);
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
}
