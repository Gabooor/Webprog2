using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion_UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;


    public void Awake()
    {
        itemSlotContainer = transform.Find("potionItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("potionItemSlotTemplate");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        int total = 0;
        float itemSlotCellSize = 65f;

        foreach (Item item in inventory.GetItemList())
        {
            if(total < 9)
            {
                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize - 65, y * itemSlotCellSize - 65);
               // Debug.Log(new Vector2(x * itemSlotCellSize - 65, y * itemSlotCellSize - 65));
                Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
                image.sprite = item.GetSprite();
                x++;
                total++;
                if (x == 3)
                {
                    x = 0;
                    y++;
                }
            }
            
        }
    }
}
