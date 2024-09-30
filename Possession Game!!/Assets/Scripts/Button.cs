using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Tooltip("The door this button corresponds to")]
    [SerializeField] Door door;
    bool isPressed;

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
}
