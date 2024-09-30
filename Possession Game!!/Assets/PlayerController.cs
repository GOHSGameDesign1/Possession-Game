using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridMovement))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    GridMovement gridMovement;

    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap colTilemap;

    [SerializeField] ParticleSystem possessVFX;

    private Vector2 targetPos;

    [SerializeField] bool isPossessed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gridMovement = GetComponent<GridMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //isPossessed = false;
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            CallMove(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CallMove(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { CallMove(Vector2.right); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { CallMove(Vector2.left); }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TryPossess();
        }
    }

    void TryPossess()
    {
        RaycastHit2D hit = Physics2D.BoxCast(gridMovement.GetCellCenterOfPoint(gridMovement.targetPos), Vector2.one * 0.5f, 0,
            Vector2.zero);

        if (hit)
        {
            if (hit.transform.CompareTag("Corpse"))
            {
                Instantiate(possessVFX, gridMovement.targetPos, Quaternion.identity);
                hit.transform.GetComponent<CorpseController>().BecomePossessed();
                gameObject.SetActive(false);
            }
        }
    }

    void CallMove(Vector2 direction)
    {
        if(isPossessed)
        {
            gridMovement.TryMove(direction);
        } else
        {
            gridMovement.GhostMove(direction);
        }
    }
}
