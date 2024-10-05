using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadNextLevel()
    {
        int sceneIndexToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if(sceneIndexToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            sceneIndexToLoad = 0;
        }
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
