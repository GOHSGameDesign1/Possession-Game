using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool isOpen;
    [SerializeField] bool isObstructed;
    [Tooltip("How many buttons are opening this door at any current moment")]
    [SerializeField] int numberOfButtons;

    Collider2D col;

    [Header("Sprites")]
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;
    SpriteRenderer sr;

    private void Awake()
    {
        numberOfButtons = 0;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //i should really change these checks to be tied to a movement event from the player but not rn cus IM LAZY!!
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.2f, 0, Vector2.zero);
        if (hit)
        {
            if(hit.transform.CompareTag("Box") || hit.transform.CompareTag("Corpse"))
            {
                isObstructed = true;
            }
            else
            {
                isObstructed = false;
            }
        }
        else
        {
            isObstructed = false;
        }

        if (numberOfButtons > 0)
        {
            isOpen = true;
        }
        else
        {
            if (!isObstructed)
            {
                isOpen = false;
            }
        }

        col.enabled = !isOpen;
        sr.sprite = (isOpen)? openSprite : closedSprite;
    }

    public void AddButton()
    {
        numberOfButtons++;
    }

    public void RemoveButton()
    {
        numberOfButtons--;
    }
}
