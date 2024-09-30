using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class CorpseTile : MonoBehaviour
{
    ColoredObject colored;

    public bool hasCorpse { get; private set; }
    private void Awake()
    {
        colored = GetComponent<ColoredObject>();
        hasCorpse = false;
    }

    void CheckForCorpse()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0, Vector2.zero,
            Mathf.Infinity, LayerMask.GetMask("Default"));

        if (hit)
        {
            if (hit.transform.TryGetComponent<CorpseController>(out CorpseController corpse))
            {
                if (corpse.GetColor() == colored.color)
                {
                    hasCorpse =true;
                }
                else
                {
                    hasCorpse = false;
                }
            }
        }
        else
        {
            hasCorpse = false;
        }
    }

    private void OnEnable()
    {
        GridMovement.onMove += CheckForCorpse;
    }

    private void OnDisable()
    {
        GridMovement.onMove -= CheckForCorpse;
    }
}
