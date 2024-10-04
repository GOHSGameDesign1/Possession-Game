using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridMovement))]
public class PlayerController : MonoBehaviour
{
    protected GridMovement gridMovement;

    [SerializeField] private ParticleSystem possessVFX;

    private void Awake()
    {
        gridMovement = GetComponent<GridMovement>();
    }

    // Update is called once per frame
    private void Update()
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
            Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Default"));

        if (hit)
        {
            if (hit.transform.CompareTag("Corpse"))
            {

                if (hit.transform.GetComponent<CorpseController>().BecomePossessed())
                {
                    Instantiate(possessVFX, gridMovement.targetPos, Quaternion.identity);
                }
            }
        }
    }
}
