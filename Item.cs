using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType
    {
        Staff,
        Sword,
        HealthPotion,
    }

    public ItemType itemType;

    public int level;
    public int damage;

    public string GetName()
    {
        switch (itemType)
        {
            default:
            case ItemType.Staff: return "Staff";
            case ItemType.Sword: return "Sword";
            case ItemType.HealthPotion: return "Health Potion";
        }
    }

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Staff: return ItemAssets.Instance.staffSprite;
            case ItemType.Sword: return ItemAssets.Instance.swordSprite;
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
        }
    }

    public bool isWeapon()
    {
        switch (itemType)
        {
            default:
            case ItemType.Staff:
            case ItemType.Sword: 
                return true;
            case ItemType.HealthPotion: 
                return false;
        }
    }

    public GameObject GetBullet()
    {
        switch (itemType)
        {
            default:
            case ItemType.Staff: return ItemAssets.Instance.staffBullet;
            case ItemType.Sword: return ItemAssets.Instance.swordBullet;  
        }
    }

    public float GetFireRate()
    {
        switch (itemType)
        {
            default:
            case ItemType.Staff: return 0.15f;
            case ItemType.Sword: return 0.25f;
        }
    }

    public int GetHealing()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return 50;
        }
    }

    public int GetDamage(int level)
    {
        switch (itemType)
        {
            default:
            case ItemType.Staff: return (int)Mathf.Round(0.01f * (Mathf.Pow(level + 1, 3)) + 0.02f * (Mathf.Pow(level + 1, 2)) + level);
            case ItemType.Sword: return (int)Mathf.Round(0.02f * (Mathf.Pow(level + 1, 3)) + 0.04f * (Mathf.Pow(level + 1, 2)) + level + 20);
        }
    }



    public float GetAliveTime()
    {
        switch (itemType)
        {
            default:
            case ItemType.Staff: return 1.5f;
            case ItemType.Sword: return 0.15f;
        }
    }

    public int GetPrice()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return 50;
        }
    }

    public int GetPrice(int level)
    {
        switch (itemType)
        {
            default:
            case ItemType.Staff: return (int) Mathf.Round(0.02f * (Mathf.Pow(level + 1, 3)) + 0.4f * (Mathf.Pow(level + 1, 2)) + level + 1);
            case ItemType.Sword: return (int) Mathf.Round(0.02f * (Mathf.Pow(level + 1, 3)) + 0.4f * (Mathf.Pow(level + 1, 2)) + level + 20);
            case ItemType.HealthPotion: return 50;
        }
    }
}
