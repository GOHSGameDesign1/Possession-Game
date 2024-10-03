using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Tooltip("The door this button corresponds to")]
    [SerializeField] List<Door> doors;
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
