using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridMovement))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    GridMovement gridMovement;

    [SerializeField] ParticleSystem possessVFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gridMovement = GetComponent<GridMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            gridMovement.GhostMove(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridMovement.GhostMove(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { gridMovement.GhostMove(Vector2.right); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { gridMovement.GhostMove(Vector2.left); }

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
}
