using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die,
    Attack
};

public enum EnemyType
{
    CloseFight,
    Shooting,
    Boss
};

public class EnemyController : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rigidBody;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public float range;
    public float speed;
    public float attackRange;
    public float coolDown;
    private bool chooseDir = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public Sprite EnemyBullet;
    public Animator animator;
    public bool notInRoom = false;
    Room room;
    public int bossHealth;
    public static EnemyController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        room = GetComponentInParent<Room>();
        room.aliveEnemyCount++;
        if (enemyType == EnemyType.Boss)
        {
            bossHealth = 10; // Примерное значение здоровья босса
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState) 
        {
            //case EnemyState.Idle:Idle(); break;
            case EnemyState.Wander: Wander(); break;
            case EnemyState.Follow: Follow();  break;
            case EnemyState.Die: break;
            case EnemyState.Attack: Attack(); break;
        }

        if (!notInRoom)
        {
            if (IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Follow;
            }
            else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }
            if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
            {
                currState = EnemyState.Attack;
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
        if (bossHealth <= 0)
        {
            Death();
        }
    }

    public bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        //Quaternion nextRotation = Quaternion.Euler(randomDir);
        //transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }
    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }
        transform.position += transform.right * speed * Time.deltaTime;
        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }

        // установка параметров анимации
        animator.SetFloat("Horizontal", transform.right.x);
        animator.SetFloat("Vertical", transform.right.y);
        animator.SetFloat("Speed", speed);
    }
    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        // установка параметров анимации
        Vector2 movement = (player.transform.position - transform.position).normalized;
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", speed);
    }
    void Attack()
    {
        if (!coolDownAttack)
        {
            switch (enemyType)
            {
                case EnemyType.CloseFight:
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                break;
                case EnemyType.Shooting:
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<SpriteRenderer>().sprite = EnemyBullet;
                    bullet.GetComponent<bulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<bulletController>().isEnemyBullet = true;
                    StartCoroutine(CoolDown()); 
                break;
                case EnemyType.Boss:
                    GameObject bulletBoss = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bulletBoss.GetComponent<SpriteRenderer>().sprite = EnemyBullet;
                    bulletBoss.GetComponent<bulletController>().GetPlayer(player.transform);
                    bulletBoss.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bulletBoss.GetComponent<bulletController>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                    break;
            }
        }

    }
    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }
    
    public void Death()
    {
        if (currState != EnemyState.Die)
        {
            room.aliveEnemyCount--; 
            if (room.aliveEnemyCount == 0)
            {
                RoomConroller.instance.OnPlayerEnterRoom(room);
            }
            Destroy(gameObject);
        }
    }
}
