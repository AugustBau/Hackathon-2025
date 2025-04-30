using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public int poolSize = 6;
    public float spawnInterval = 6f;
    public float spawnY = 5f;
    public float xStart = 12f;

    private List<GameObject> pool = new List<GameObject>();
    private float timer;

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bird = Instantiate(birdPrefab);
            bird.SetActive(false);
            pool.Add(bird);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBirdPair();
            timer = 0f;
        }
    }

    void SpawnBirdPair()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject bird = GetBirdFromPool();
            if (bird != null)
            {
                float offsetY = Random.Range(-1f, 1f);
                bird.GetComponent<Bird>().Fly(new Vector2(xStart, spawnY + offsetY));
            }
        }
    }

    GameObject GetBirdFromPool()
    {
        foreach (var bird in pool)
        {
            if (!bird.activeInHierarchy)
                return bird;
        }

        return null;
    }
}
