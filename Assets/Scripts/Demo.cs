using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id) {
        if (inventoryManager.AddItem(itemsToPickup[id]))
            Debug.Log("Item added");
        else
            Debug.Log("Item not added");
    }

    public void GetSelectedItem() {
        Item receiveditem = inventoryManager.GetSelectedItem(false);

        if (receiveditem != null)
            Debug.Log("Received item " + receiveditem);
        else
            Debug.Log("No item received");
    }

    public void UseSelectedItem() {
        Item receiveditem = inventoryManager.GetSelectedItem(true);

        if (receiveditem != null)
            Debug.Log("Use item " + receiveditem);
        else
            Debug.Log("No item received");
    }
}
