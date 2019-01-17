using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Corrector : MonoBehaviour
{

    public Inventory inv;

    // Start is called before the first frame update
    void Start()
    {
        inv.resetPositions();
    }

         
    void OnEnable()
{
    //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
    SceneManager.sceneLoaded += OnLevelFinishedLoading;
}

    void OnDisable()
{
    //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
}

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
{
        Debug.Log("Thing worked");
        inv.resetPositions();
        if (SceneManager.GetActiveScene().buildIndex == 2) inv.Add(21);
}

}
