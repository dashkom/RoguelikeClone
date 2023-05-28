using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigidBody;
    public Text levelText;
    public static int collectedAmount = 0;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    public Animator anim;
    Vector2 movement;
    private SpriteRenderer spriteRenderer;
    public static PlayerController instance;
    //public int currentLevel = 1;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");
        if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVert);
            lastFire = Time.time;
        }

        rigidBody.velocity = new Vector3 (movement.x * speed, movement.y * speed, 0);
        //levelText.text = "Уровень: " + currentLevel;

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }
    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
        );
    }
    public void FlashRed(float duration)
    {
        StartCoroutine(FlashRedCoroutine(duration));
    }

    private IEnumerator FlashRedCoroutine(float duration)
    {
        // Устанавливаем цвет спрайта на красный
        spriteRenderer.color = Color.red;

        // Ждем указанную длительность
        yield return new WaitForSeconds(duration);

        // Восстанавливаем исходный цвет спрайта
        spriteRenderer.color = Color.white;
    }

}
