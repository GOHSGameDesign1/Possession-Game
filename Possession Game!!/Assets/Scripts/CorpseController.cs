using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class CorpseController : PlayerController
{
    //GridMovement gridMovement;

    GameObject Player;

    [SerializeField] private bool isPossessed;

    [SerializeField] ColoredObject.Colors corpseColor;

    [SerializeField] private bool canBePossessed;

    [Header("Sprites")]
    [SerializeField] Sprite deadSprite;
    [SerializeField] Sprite possessedSprite;
    SpriteRenderer sr;

    private Color baseColor;

    private void Awake()
    {
        gridMovement = GetComponent<GridMovement>();
        Player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = deadSprite;
        CheckCanBePossessed();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPossessed) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gridMovement.TryMove(Vector2.down, corpseColor);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridMovement.TryMove(Vector2.up, corpseColor);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { gridMovement.TryMove(Vector2.right, corpseColor); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { gridMovement.TryMove(Vector2.left, corpseColor); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPossessed = false;
            sr.sprite = deadSprite;
            Player.transform.position = gridMovement.targetPos;
            Player.SetActive(true);
        }
    }

    public bool BecomePossessed()
    {
        if (!canBePossessed) return false;
        isPossessed = true;
        Player.SetActive(false);
        sr.sprite = possessedSprite;
        return true;
    }

    void CheckCanBePossessed()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0, Vector2.zero, Mathf.Infinity,
            LayerMask.GetMask("ColoredWall"));
        if (hit)
        {
            if(hit.transform.TryGetComponent<ColoredObject>( out ColoredObject wall))
            {
                canBePossessed = (wall.color == corpseColor);
            }
        } else
        {
            canBePossessed = true;
        }
    }

    public ColoredObject.Colors GetColor()
    {
        return corpseColor;
    }

    private void OnEnable()
    {
        GridMovement.onMove += CheckCanBePossessed;
    }

    private void OnDisable()
    {
        GridMovement.onMove -= CheckCanBePossessed;
    }
}
