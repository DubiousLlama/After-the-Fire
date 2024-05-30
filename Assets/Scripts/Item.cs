using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    
    [Header("Only gameplay")]
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public String plantName;
    public Sprite image;
}
public enum ItemType {
    Plant,
    QuestItem
}

public enum ActionType {
    Idk,
    Smth
}
