using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTransitionVFX : MonoBehaviour
{
    //[SerializeField] GameObject transitionVFX;
    //private Transform player;

    //private void Awake()
    //{
    //    player = GetComponent<Transform>();
    //}

    //void NextLevelTransition()
    //{
    //    TransitionVFX vfx = Instantiate(transitionVFX, player.position, Quaternion.identity).GetComponent<TransitionVFX>();
    //    vfx.Expand();
    //    StartCoroutine(CallNextLevel(vfx.GetTransitionTime()));
    //}

    //void RestartTransition()
    //{
    //    TransitionVFX vfx = Instantiate(transitionVFX, player.position, Quaternion.identity).GetComponent<TransitionVFX>();
    //    vfx.Shrink();
    //    StartCoroutine(CallRestart(vfx.GetTransitionTime()));
    //}

    //IEnumerator CallNextLevel(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    SceneLoader.LoadNextLevel();
    //}

    //IEnumerator CallRestart(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    SceneLoader.RestartLevel();
    //}

    //private void OnEnable()
    //{
        
    //}

    //private void OnDisable()
    //{
        
    //}
}
