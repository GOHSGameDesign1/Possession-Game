using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    GameObject transitionVFX;
    private GameObject Player;
    private bool resetting;

    private void Awake()
    {
        //if (Instance == null)
        //{
            Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        //DontDestroyOnLoad(gameObject);

        transitionVFX = (GameObject)Resources.Load("Prefabs/TransitionVFX");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = GameObject.FindWithTag("Player");
        resetting = false;
        if (Player != null)
        {
            GameObject vfx = Instantiate(transitionVFX, Player.transform.position, Quaternion.identity);
            vfx.GetComponent<TransitionVFX>().Shrink();
        } else
        {
            GameObject vfx = Instantiate(transitionVFX, Vector3.zero, Quaternion.identity);
            vfx.GetComponent<TransitionVFX>().Shrink();
        }
    }

    public static SceneLoader GetInstance() {  return Instance; }


    private void Update()
    {
        if (resetting) return;
        if(Input.GetKeyDown(KeyCode.R))
        {
            resetting = true;
            CallRestart();        
        }
    }

    public float BeginTransition()
    {
        if (Player != null)
        {
            TransitionVFX vfx = Instantiate(transitionVFX, Player.transform.position, Quaternion.identity).GetComponent<TransitionVFX>();
            vfx.Expand();
            return vfx.GetTransitionTime();
        } else
        {
            TransitionVFX vfx = Instantiate(transitionVFX, Vector3.zero, Quaternion.identity).GetComponent<TransitionVFX>();
            vfx.Expand();
            return vfx.GetTransitionTime();
        }
    }

    public void CallNextLevel()
    {
        StartCoroutine(LoadNextLevel(BeginTransition()));
    }

    IEnumerator LoadNextLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        int sceneIndexToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndexToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            sceneIndexToLoad = 0;
        }
        SceneManager.LoadScene(sceneIndexToLoad);
    }
    public void CallRestart()
    {
        StartCoroutine(RestartLevel(BeginTransition()));
    }
    IEnumerator RestartLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
