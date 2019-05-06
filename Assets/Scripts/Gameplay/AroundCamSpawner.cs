using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundCamSpawner : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private Vector2 camRatio;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;

    private float timer = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (timer <= 0) {
            GameObject instance = Instantiate(prefab, transform);
            instance.transform.localPosition = NewSpawnPosition();
            timer = spawnTime;
        }
        timer -= Time.deltaTime;
    }

    private enum Side { top, bottom, left, right }
    private Vector2 NewSpawnPosition() {
        Vector2 pos = new Vector2(Random.Range(minDist, maxDist), Random.Range(-1.0f, 1.0f));
        Side side = (Side)Random.Range(0, 4);
        Vector2 camHalfSize;
        camHalfSize.y = Camera.main.orthographicSize;
        camHalfSize.x = camHalfSize.y * camRatio.x / camRatio.y;

        if (side == Side.top || side == Side.bottom) {
            Swap(ref pos.x, ref pos.y);
            pos.x *= camHalfSize.x + pos.y;
            if (side == Side.bottom)
                pos.y *= -1;
            pos.y += (side == Side.top ? camHalfSize.y : -camHalfSize.y);
        } else {
            pos.y *= camHalfSize.y + pos.x;
            if (side == Side.left)
                pos.x *= -1;
            pos.x += (side == Side.right ? camHalfSize.x : -camHalfSize.x);
        }
        return pos;
    }

    private void Swap(ref float f1, ref float f2) {
        float tmp = f1;
        f1 = f2;
        f2 = tmp;
    }
}
