using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] string ButtonOverSound = "ButtonOverSound";

    [SerializeField] string ButtonPressedSound = "ButtonPressedSound";
    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.Log("MenuManager: No audioManager found.");
        }


        audioManager.PlaySound("MenuMusic");
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(ButtonOverSound);
    }

    public void Quit()
    {
        Debug.Log("QUITTING");
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
