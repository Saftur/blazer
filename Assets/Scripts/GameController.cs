using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [Header ("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject player;

    [Header ("Gameplay")]
    [Tooltip("Maximum player size before changing levels")]
    [SerializeField] private float playerSizeThresh;
    [Tooltip ("Maximum size of the camera before changing levels")]
    [SerializeField] private float maxCameraSize;

    // Start is called before the first frame update
    void Start () {
        //Subscribe to player scale event for checking when to change levels

    }

    // Update is called once per frame
    void Update () {

    }
    
    // When the player changes in size.
    private void OnPlayerScale(float size) {
        // Check if size exceeds maximum player size

        // zoom the camera out
    }
}
