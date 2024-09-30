using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Tooltip("The door this button corresponds to")]
    [SerializeField] Door door;
    [SerializeField] bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Box") || collision.CompareTag("Corpse"))
        {
            door.AddButton();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") || collision.CompareTag("Corpse"))
        {
            door.RemoveButton();
        }
    }

    void CheckForPress()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.2f, 0, Vector2.zero);
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
            if (isPressed)
            {
                door.AddButton();
            } else { door.RemoveButton(); }
        }
    }

    private void OnEnable()
    {
        PlayerController.onMove += CheckForPress;
    }

    private void OnDisable()
    {
        PlayerController.onMove -= CheckForPress;
    }
}
