using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public int playerMoney = 100;
    public int playerHealth = 100;
    public int rewardPerEnemy = 10;

    [SerializeField] private int basicTowerCost = 50;
    [SerializeField] private int advancedTowerCost = 100;

    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] TextMeshProUGUI healthText;

    private void Awake()
    {
        if (Instance && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

 
    private void Update()
    {
        UpdateUI();
        //LogPerformanceMetrics();
    }

    private void OnEnable()
    {
        EnemySpawner.OnWaveStarted += HandleWaveStarted;
        EnemySpawner.OnWaveCompleted += HandleWaveCompleted;
    }

    private void OnDisable()
    {
        EnemySpawner.OnWaveStarted -= HandleWaveStarted;
        EnemySpawner.OnWaveCompleted -= HandleWaveCompleted;
    }

    private void HandleWaveStarted(int waveIndex)
    {
        Debug.Log($"Wave {waveIndex + 1} started!");
    }

    private void HandleWaveCompleted(int waveIndex)
    {
        Debug.Log($"Wave {waveIndex + 1} completed!");
    }

    public int GetTowerCost(string towerType)
    {
        return towerType switch
        {
            "Basic" => basicTowerCost,
            "Advanced" => advancedTowerCost,
            _ => 0
        };
    }

    public void DeductMoney(int amount)
    {
        playerMoney -= amount;
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateUI();
    }

    public void ReducePlayerHealth(int damage)
    {
        if(playerHealth >= 0)
        {
            playerHealth -= damage;
        }

        else if (playerHealth <= 0) {

            GameOver();
        }
    }

    public void UpdateUI()
    {
        if (moneyText != null) moneyText.text = $"{playerMoney}";
        if (healthText != null) healthText.text = $"{playerHealth}";
    }

    public void UpdateWave(int waveIndex)
    {
        if (waveText != null) waveText.text = $"{waveIndex}";
       
    }

    private void GameOver()
    {
        Time.timeScale = 0.0f;
    }

    private void LogPerformanceMetrics()
    {
        Debug.Log($"Frame Time: {Time.deltaTime * 1000} ms");
        Debug.Log($"Allocated Memory: {System.GC.GetTotalMemory(false) / (1024 * 1024)} MB");
    }

    public void SelectTowerTypeBasic() => Manager.Instance.SetSelectedTowerType("Basic");
    public void SelectTowerTypeAdvanced() => Manager.Instance.SetSelectedTowerType("Advanced");
}


