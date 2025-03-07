using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PrefabStorage", menuName = "Persistent Prefab Storage")]
public class PrefabStorage : ScriptableObject
{
    [System.Serializable]
    public class PrefabEntry
    {
        public string name;
        public string prefabPath;
    }

    public List<PrefabEntry> prefabs = new List<PrefabEntry>();
}

