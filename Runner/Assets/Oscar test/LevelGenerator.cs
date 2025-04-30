using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public GameObject buildingPrefab; // First 3
    public GameObject[] otherPrefabs; // After that
    public Transform player;

    public int tilesOnScreen = 5;
    public float spawnAhead = 20f;
    public float maxDistance = 20f;

    private float spawnX = 0f;
    private int tilesSpawned = 0;
    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (player.position.x + spawnAhead > spawnX && player.position.x < maxDistance)
        {
            SpawnTile();
            RemoveOldestTile();
        }
    }

    void SpawnTile()
    {
        GameObject prefabToUse;

        if (tilesSpawned < 3)
        {
            prefabToUse = buildingPrefab;
        }
        else
        {
            prefabToUse = otherPrefabs[Random.Range(0, otherPrefabs.Length)];
        }

        GameObject tile = Instantiate(prefabToUse);
        float tileWidth = GetPrefabWidth(tile);

        float yOffset = Random.Range(-0.2f, 0.2f);
        float gap = Random.Range(0.5f, 1f);

        Vector3 spawnPos = new Vector3(spawnX, yOffset, 0);
        tile.transform.position = spawnPos;

        SetLayerRecursively(tile, "Ground");

        spawnX += tileWidth + gap;
        activeTiles.Add(tile);
        tilesSpawned++;
    }

    float GetPrefabWidth(GameObject tile)
    {
        // Use renderer bounds to determine width
        Renderer renderer = tile.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size.x;
        }

        // Fallback to BoxCollider2D
        BoxCollider2D col = tile.GetComponentInChildren<BoxCollider2D>();
        if (col != null)
        {
            return col.bounds.size.x;
        }

        // Default fallback
        return 10f;
    }

    void RemoveOldestTile()
    {
        if (activeTiles.Count > tilesOnScreen)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
    }

    void SetLayerRecursively(GameObject obj, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layerName);
        }
    }
}
