using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PersistentPrefabSpawner : MonoBehaviour
{
    public PrefabStorage prefabStorage; // Reference to PrefabStorage
    private string selectedPrefabPath = ""; // Path of selected prefab

    public void SpawnPrefab()
    {
        if (!string.IsNullOrEmpty(selectedPrefabPath))
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(selectedPrefabPath);
            if (prefab != null)
            {
                Instantiate(prefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Prefab not found at: " + selectedPrefabPath);
            }
        }
        else
        {
            Debug.LogWarning("No prefab selected!");
        }
    }

    public void SetSelectedPrefab(string path)
    {
        selectedPrefabPath = path;
    }
}
