using UnityEngine;

[System.Serializable]
public class introSoundObj
{
    public string sound;
    public introSoundType soundType;
    public float playAtTime;
    public float playForTime = -1f;
}

public enum introSoundType
{
    music,
    ambiance,
    sfx
}
