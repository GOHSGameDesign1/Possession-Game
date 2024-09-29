using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap colTilemap;

    private Vector2 targetPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { Move(Vector2.right); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { Move(Vector2.left); }
    }

    void Move(Vector2 direction)
    {
        if(CanMove(direction))
        {
            Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
            targetPos = groundTilemap.GetCellCenterWorld(gridPos);
            StopAllCoroutines();
            StartCoroutine(LerpSmoothToTarget());
        }
    }

    bool CanMove(Vector2 direction)
    {
        Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if(!groundTilemap.HasTile(gridPos) || colTilemap.HasTile(gridPos))
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.BoxCast(groundTilemap.GetCellCenterWorld(gridPos), Vector2.one * 0.5f, 0, Vector2.zero);
        if (hit)
        {
            if (hit.transform.CompareTag("Box"))
            {
                return hit.transform.GetComponent<GridMovement>().TryMove(direction);
            }
        }

        return true;
    }

    IEnumerator LerpSmoothToTarget()
    {
        while(Vector2.Distance(rb.position, targetPos) > 0.01f)
        {
            rb.position = ExpDecay(rb.position, targetPos, 16, Time.deltaTime);
            yield return null;
        }
        rb.position = targetPos;
    }

    Vector2 ExpDecay(Vector2 a, Vector2 b, float decay, float deltaTime)
    {
        return b + (a-b) * Mathf.Exp(-decay*deltaTime);
    }
}
