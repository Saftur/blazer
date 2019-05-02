using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 spawnAreaHalfSize;
    [SerializeField] private bool singleSpawn;
    [SerializeField] private float spawnTime;
    [SerializeField] private int numStartSpawn;

    private float timer = 0;

    // Start is called before the first frame update
    void Start() {
        if (singleSpawn)
            for (int i = 0; i < numStartSpawn; i++)
                Spawn();
    }

    // Update is called once per frame
    void Update() {
        if (singleSpawn)
            return;
        if (timer <= 0) {
            Spawn();
        }
        timer -= Time.deltaTime;
    }

    private void Spawn() {
        GameObject instance = Instantiate(prefab, transform);
        instance.transform.localPosition = NewSpawnPosition();
        timer = spawnTime;
    }

    private Vector2 NewSpawnPosition() {
        return new Vector2(Random.Range(-spawnAreaHalfSize.x, spawnAreaHalfSize.x), Random.Range(-spawnAreaHalfSize.y, spawnAreaHalfSize.y));
    }
}
