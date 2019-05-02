using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public delegate void SpawnHandler (GameObject instance);
    public event SpawnHandler OnSpawn;

    [Header ("References")]
    [SerializeField] private GameObject prefab = default;
    [Header ("Spawner Settings")]
    [Tooltip ("Area objects can spawn within")]
    [SerializeField] private Vector2 spawnAreaHalfSize = default;
    [Tooltip ("Continues to spawn objects over time")]
    [SerializeField] private bool continuous = true;
    [Tooltip ("Spawn delay rate for continuous spawners")]
    [SerializeField] private float spawnTime = 1f;
    [Tooltip ("How many objects to spawn initially")]
    [SerializeField] private int numStartSpawn = 1;

    private float timer = 0;

    // Start is called before the first frame update
    void Start () {
        for (int i = 0; i < numStartSpawn; i++) {
            Spawn ();
        }
    }

    // Update is called once per frame
    void Update () {
        if (!continuous) {
            return;
        }

        if (timer <= 0) {
            Spawn ();
        }
        timer -= Time.deltaTime;
    }

    private void Spawn () {
        GameObject instance = Instantiate (prefab, transform);
        instance.transform.localPosition = NewSpawnPosition ();
        timer = spawnTime;

        OnSpawn?.Invoke (instance);
    }

    private Vector2 NewSpawnPosition () {
        return new Vector2 (Random.Range (-spawnAreaHalfSize.x, spawnAreaHalfSize.x), Random.Range (-spawnAreaHalfSize.y, spawnAreaHalfSize.y));
    }
}
