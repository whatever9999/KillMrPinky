using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(11);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
