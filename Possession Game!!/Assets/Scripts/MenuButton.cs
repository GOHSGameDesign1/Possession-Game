using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] float bigScaleModifier;
    float scaleModifier;


    // Start is called before the first frame update
    void Start()
    {
        scaleModifier = 1;
    }

    public void Expand()
    {
        //StopAllCoroutines();
        //StartCoroutine(LerpScale(scaleModifier));
        scaleModifier = bigScaleModifier;
    }

    public void Shrink()
    {
        scaleModifier = 1;
    }

    private void Update()
    {
        transform.localScale = Vector2.one * ExpDecay(transform.localScale.x, scaleModifier, 16, Time.deltaTime);
    }

    float ExpDecay(float a, float b, float decay, float deltaTime)
    {
        return b + (a-b) * Mathf.Exp(-decay *  deltaTime);
    }
}
