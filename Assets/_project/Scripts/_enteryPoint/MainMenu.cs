using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject Curtain;

    private void Start()
    {
        G.AudioManager.PlayMusic(R.Audio.MainMenuMusic);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}