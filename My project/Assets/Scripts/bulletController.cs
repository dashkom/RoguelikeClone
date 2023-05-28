using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;
    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;
    private EnemyController boss;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathDelay());
    }

    //private void Awake()
    //{
    //    if (!isEnemyBullet)
    //    {
    //        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyController>();
    //    }

    //}
    // Update is called once per frame
    void Update()
    {
        if (isEnemyBullet)
        {
            curPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            if (curPos == lastPos)
            {
                Destroy(gameObject);
            }
            lastPos = curPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy" && !isEnemyBullet)
        {
            collider.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        if (collider.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }
        if (collider.tag == "Boss" && !isEnemyBullet)
        {
            EnemyController boss = collider.gameObject.GetComponent<EnemyController>();
            if (boss.bossHealth > 0)
            {
                boss.bossHealth -= 1;
            }
            else
            {
                collider.gameObject.GetComponent<EnemyController>().Death();
            }
            Debug.Log("BOSS DAMAGED");
            Destroy(gameObject);
            //boss.bossHealth -= 1;
        }
    }
}
