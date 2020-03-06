using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [SerializeField] string ButtonOverSound = "ButtonOverSound";

    [SerializeField] string ButtonPressedSound = "ButtonPressedSound";
    AudioManager audioManager;

    void Start() {
        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.Log("MenuManager: No audioManager found.");
        }

        
        audioManager.PlaySound("MenuMusic");
    }
    public void StartGame()
    {
        audioManager.StopSound("MenuMusic");        
        audioManager.PlaySound(ButtonPressedSound);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);        
    }

    public void QuitGame()
    {
        audioManager.PlaySound(ButtonPressedSound);

        Debug.Log("IVE QUIT!");
        Application.Quit();        
    }

    public void OnMouseOver(){
        audioManager.PlaySound(ButtonOverSound);
    }
}
