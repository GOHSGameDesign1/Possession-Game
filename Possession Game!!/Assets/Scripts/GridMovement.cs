using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Tilemap groundTilemap;
    Tilemap colTilemap;

    public Vector2 targetPos {  get; private set; }

    bool isLerping;

    public delegate void OnMove();
    public static event OnMove onMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject grid = GameObject.Find("Grid");
        groundTilemap = grid.GetComponentsInChildren<Tilemap>()[0];
        colTilemap = grid.GetComponentsInChildren<Tilemap>()[1];
    }
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        isLerping = false;
    }

    public Vector2 GetCellCenterOfPoint(Vector3 point)
    {
        return groundTilemap.GetCellCenterWorld(GetGridPosOfPoint(point));
    }

    Vector3Int GetGridPosOfPoint(Vector3 point)
    {
        return groundTilemap.WorldToCell(point);
    }

    public bool TryMove(Vector2 direction)
    {
        if (isLerping) return false;
        if (CanMove(direction))
        {
            targetPos = GetCellCenterOfPoint(transform.position + (Vector3)direction);
            StopAllCoroutines();
            StartCoroutine(LerpSmoothToTarget());
            return true;
        }

        return false;
    }

    public void GhostMove(Vector2 direction)
    {
        if (isLerping) return;
        Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPos))
        {
            return;
        }

        targetPos = GetCellCenterOfPoint(transform.position + (Vector3)direction);
        StopAllCoroutines();
        StartCoroutine(LerpSmoothToTarget());
    }

    bool CanMove(Vector2 direction)
    {
        Vector3Int gridPos = GetGridPosOfPoint(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPos) || colTilemap.HasTile(gridPos))
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.BoxCast(groundTilemap.GetCellCenterWorld(gridPos), Vector2.one * 0.5f, 0, Vector2.zero);
        if (hit)
        {
            if (hit.transform.CompareTag("Door")) return false;
            if (hit.transform.CompareTag("Box"))
            {
                return hit.transform.GetComponent<GridMovement>().TryMove(direction);
            }
        }

        return true;
    }

    IEnumerator LerpSmoothToTarget()
    {
        isLerping = true;
        while (Vector2.Distance(rb.position, targetPos) > 0.1f)
        {
            rb.position = ExpDecay(rb.position, targetPos, 40, Time.deltaTime);
            yield return null;
        }
        rb.position = targetPos;
        onMove();
        isLerping = false;
    }

    Vector2 ExpDecay(Vector2 a, Vector2 b, float decay, float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }
}
