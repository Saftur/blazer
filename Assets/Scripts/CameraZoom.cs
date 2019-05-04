using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour {

    [Header ("References")]
    [SerializeField] private Camera mainCamera = default;
    [SerializeField] private Scalable player = default;

    [Header ("Gameplay")]
    [Tooltip("When player reaches this scale camera will be fully zoomed out")]
    [SerializeField] private float playerScaleThresh = default;
    [Tooltip ("Size of camera when fully zoomed out")]
    [SerializeField] private float maxCameraSize = default;

    // Speed at which camera changes size
    [Header ("Visuals")]
    [Tooltip ("Speed at which camera will zoom in/out")]
    [SerializeField] private float cameraLerpSpeed = 2.0f;

    // Initial size of the camera
    private float cameraInitSize;
    // Size camera should be at
    private float cameraTargetSize;

    // Start is called before the first frame update
    void Start () {
        // Subscribe to player scale event for checking when to change levels
        player.OnScale += OnPlayerScale;

        // Initialize intial camera Size
        cameraInitSize = mainCamera.orthographicSize;
        // Initialize camera target size
        cameraTargetSize = cameraInitSize;
    }
    
    // When the player changes in size.
    private void OnPlayerScale(float scalar) {
        // Check if the camera is at max size
        if(cameraTargetSize >= maxCameraSize) {
            return;
        }

        // Set the camera's target zoom
        float zoom = (scalar - 1 / playerScaleThresh) * (maxCameraSize - cameraInitSize);
        cameraTargetSize = cameraInitSize + zoom;
    }

    void Update () {
        // Zoom the camera towards it's target zoom
        mainCamera.orthographicSize = Mathf.Lerp (mainCamera.orthographicSize, cameraTargetSize, cameraLerpSpeed * Time.deltaTime);
    }
}
