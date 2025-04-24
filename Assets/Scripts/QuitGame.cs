using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour{
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
        }
    }

    void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
}
