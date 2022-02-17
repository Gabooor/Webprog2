using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class Player : MonoBehaviour
{
    //HP VARIABLES
    public static float health;
    private float maxhealth = 100;
    public Image healthBar;
    public static RectTransform rtHealthBar;
    private float hpwidth;
    private float hpheight;

    //MP VARIABLES
    public static float mana;
    private float maxmana = 100;
    public static float manaRegen;
    public static Text manaRegenText;
    public Image manaBar;
    public static RectTransform rtManaBar;
    private float mpwidth;
    private float mpheight;
    private bool isAbilityActive;
    private float abilityTimer;

    //GENERAL VARIABLES
    public static float moveSpeed;
    public static Text speedText;
    public static int damageBonus;
    public static Text damageBonusText;
    public static float fireRate;
    public static Text fireRateText;
    public static int enemiesKilled;
    public static int money;
    public static Text moneyText;

    //LEVEL VARIABLES
    public static int level = 1;
    public static int xp;
    public static int xpNeeded;
    public static Text levelText;
    public static Text xpText;
    public static Text healthText;
    public static Image xpBar;
    public static RectTransform rtXpBar;

    //INVENTORY VARIABLES
    public static Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    public static Inventory potionInventory;
    [SerializeField] private Potion_UI_Inventory potionUIInventory;
    public Transform selectedSlot;
    public static int selectedSlotValue;
    public static Item equippedItem;
    public static int staffLevel;
    public static int swordLevel;

    //VARIABLES FOR SHOOTING
    public Transform firePoint;
    public GameObject bulletPrefab;
    private float bulletSpeed = 7f;
    private float shootingCooldown;
    private float originalShootingCooldown;
    public static bool isShooting;

    //VARIABLES FOR ENEMY DETECTING
    public static List<GameObject> enemies;
    public Transform coin;


    // Start is called before the first frame update
    void Start()
    {
        //SETTING UP INVENTORY
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

        //SETTING UP POTION INVENTORY
        potionInventory = new Inventory();
        potionUIInventory.SetInventory(potionInventory);

        //SETTING UP HP AND HP BAR
        health = maxhealth;
        rtHealthBar = healthBar.GetComponent<RectTransform>();
        hpwidth = rtHealthBar.rect.width;
        hpheight = rtHealthBar.rect.height;
        healthText = GameObject.Find("amountOfHealth").GetComponent<Text>();
        healthText.text = health.ToString();

        //SETTING UP MP AND MP BAR
        mana = maxmana;
        rtManaBar = manaBar.GetComponent<RectTransform>();
        mpwidth = rtManaBar.rect.width;
        mpheight = rtManaBar.rect.height;

        //SETTING UP GENERAL STATS
        enemiesKilled = 0;
        money = 0;
        moneyText = GameObject.Find("amountOfMoney").GetComponent<Text>();
        moneyText.text = money.ToString();

        //SETTING UP LEVELS
        xpNeeded = (int)(Mathf.Round(0.05f * (Mathf.Pow(level + 1, 3)) + 1.5f * (Mathf.Pow(level + 1, 2)) + level + 1)) - (int)Mathf.Round(0.05f * (Mathf.Pow(level, 3)) + 1.5f * (Mathf.Pow(level, 2)) + level);
        levelText = GameObject.Find("yourLevelText").GetComponent<Text>();
        levelText.text = "Your level: " + level.ToString();
        xpText = GameObject.Find("yourXpText").GetComponent<Text>();
        xpBar = GameObject.Find("xpBar").GetComponent<Image>();
        rtXpBar = xpBar.GetComponent<RectTransform>();
        rtXpBar.sizeDelta = new Vector2(0, rtXpBar.sizeDelta.y);
        rtXpBar.localPosition = new Vector2(-105f, rtXpBar.localPosition.y);

        //ADDING STARTER ITEMS TO INVENTORY
        inventory.AddItem(new Item { itemType = Item.ItemType.Staff });

        //SETTING PRIMARY WEAPON
        equippedItem = inventory.itemList[0];
        selectedSlotValue = 0;

        shootingCooldown = 0f;
        enemies = new List<GameObject>();
        isShooting = false;
        isAbilityActive = false;
        abilityTimer = 3f;
        staffLevel = 1;
        swordLevel = 0;

        moveSpeed = 3f;
        speedText = GameObject.Find("SpeedText").GetComponent<Text>();
        speedText.text = moveSpeed.ToString();
        damageBonus = 0;
        damageBonusText = GameObject.Find("DamageText").GetComponent<Text>();
        damageBonusText.text = "+" + damageBonus.ToString();
        fireRate = 1f;
        fireRateText = GameObject.Find("FireRateText").GetComponent<Text>();
        fireRateText.text = fireRate.ToString() + "x";
        manaRegen = 5;
        manaRegenText = GameObject.Find("ManaRegenerationText").GetComponent<Text>();
        manaRegenText.text = manaRegen.ToString() + "/s";

        ItemWorld.SpawnItemWorld(new Vector2(-2, 0), new Item { itemType = Item.ItemType.HealthPotion });

        for (int i = 0; i < 11; i++)
        {
            //ItemWorld.SpawnItemWorld(new Vector2(i* 2, 0), new Item { itemType = Item.ItemType.HealthPotion });
            //Debug.Log(i + ".: " + Mathf.Round(0.02f * (Mathf.Pow(i + 1, 3)) + 0.04f * (Mathf.Pow(i + 1, 2)) + i + 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //GETTING THE EQUPPIED ITEM BASED ON NUMBER PRESSES
        if (Input.GetKeyDown("1"))
        {
            selectedSlot.position = new Vector3(855.0f, 65.0f, 0.0f);
            equippedItem = inventory.itemList[0]; //SLOT1
            selectedSlotValue = 0;
        }
        if (Input.GetKeyDown("2"))
        {
            if (inventory.itemList.Count >= 2)
            {
                selectedSlot.position = new Vector3(925.0f, 65.0f, 0.0f);
                equippedItem = inventory.itemList[1]; //SLOT2
                selectedSlotValue = 0;
            }
        }
        if (Input.GetKeyDown("3"))
        {
            if (inventory.itemList.Count >= 3)
            {
                selectedSlot.position = new Vector3(995.0f, 65.0f, 0.0f);
                equippedItem = inventory.itemList[2]; //SLOT3
                selectedSlotValue = 0;
            }
        }
        if (Input.GetKeyDown("4"))
        {
            if (inventory.itemList.Count >= 4)
            {
                selectedSlot.position = new Vector3(1065.0f, 65.0f, 0.0f);
                equippedItem = inventory.itemList[3]; //SLOT4
                selectedSlotValue = 0;
            }
        }

        //TAKING DAMAGE AND DEATH
        if (health > 0)
        {
        hpwidth = health / maxhealth * 100;
            rtHealthBar.sizeDelta = new Vector2(hpwidth, hpheight);  
        }
        else
        {
            rtHealthBar.sizeDelta = new Vector2(0, hpheight);
            Debug.Log("You died");
            //ANIM�CI�T BERAKNI
            //J�T�K V�GE
        }

        //SHOOTING
        if (Input.GetMouseButton(0) && shootingCooldown == 0)
        {
            if (equippedItem.isWeapon())
            {
                originalShootingCooldown = equippedItem.GetFireRate();
                Shoot();
                if (!isAbilityActive)
                {
                    shootingCooldown = originalShootingCooldown / fireRate;
                }
                else
                {
                    shootingCooldown = originalShootingCooldown / 2;
                }
            }
        }

        if(Input.GetMouseButton(0) && equippedItem.isWeapon())
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }

        if (shootingCooldown > 0)
        {
            shootingCooldown -= 1 * Time.deltaTime;
        }
        if (shootingCooldown < 0) shootingCooldown = 0;

        //USING ITEMS
        if(health < maxhealth)
        {
            if (Input.GetKeyDown("c"))
            {
                if (potionInventory.itemList.Count >= 1)
                {
                    Heal(50);
                    potionInventory.DeleteItemByIndex(potionInventory.itemList.Count - 1);
                }
                else
                {
                    Debug.Log("You don't have any potions");
                }
            }
        }

        //SPECIAL ABILITY - MANA
        if (Input.GetKeyDown("space"))
        {  
            if(mana == 100)
            {
                mana = 0;
                mpwidth = mana / maxmana * 100;
                rtManaBar.sizeDelta = new Vector2(mpwidth, mpheight);
                Debug.Log("Ability activated");
                isAbilityActive = true;
            }
            else
            {
                Debug.Log("You don't have enough mana");
            }
        }
        if (isAbilityActive)
        {
            abilityTimer -= 1 * Time.deltaTime;
        }
        if(abilityTimer < 0)
        {
            isAbilityActive = false;
            Debug.Log("Abiltiy ran off");
            abilityTimer = 3f;
        }
        if(mana < 100)
        {
            mana += manaRegen * Time.deltaTime;
            mpwidth = mana / maxmana * 100;
            rtManaBar.sizeDelta = new Vector2(mpwidth, mpheight);
        }
        if (mana > 100) mana = 100;

    }

    //ITEM PICKUP
    void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            if(itemWorld.GetItem().GetName() == "Health Potion")
            {
                if(potionInventory.itemList.Count < 9)
                {
                    potionInventory.AddItem(itemWorld.GetItem());
                    itemWorld.DestroySelf();
                    //Debug.Log(potionInventory.itemList.Count);
                }
            }
            else if(inventory.itemList.Count < 4)
            {
                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
    }

    //ENEMY ATTACK
    void OnCollisionEnter2D(Collision2D col)
    {
        Enemy enemy = col.transform.GetComponent<Enemy>();
        enemy.isInRange = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        Enemy enemy = col.transform.GetComponent<Enemy>();
        enemy.isInRange = false;
    }

    //HEALING
    void Heal(int hp)
    {
        health += hp;
        if (health > maxhealth)
        {
            health = maxhealth;
        }
        healthText.text = health.ToString();
    }
    
    //SHOOTING
    void Shoot()
    {
        GameObject bullet = Instantiate(equippedItem.GetBullet(), firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
    }

    //INCREASING XP AND LEVELS
    public static void increaseXP(int xpp)
    {
        xpNeeded = (int)(Mathf.Round(0.05f * (Mathf.Pow(level + 1, 3)) + 1.5f * (Mathf.Pow(level + 1, 2)) + level + 1)) - (int)Mathf.Round(0.05f * (Mathf.Pow(level, 3)) + 1.5f * (Mathf.Pow(level, 2)) + level);
        if(xp + xpp >= xpNeeded)
        {
            int newXp = xp + xpp - xpNeeded;
            xp = newXp;
            level++;
            xpNeeded = (int)(Mathf.Round(0.05f * (Mathf.Pow(level + 1, 3)) + 1.5f * (Mathf.Pow(level + 1, 2)) + level + 1)) - (int)Mathf.Round(0.05f * (Mathf.Pow(level, 3)) + 1.5f * (Mathf.Pow(level, 2)) + level);
            levelText.text = "Your level: " + level.ToString();
            xpText.text = xp.ToString() + "/" + xpNeeded.ToString();
            rtXpBar.sizeDelta = new Vector2(210 * ((float)xp / xpNeeded), rtXpBar.sizeDelta.y);
            rtXpBar.localPosition = new Vector2((105 - (rtXpBar.sizeDelta.x / 2)) * -1, rtXpBar.localPosition.y);
            float random = Random.Range(0f, 1f);
            if (random <= 0.25f)
            {
                moveSpeed = Mathf.Round((moveSpeed + 0.1f) * 100f) / 100f;
                speedText.text = moveSpeed.ToString();
            }
            else if(random > 0.25f && random <= 0.5f)
            {
                damageBonus += 1;
                damageBonusText.text = damageBonus.ToString();
            }
            else if(random > 0.5f && random <= 0.75f)
            {
                fireRate = Mathf.Round((fireRate + 0.05f) * 100f) / 100f;
                fireRateText.text = fireRate.ToString();
            }
            else
            {
                manaRegen += 0.5f;
                manaRegenText.text = manaRegen.ToString();
            }
        }
        else
        {
            xp += xpp;
            xpText.text = xp.ToString() + "/" + xpNeeded.ToString();
            rtXpBar.sizeDelta = new Vector2(210*((float)xp/xpNeeded), rtXpBar.sizeDelta.y);
            rtXpBar.localPosition = new Vector2((105- (rtXpBar.sizeDelta.x / 2)) * -1, rtXpBar.localPosition.y);
        }
    }
}
