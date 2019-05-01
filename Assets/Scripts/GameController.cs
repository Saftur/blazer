using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [Header ("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Scalable player;

    [Header ("Gameplay")]
    [Tooltip("Maximum player scale before changing levels")]
    [SerializeField] private float playerScaleThresh;
    [Tooltip ("Maximum size of the camera before changing levels")]
    [SerializeField] private float maxCameraSize;

    // Initial size of the camera
    private float cameraInitSize;

    // Start is called before the first frame update
    void Start () {
        // Subscribe to player scale event for checking when to change levels
        player.OnScale += OnPlayerScale;

        // Initialize intial camera Size
        cameraInitSize = mainCamera.orthographicSize;
    }
    
    // When the player changes in size.
    private void OnPlayerScale(float scalar) {
        // Check if size exceeds maximum player size
        if(scalar >= playerScaleThresh) {
            ProgressLevel ();
            return;
        }

        // zoom the camera out
        float zoom = (scalar / playerScaleThresh) * (maxCameraSize - cameraInitSize);
        mainCamera.orthographicSize = cameraInitSize + zoom;
    }

    private void ProgressLevel () {

    }
}
