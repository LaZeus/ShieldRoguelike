using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class EndGameMenu : MonoBehaviour {


    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToPatreon()
    {
        #if !UNITY_EDITOR
            openWindow("https://www.patreon.com/LaZeus");
        #endif
    }
}
