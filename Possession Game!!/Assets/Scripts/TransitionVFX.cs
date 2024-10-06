using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionVFX : MonoBehaviour
{
    [SerializeField] private float transitionTime;
    private float decayConstant;

    private AudioSource source;

    private void Awake()
    {
        decayConstant = -Mathf.Log(0.1f)/transitionTime; // This math is from the exponential decay function
        // https://en.wikipedia.org/wiki/Exponential_decay

        source = GetComponent<AudioSource>();
    }

    public void Expand()
    {
        transform.localScale = Vector2.one * 0.1f;
        StartCoroutine(LerpSize(30f, true));
        source.pitch = 1;
        source.Play();
    }

    public void Shrink()
    {
        transform.localScale = Vector2.one * 30f;
        StartCoroutine(LerpSize(0.1f, false));
        //source.timeSamples = source.clip.samples - 1;
        //source.pitch = -1;
        //source.Play();
    }

    IEnumerator LerpSize(float endSizeScalar, bool nextLevel)
    {
        while(Mathf.Abs(transform.localScale.x - endSizeScalar) > 0.1f) {
            transform.localScale = Vector2.one * ExpDecay(transform.localScale.x, endSizeScalar, decayConstant, Time.deltaTime);
            yield return null;
        }
        transform.localScale = Vector2.one * endSizeScalar;
        if (nextLevel) { }
        else { Destroy(gameObject); }
    }

    float ExpDecay(float a, float b, float decay,  float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }

    public float GetTransitionTime()
    {
        return transitionTime;
    }
}
