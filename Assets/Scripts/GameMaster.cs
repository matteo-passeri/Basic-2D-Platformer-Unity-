using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    [SerializeField] private int maxLives = 3;
    private static int _remainingLives = 3;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }

    [SerializeField] private int startingMoney;
    public static int Money;

    void Awake()
    {


        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
    public Transform spawnPrefab;
    public string respawnSoundName = "Respawning";
    public string respawnCountdownSoundName = "RespawningCountdown";
    public string gameOverSoundName = "GameOver";

    public CameraShake cameraShake;

    [SerializeField] private GameObject GameOverUI;

    [SerializeField] private GameObject upgradeMenu;

    // cache
    private AudioManager audioManager;

    void Start()
    {
        _remainingLives = maxLives;

        Money = startingMoney;

        if (cameraShake == null)
        {
            Debug.LogError("No camera shake referenced in gm");
        }

        // caching
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.Log("No audioManager found in the scene");
        }

        audioManager.PlaySound("GameMusic");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    public void ToggleUpgradeMenu()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
    }

    public void EndGame()
    {
        Debug.Log("GAMEOVER");

        audioManager.PlaySound(gameOverSoundName);
        GameOverUI.SetActive(true);
    }
    public IEnumerator _RespawnPlayer()
    {
        audioManager.PlaySound(respawnCountdownSoundName);

        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(respawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GetComponent<AudioSource>().Play();

        Transform spawnParticleClone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(spawnParticleClone.gameObject, 3f);
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives--;
        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
        gm.GetComponent<WaveSpawner>().enemyOnScreen--;
    }

    public void _KillEnemy(Enemy _enemy)
    {
        // Add dying sound
        audioManager.PlaySound(_enemy.deathSoundName);

        // Get money from enemy
        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");

        // Add particles-explosion
        Transform _deathParticlesClone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(_deathParticlesClone.gameObject, 3f);

        // Add camera-shake
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }


}
