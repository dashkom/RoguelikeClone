using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom, floor
    }

    public DoorType doortype;
    public Sprite openSprite;
    public Sprite closedSprite;
    public SpriteRenderer spriteRenderer;
    public float closeDelay = 1f;
    public Room room;
    public bool isOpen = false;
    public BoxCollider2D doorCollider;
    //PlayerController playerController;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<BoxCollider2D>();
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OpenDoor()
    {
        spriteRenderer.sprite = openSprite;
        doorCollider.isTrigger = true;
        isOpen = true;
    }

    public void CloseDoor()
    {
        spriteRenderer.sprite = closedSprite;
        doorCollider.isTrigger = false;
        isOpen = false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && doortype == DoorType.floor && isOpen)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameController.Level++;
            //playerController.currentLevel++;
        }
    }
}