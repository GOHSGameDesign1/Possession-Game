using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class CorpseController : PlayerController
{
    //GridMovement gridMovement;

    GameObject Player;

    [SerializeField] private bool isPossessed;

    [SerializeField] SpriteRenderer sr;

    private void Awake()
    {
        gridMovement = GetComponent<GridMovement>();
        Player = GameObject.FindGameObjectWithTag("Player");
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPossessed) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gridMovement.TryMove(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridMovement.TryMove(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { gridMovement.TryMove(Vector2.right); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { gridMovement.TryMove(Vector2.left); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPossessed = false;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
            Player.transform.position = gridMovement.targetPos;
            Player.SetActive(true);
        }
    }

    public void BecomePossessed()
    {
        isPossessed = true;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }
}
