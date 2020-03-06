using UnityEngine;
using UnityEngine.UI;


public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Text speedText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text healthRegenText;
    [SerializeField] private float healthMultiplier = 1.2f;
    [SerializeField] private float speedMultiplier = 1.1f;
    [SerializeField] private float damageMultiplier = 1.2f;
    [SerializeField] private float healthRegenMultiplier = 2f;
    [SerializeField] private int updatePrice = 50;

    private PlayerStats playerStats;

    // void Start()
    // {

    // }

    void OnEnable()
    {
        playerStats = PlayerStats.instance;

        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "HEALTH: " + playerStats.maxHealth.ToString();
        damageText.text = "DAMAGE: " + playerStats.damage.ToString();
        speedText.text = "SPEED: " + playerStats.movementSpeed.ToString();
        healthRegenText.text = "REGEN: " + playerStats.healthRegenRate.ToString();

    }

    public void UpgradeHealth()
    {
        if (GameMaster.Money < updatePrice)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        playerStats.maxHealth = (int)(playerStats.maxHealth * healthMultiplier);
        GameMaster.Money -= updatePrice;
        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }
    public void UpgradeDamage()
    {
        if (GameMaster.Money < updatePrice)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        playerStats.damage = (int)(playerStats.damage * damageMultiplier);
        GameMaster.Money -= updatePrice;
        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }
    public void UpgradeSpeed()
    {
        if (GameMaster.Money < updatePrice)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        playerStats.movementSpeed = Mathf.Round(playerStats.movementSpeed * speedMultiplier);
        GameMaster.Money -= updatePrice;
        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }
    public void UpgradeHealthRegen()
    {
        if (GameMaster.Money < updatePrice)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        playerStats.healthRegenRate++;
        GameMaster.Money -= updatePrice;
        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }



}