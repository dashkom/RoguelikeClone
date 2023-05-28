using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private static int health = 6;
    private static int maxHealth = 6;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;
    private static int level = 1;
    public Text healthText;
    public Text levelText;

    public static int Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static int Level { get => level; set => level = value; }

    private void Start()
    {
        health = 6;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;
        levelText.text = "Level: " + level;
    }

    public static void DamagePlayer(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            KillPlayer();
        }
        else
        {
            // Вызываем метод FlashRed у экземпляра PlayerController
            PlayerController.instance.FlashRed(0.1f);
        }
    }

    public static void HealPlayer(int healAmount)
    {
        Health = Mathf.Min(maxHealth, health + healAmount);
    }

    private static void KillPlayer()
    {
        GameProgressData.Records records = new GameProgressData.Records(level);
        GameProgressData.Records.AddRecords(records, Path.Combine(Application.dataPath, "data.dat"));
        SceneManager.LoadScene("Death");
    }
}
