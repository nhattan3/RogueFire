using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Energy Settings")]
    private int currentEnergy;
    [SerializeField] private int energyThreshold = 3;
    [SerializeField] private Image energyBar;

    [Header("Gameplay Elements")]
    [SerializeField] private GameObject boss; 
    [SerializeField] private GameObject enemySpaner;

    [Header("UI Menus")]
    [SerializeField] private GameObject gameUi; 
    [SerializeField] private GameObject mainMenu; 
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winMenu;

    [Header("Audio System")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private GameObject red; 
    private bool bossCalled = false; 

    void Start()
    {
        currentEnergy = 0;
        UpdateEnergyBar();
        boss.SetActive(false); 
        MainMenu();
        audioManager.StopAudioGame();
        cam.Lens.OrthographicSize = 5f;
        red.SetActive(false);

    }

    // --- LOGIC GAMEPLAY ---

    public void AddEnergy()
    {
        if (bossCalled)
        {
            return;
        }

        currentEnergy += 1;
        UpdateEnergyBar();

        if (currentEnergy == energyThreshold)
        {
            CallBoss();
        }
    }

    private void CallBoss()
    {
        bossCalled = true;
        boss.SetActive(true);
        enemySpaner.SetActive(false);
        gameUi.SetActive(false); 
        audioManager.PlayBossAudio();
        cam.Lens.OrthographicSize = 10f;
        red.SetActive(true); 
    }

    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / (float)energyThreshold);
            energyBar.fillAmount = fillAmount;
        }
    }

    // THÊM HÀM NÀY: Gọi từ PlayerCollision khi ăn được Usb
    public void WinGame()
    {
        Time.timeScale = 0f;            // Dừng mọi chuyển động trong game
        audioManager.StopAudioGame();   // Tắt toàn bộ âm thanh nền
        
        // Bật/Tắt các Menu UI tương ứng khi thắng
        winMenu.SetActive(true); 
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameUi.SetActive(false);
    }


    // --- QUẢN LÝ CÁC MENU UI ---

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        gameUi.SetActive(false);
        Time.timeScale = 0f;
    }

    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        gameUi.SetActive(false);
        Time.timeScale = 0f;
    }

    public void PauseGameMenu()
    {
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        winMenu.SetActive(false);
        gameUi.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        winMenu.SetActive(false);
        gameUi.SetActive(true); // Bật giao diện chơi game lên
        Time.timeScale = 1f;
        audioManager.PlayDefaultAudio();
    }

    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        winMenu.SetActive(false);
        gameUi.SetActive(true);
        Time.timeScale = 1f;
    }
}