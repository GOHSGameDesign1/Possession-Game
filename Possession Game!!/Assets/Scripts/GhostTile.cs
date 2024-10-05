using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTile : MonoBehaviour
{
    [SerializeField] List<CorpseTile> corpseTiles;
    bool canWin;

    private void Awake()
    {
        canWin = CheckCanWin();
    }

    bool CheckCanWin()
    {
        foreach(CorpseTile tile in corpseTiles)
        {
            if (!tile.hasCorpse) return false;
        }

        return true;
    }

    void CheckForPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0, Vector2.zero,
    Mathf.Infinity, LayerMask.GetMask("Ghost"));

        if (hit)
        {
            canWin = CheckCanWin();
            if (canWin)
            {
                hit.transform.gameObject.SetActive(false);
                Debug.Log("Win!!!");
                SceneLoader.GetInstance().CallNextLevel();
            }
        }
    }

    private void OnEnable()
    {
        GridMovement.onMove += CheckForPlayer;
    }

    private void OnDisable()
    {
        GridMovement.onMove -= CheckForPlayer;
    }
}
