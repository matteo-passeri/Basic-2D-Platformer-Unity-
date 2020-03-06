using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave; }
    }
    public int enemyOnScreen = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;

    // change to private later
    public float waveCountdown;
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }
    // private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn Points ref. in WaveSpawner");
        }

        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        // Debug.Log(enemyOnScreen);
        
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("WAVE COMLETED");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("ALL WAVES COMPLETED. Looping...");
        }

        nextWave++;
    }

    bool EnemyIsAlive()
    {
        // searchCountdown -= Time.deltaTime;
        // if (searchCountdown <= 0f)
        // {
        //     searchCountdown = 1f;
        if (enemyOnScreen <= 0)
        {
            return false;
        }

        // }
        return true;

    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);

            enemyOnScreen++;

            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("spawning enemy: " + _enemy.name);

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(_enemy, _sp.position, _sp.rotation);

    }

}
