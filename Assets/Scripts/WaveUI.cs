using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text WaveCountdownText;

    // [SerializeField]
    // GameObject WaveIncoming;

    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState previousState;

    // Start is called before the first frame update
    void Start()
    {
        if (spawner == null)
        {
            Debug.Log("No spawner referenced.");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.Log("No waveAnimator referenced.");
            this.enabled = false;
        }
        if (WaveCountdownText == null)
        {
            Debug.Log("No WaveCountdownText referenced.");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.Log("No waveCountText referenced.");
            this.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
                // default:
                //     WaveIncoming.gameObject.SetActive(false);
                //     break;
        }

        previousState = spawner.State;

    }

    void UpdateCountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.COUNTING)
        {
            // Debug.Log("Counting");

            waveAnimator.SetBool("WaveIncoming", false);
            // WaveIncoming.gameObject.SetActive(false);
            // WaveCountdownText.gameObject.SetActive(true);
            waveAnimator.SetBool("WaveCountdown", true);
        }
        WaveCountdownText.text = ((int)spawner.WaveCountdown + 1).ToString();
    }
    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            // Debug.Log("Spawning");

            waveAnimator.SetBool("WaveCountdown", false);
            // WaveCountdownText.gameObject.SetActive(false);
            // WaveIncoming.gameObject.SetActive(true);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = (spawner.NextWave + 1).ToString();
        }
    }

    // IEnumerator WaitAndVanish()
    // {
    //     yield return new WaitForSeconds(0.3f);
    //     WaveCountdownText.gameObject.SetActive(false);
    //     WaveIncoming.gameObject.SetActive(false);
    //     waveAnimator.SetBool("WaveIncoming", false);



    // }
}
