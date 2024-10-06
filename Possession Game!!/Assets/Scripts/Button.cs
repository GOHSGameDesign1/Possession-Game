using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Tooltip("The door this button corresponds to")]
    [SerializeField] List<Door> doors;
    [SerializeField] bool isPressed;

    [SerializeField] Sprite unPressedSprite;
    [SerializeField] Sprite pressedSprite;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        sr.sprite = unPressedSprite;
    }

    void CheckForPress()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.2f, 0, Vector2.zero, Mathf.Infinity,
            LayerMask.GetMask("Default"));
        if (hit)
        {
            if(hit.transform.CompareTag("Box") || hit.transform.CompareTag("Corpse"))
            {
                ChangePressed(true);
            }
            else
            {
                ChangePressed(false);
            }
        }
        else
        {
            ChangePressed(false);
        }
    }

    void ChangePressed(bool value)
    {
        if(isPressed != value)
        {
            isPressed = value;

            foreach (Door door in doors)
            {
                if (isPressed)
                {
                    door.AddButton();
                } 
                else 
                {
                    door.RemoveButton();
                }
            }

            if (isPressed)
            {
                sr.sprite = pressedSprite;
            } else
            {
                sr.sprite = unPressedSprite;
            }

        }
    }

    private void OnEnable()
    {
        GridMovement.onMove += CheckForPress;
    }

    private void OnDisable()
    {
        GridMovement.onMove -= CheckForPress;
    }
}
