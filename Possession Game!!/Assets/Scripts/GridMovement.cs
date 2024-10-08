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

    public bool isLerping { get; private set; }

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

    private void OnEnable()
    {
        targetPos = transform.position;
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
            StopCoroutine("LerpSmoothToTarget");
            StartCoroutine(LerpSmoothToTarget());
            return true;
        }
        StartCoroutine("ShakeNo");
        return false;
    }

    public bool TryMove(Vector2 direction, ColoredObject.Colors color)
    {
        if (isLerping) return false;
        if (CanMove(direction, color))
        {
            targetPos = GetCellCenterOfPoint(transform.position + (Vector3)direction);
            StopCoroutine("LerpSmoothToTarget");
            StartCoroutine(LerpSmoothToTarget());
            return true;
        }
        StartCoroutine("ShakeNo");
        return false;
    }

    public void GhostMove(Vector2 direction)
    {
        if (isLerping) return;
        Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPos))
        {
            StartCoroutine("ShakeNo");
            return;
        }

        targetPos = GetCellCenterOfPoint(transform.position + (Vector3)direction);
        StopCoroutine("LerpSmoothToTarget");
        StartCoroutine(LerpSmoothToTarget());
    }

    bool CanMove(Vector2 direction)
    {
        Vector3Int gridPos = GetGridPosOfPoint(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPos) || colTilemap.HasTile(gridPos))
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.BoxCast(groundTilemap.GetCellCenterWorld(gridPos), Vector2.one * 0.5f, 0, Vector2.zero, 
            Mathf.Infinity, LayerMask.GetMask("Default"));
        if (hit)
        {
            if (hit.transform.CompareTag("Door")) return false;
            if (hit.transform.CompareTag("Box") || hit.transform.CompareTag("Corpse"))
            {
                return hit.transform.GetComponent<GridMovement>().TryMove(direction);
            }
        }

        return true;
    }

    bool CanMove(Vector2 direction, ColoredObject.Colors color)
    {
        Vector3Int gridPos = GetGridPosOfPoint(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPos) || colTilemap.HasTile(gridPos))
        {
            return false;
        }

        RaycastHit2D colorHit = Physics2D.BoxCast(groundTilemap.GetCellCenterWorld(gridPos), Vector2.one * 0.5f, 0, Vector2.zero,
            Mathf.Infinity, LayerMask.GetMask("ColoredWall"));

        if(colorHit)
        {
            if (colorHit.transform.TryGetComponent<ColoredObject>(out ColoredObject wall))
            {
                if (wall.color != color) return false;
            }
        }

        RaycastHit2D hit = Physics2D.BoxCast(groundTilemap.GetCellCenterWorld(gridPos), Vector2.one * 0.5f, 0, Vector2.zero,
            Mathf.Infinity, LayerMask.GetMask("Default"));
        if (hit)
        {
            if (hit.transform.CompareTag("Door")) return false;
            if (hit.transform.CompareTag("Box") || hit.transform.CompareTag("Corpse"))
            {
                return hit.transform.GetComponent<GridMovement>().TryMove(direction);
            }
        }

        return true;
    }

    IEnumerator LerpSmoothToTarget()
    {
        if(TryGetComponent<AudioSource>(out AudioSource source))
        {
            source.pitch = 1 + Random.Range(-0.3f, 0.3f);
            source.Play();
        }
        isLerping = true;
        while (Vector2.Distance(rb.position, targetPos) > 0.1f)
        {
            rb.position = ExpDecay(rb.position, targetPos, 40, Time.deltaTime);
            yield return null;
        }
        rb.position = targetPos;
        if (onMove != null)
        {
            onMove();
        }
        isLerping = false;
    }

    IEnumerator ShakeNo()
    {
        transform.rotation = Quaternion.Euler(0, 0, 20);
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.Euler(0, 0, -20f);
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.identity;
    }

    Vector2 ExpDecay(Vector2 a, Vector2 b, float decay, float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }
}
