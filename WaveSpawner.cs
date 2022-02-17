using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public GameObject warrior;
    public GameObject warriorBoss;
    public GameObject tank;
    public GameObject tankBoss;
    public GameObject mage;
    public GameObject mageBoss;
    public Transform player;
    private float cooldown;

    public static int enemiesRemaining;

    private int waveCount;

    public static Text waveCountText;
    public static Text enemiesRemainingText;

    // Start is called before the first frame update
    void Start()
    {
        waveCountText = GameObject.Find("waveCountText").GetComponent<Text>();
        enemiesRemainingText = GameObject.Find("enemiesRemainingText").GetComponent<Text>();

        enemiesRemaining = 0;
        cooldown = 3f;
        waveCount = 0;
        waveCountText.text = "Wave " + waveCount.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(enemiesRemaining <= 1 || cooldown == 0)
        {
            waveCount++;
            waveCountText.text = "Wave " + waveCount.ToString();
            SpawnWave();
            cooldown = 10f;
        }
        if(cooldown > 0)
        {
            cooldown -= 1f * Time.deltaTime;
        }
        if (cooldown < 0) cooldown = 0;
    }

    void SpawnWave()
    {
        int enemiesToSpawn = 5 + waveCount / 3;
        //Debug.Log("Wave " + waveCount + " | " + enemiesToSpawn);
        for (int i = 1; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        enemiesRemaining++;
        enemiesRemainingText.text = "Enemies remaining: " + enemiesRemaining.ToString();
        Vector2 pos = player.position;
        float X = Random.Range(-10f, 10f);
        float Y = 10f;
        bool isPositive;
        if (Random.Range(0f, 1f) > 0.5f)
        {
            isPositive = true;
        }
        else isPositive = false;
        if (X > 0)
        {
            if (isPositive)
            {
                Y = 10f - X;
            }
            else Y = (10f - X) * -1;
        }
        else if (X < 0)
        {
            if (isPositive)
            {
                Y = -10f - X;
            }
            else Y = (-10f - X) * -1;
        }
        pos.x += X;
        pos.y += Y;
        GameObject enemy;
        float random = Random.Range(0f, 1f);
        float random2 = Random.Range(0f, 1f);
        if (random < 0.1f)
        {
            if (random2 > 0.9f)
            {
                enemy = Instantiate(tankBoss, pos, player.rotation);
                Enemy enemyE = enemy.transform.GetComponent<Enemy>();
                enemyE.isBoss = true;
            }
            else
            {
                enemy = Instantiate(tank, pos, player.rotation);
            }
        }
        else if(random > 0.1f && random < 0.4)
        {
            if(random2 > 0.9f)
            {
                enemy = Instantiate(warriorBoss, pos, player.rotation);
                Enemy enemyE = enemy.transform.GetComponent<Enemy>();
                enemyE.isBoss = true;
            }
            else
            {
                enemy = Instantiate(warrior, pos, player.rotation);
            }
        }
        else
        {
            if (random2 > 0.9f)
            {
                enemy = Instantiate(mageBoss, pos, player.rotation);
                Enemy enemyE = enemy.transform.GetComponent<Enemy>();
                enemyE.isBoss = true;
            }
            else
            {
                enemy = Instantiate(mage, pos, player.rotation);
            }
        }
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
    }
}
