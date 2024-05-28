using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackSize = 4;
    public InvSlot[] invSlots;
    public GameObject invItemPrefab;

    int selectedSlot = -1;

    private void Start() {
        ChangeSelectedSlot(0);
    }

    private void Update() {
        if (Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8) {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue) {
        if (selectedSlot > -1)
            invSlots[selectedSlot].Deselect();
        invSlots[newValue].Select();
        selectedSlot = newValue;
    }

     public bool AddItem(Item item) {
        for (int i = 0; i < invSlots.Length; i++) {
            InvSlot slot = invSlots[i];
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

    void SpawnNewItem(Item item, InvSlot slot) {
        GameObject newItemGo = Instantiate(invItemPrefab, slot.transform);
        InvItem invItem = newItemGo.GetComponent<InvItem>();
        invItem.InitializeItem(item);
    }

    public Item GetSelectedItem(bool use) {
        InvSlot slot = invSlots[selectedSlot];
        InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();

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
