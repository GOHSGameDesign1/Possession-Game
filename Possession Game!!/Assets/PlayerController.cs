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

    private Vector2 targetPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gridMovement = GetComponent<GridMovement>();
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
            gridMovement.TryMove(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridMovement.TryMove(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { gridMovement.TryMove(Vector2.right); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { gridMovement.TryMove(Vector2.left); }
    }
}
