using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.pitch = 1 + Random.Range(-0.3f, 0.3f);
    }
}
