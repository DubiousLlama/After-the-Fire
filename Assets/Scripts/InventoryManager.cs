using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;
using System;

public class InventoryManager : MonoBehaviour
{
    public int maxStackSize = 100;
    public InvSlot[] invSlots;
    public GameObject invItemPrefab;
    public PuzzleManager puzzleGrid;
    public Item[] itemsToStart;
    public Item[] allPlants;
    private Dictionary<string, Item> plants;


    int selectedSlot = -1;

    private void Awake() {
        plants = new Dictionary<string, Item>();
        foreach(Item i in allPlants){   
            plants.Add(i.name, i);
        }

        foreach(Item i in itemsToStart) {
            AddItem(i);
        }
    }

    private void Start() {
        ChangeSelectedSlot(0);
    }

    private void Update() {
        
    }

    public void ChangeSelectedSlot(int newValue) {
        if (selectedSlot > -1)
            invSlots[selectedSlot].Deselect();
        invSlots[newValue].Select();
        selectedSlot = newValue;
    }

     public bool AddItem(Item item) {
        for (int i = 0; i < invSlots.Length; i++) {
            InvSlot slot = invSlots[i];
            // Debug.Log(i);
            InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();

            if(itemInSlot != null && itemInSlot.item == item &&
               itemInSlot.count < maxStackSize && itemInSlot.item.stackable) {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < invSlots.Length; i++) {
            InvSlot slot = invSlots[i];
            InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();

            if(itemInSlot == null) {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public bool AddItem(string item) {
        return AddItem(plants[item]);
    }

    void SpawnNewItem(Item item, InvSlot slot) {
        GameObject newItemGo = Instantiate(invItemPrefab, slot.transform);
        InvItem invItem = newItemGo.GetComponent<InvItem>();
        invItem.InitializeItem(item);
    }

    public Item GetSelectedItem(bool use) {
        InvSlot slot = invSlots[selectedSlot];
        InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();

        Debug.Log(itemInSlot == null);

        if(itemInSlot != null) {
            Item item = itemInSlot.item;
            if(use == true) {
                itemInSlot.count--;
                if(itemInSlot.count <= 0) {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.RefreshCount();
                }
            }
            return itemInSlot.item;
        }

        return null;
    }
}
