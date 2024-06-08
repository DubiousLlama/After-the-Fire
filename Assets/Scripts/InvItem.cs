using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InvItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [Header("UI")]
    public Image image;
    public TMP_Text countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    
    public void InitializeItem(Item newItem) {
        item = newItem;
        image = GetComponent<Image>();
        image.sprite = newItem.image;
        image.preserveAspect = true;
        RefreshCount();
    }

    public void RefreshCount() {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
    }

    
    //dragon drop
    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
