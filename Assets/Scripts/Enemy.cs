using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        public int startPctHealth = 1;
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 10;

        public void Init()
        {
            curHealth = maxHealth * startPctHealth;
        }
    }

    public EnemyStats enemyStats = new EnemyStats();

    public Transform deathParticles;
    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    AudioManager audioManager;

    public string deathSoundName = "Explosion";

    public int moneyDrop = 50;

    [Header("Optional:")]
    [SerializeField] private StatusIndicator statusIndicator;

    void Start()
    {
        enemyStats.Init();

        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.Log("ENEMY: No audioManager found.");
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }

        if (deathParticles == null)
        {
            Debug.LogError("No deathParticles in enemy");
        }
    }


    public void DamageEnemy(int damage)
    {
        enemyStats.curHealth -= damage;
        if (enemyStats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);

            Debug.Log("It's DEAD!");
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }

    }

    // enemy damage
    void OnCollisionEnter2D(Collision2D _collInfo)
    {
        Player _player = _collInfo.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(enemyStats.damage);

            // kamikaze enemy
            GameMaster.KillEnemy(this);
        }


    }
}

