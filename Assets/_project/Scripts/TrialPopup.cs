using UnityEngine;
using UnityEngine.UI;

public class TrialPopup : MonoBehaviour
{
    public static TrialPopup Instance { get; private set; }

    [Header("UI элементы")] public GameObject Panel;
    public Text TitleText;
    public Text SacrificePowerText;
    public Text EnemyPowerText;
    //public Slider PowerSlider;
    public Text ChanceText;
    public Text ResultText;
    public Button CloseButton;

    [Header("Настройки")] public float AutoCloseDelay = 3f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Panel.SetActive(false);

        if (CloseButton != null)
            CloseButton.onClick.AddListener(() => Panel.SetActive(false));
    }

    public void ShowTrial(int sacrificePower, int enemyPower)
    {
        Panel.SetActive(true);

        // Рассчитываем шанс победы
        float winChance = CalculateWinChance(sacrificePower, enemyPower);
        bool success = Random.value < winChance;

        // Заполняем UI
        TitleText.text = "⚔️ ИСПЫТАНИЕ ТОТЕМА ⚔️";
        SacrificePowerText.text = $"Твоя сила: {sacrificePower}";
        EnemyPowerText.text = $"Сила врага: {enemyPower}";

        //PowerSlider.maxValue = enemyPower * 2;
        //PowerSlider.value = sacrificePower;

        // Показываем шанс
        ChanceText.text = $"Шанс победы: {Mathf.RoundToInt(winChance * 100)}%";
        ChanceText.color = winChance >= 0.5f ? Color.green : Color.red;

        // Показываем результат
        if (success)
        {
            ResultText.text = "✅ ПОБЕДА! Тотем доволен!";
            ResultText.color = Color.green;
        }
        else
        {
            ResultText.text = "❌ ПОРАЖЕНИЕ! Тотем насылает проклятие!";
            ResultText.color = Color.red;
        }

        // Логируем результат
        if (success)
        {
            // Награда
            G.ResourceManager.AddResource(ResourceType.Food, 15);
            G.ResourceManager.AddResource(ResourceType.Gold, 10);
            LogSystem.Instance.AddLog($"ПОБЕДА в испытании! +15 еды, +10 золота", Color.green, "🏆");
        }
        else
        {
            // Проклятие
            G.CurseManager.ApplyCurse("rot", 45f);
            LogSystem.Instance.AddLog($"ПОРАЖЕНИЕ! Наложено проклятие 'Гниль'", Color.red, "💀");
        }

        // Автозакрытие
        //Invoke(nameof(Hide), AutoCloseDelay);
    }

    private float CalculateWinChance(int playerPower, int enemyPower)
    {
        if (playerPower >= enemyPower) return 0.9f; // 90% шанс
        if (playerPower <= enemyPower / 2) return 0.1f; // 10% шанс

        // Линейная зависимость
        return (float)playerPower / enemyPower * 0.8f + 0.1f;
    }

    private void Hide()
    {
        Panel.SetActive(false);
    }
}