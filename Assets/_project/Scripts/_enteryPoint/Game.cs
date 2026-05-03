using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject NoobPanel;
    
    public Totem Totem;
    private bool _isStop = false;

    public GameObject[] buttons;
    public bool IsPaused = false;
    public InputSystem InputSystem;
    public Camera MainCamera;

    private void Awake()
    {
        G.Game = this;
        
        InputSystem = new InputSystem();
        InputSystem.Enable();
        MainCamera = Camera.main;
    }

    private void OnDisable()
    {
         InputSystem.Disable();
    }

    private void Start()
    {
        if (Totem == null)
            Totem = FindFirstObjectByType<Totem>();

        G.AudioManager.PlayMusic(R.Audio.BackgroundMusic);
        //NoobPanel.SetActive(true);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (_isStop == false)
            WinLoseCondition();
        
        if(Input.GetMouseButtonDown(0))
            G.AudioManager.PlaySound(R.Audio.MouseClickSound);
    }

    public void WinLoseCondition()
    {
        if (Health.Instance.CurrentHealth <= 0 || Totem.CurrentFavor <= Totem.MinFavorForLose)
        {
            LogSystem.Instance.LogGameEnd(false);
            ResoultHandler.Instance.ShowResoult(false);
            G.AudioManager.PlaySound(R.Audio.LoseSound);
            _isStop = true;
            Time.timeScale = 0;
        }
        else if (Totem.CurrentFavor >= Totem.MinFavorForWin && Health.Instance.CurrentHealth > 0)
        {
            LogSystem.Instance.LogGameEnd(true);
            ResoultHandler.Instance.ShowResoult(true);
            _isStop = true;
            Time.timeScale = 0;
        }
    }

    public void SetUIInteractable(bool interactable)
    {
        foreach (GameObject button in buttons)
        {
            Button btn = button.GetComponent<Button>();
            if(btn != null)
                btn.interactable = interactable;
        }
    }

    public void PausedGame()
    {
        IsPaused = true;
        SetUIInteractable(false);
    }

    public void UnPausedGame()
    {
        IsPaused = false;
        SetUIInteractable(true);
    }

    public void RepeatGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        G.ResourceManager.ResetResource();
    }

    public void CloseNoobPanel()
    {
        NoobPanel.SetActive(false);
    }
}