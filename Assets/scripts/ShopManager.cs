using UnityEngine;
using UnityEngine.UI;
using TMPro;
//gpt
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("UI")]
    public GameObject ShopPanel;
    public Button    UpgradeButton;
    public TMP_Text  UpgradeLabel;

    [Header("Cost Settings")]
    public int BaseKnifeUpgradeCost = 50;
    public int CostMultiplier       = 2;

    [Header("Input")]
    [Tooltip("Key to open/close the shop panel")]
    public KeyCode ToggleKey = KeyCode.Tab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShopPanel.SetActive(false);
        UpgradeButton.onClick.AddListener(OnUpgradeClicked);
        // no RefreshUI here – we'll do it on open
    }

    private void Update()
    {
        if (Input.GetKeyDown(ToggleKey))
            ToggleShop();
    }

    private void ToggleShop()
    {
        // Always refresh *before* changing panel visibility
        RefreshUI();

        bool wasOpen = ShopPanel.activeSelf;
        ShopPanel.SetActive(!wasOpen);
        Time.timeScale = wasOpen ? 1f : 0f;
    }

    private void OnUpgradeClicked()
    {
        var kt   = KnifeThrower.Instance;
        int tier = kt.CurrentUpgradeTier;
        int cost = BaseKnifeUpgradeCost * (int)Mathf.Pow(CostMultiplier, tier);

        if (GoldManager.Instance.GetGold() < cost)
            return;

        GoldManager.Instance.RemoveGold(cost);
        kt.UpgradeWeapon();
        RefreshUI();
    }

    public void RefreshUI()
    {
        var kt   = KnifeThrower.Instance;
        int tier = kt.CurrentUpgradeTier;

        if (kt.MaxUpgradeLevelReached)
        {
            UpgradeLabel.text          = "MAX UPGRADE";
            UpgradeButton.interactable = false;
        }
        else
        {
            int cost     = BaseKnifeUpgradeCost * (int)Mathf.Pow(CostMultiplier, tier);
            int nextTier = tier + 1;

            UpgradeLabel.text = 
                $"Cost: {cost} Gold\n" +
                $"Upgrade to Tier {nextTier}\n" +
                $"(×2 Damage)";
            UpgradeButton.interactable = true;
        }
    }
}
