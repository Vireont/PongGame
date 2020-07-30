using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Restart_press()
    {
        Debug.Log("Presed: ");
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);

    }

    public void Start_press()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Exit_press()
    {
        Application.Quit();
    }
}
