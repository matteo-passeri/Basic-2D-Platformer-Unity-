using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{


    [SerializeField] private StatusIndicator statusIndicator;

    private AudioManager audioManager;

    [Range(0, 1)] private int randomGruntVoice;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName01 = "GruntVoice01";
    public string damageSoundName02 = "GruntVoice02";

    [SerializeField] public int bottomLineDeath = -40;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = PlayerStats.instance;

        playerStats.curHealth = playerStats.maxHealth;

        
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.Log("ENEMY: No audioManager found.");
        }


        if (statusIndicator == null)
        {
            Debug.LogError("No status indicator on player");
            statusIndicator = GameObject.FindGameObjectWithTag("StatusIndicator").GetComponent<StatusIndicator>();

            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }
        else
        {
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }

        InvokeRepeating("RegenHealth", 1f/playerStats.healthRegenRate, 1f/playerStats.healthRegenRate);
    }

    void RegenHealth()
    {
        playerStats.curHealth += 1;
        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

    void Update()
    {
        if (transform.position.y <= bottomLineDeath)
        {
            DamagePlayer(99999);
        }

    }

    // void OnUpgradeMenuToggle(bool active)
    // {
    //     GetComponent<Platformer2DUserControl>().enabled = !active;
    //     Weapon _weapon = GetComponentInChildren<Weapon>();
    //     if (_weapon != null)
    //     {
    //         _weapon.enabled = !active;
    //     }
    // }

    public void DamagePlayer(int damage)
    {
        playerStats.curHealth -= damage;

        if (playerStats.curHealth <= 0)
        {
            GameMaster.KillPlayer(this);

            audioManager.PlaySound("DeathVoice");

            Debug.Log("IM DEAD!");
        }
        else
        {
            randomGruntVoice = Random.Range(0, 1);
            if (randomGruntVoice == 0)
            {
                audioManager.PlaySound(damageSoundName01);
            }
            else
            {
                audioManager.PlaySound(damageSoundName02);

            }
        }

        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }


}
