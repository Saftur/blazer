using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [Header ("References")]
    [SerializeField] private Camera mainCamera = default;
    [SerializeField] private Scalable player = default;
    [Tooltip("Name of next level in the game progression (or end screen)")]
    [SerializeField] private String nextScene = default;

    [Header ("Gameplay")]
    [Tooltip("Maximum player scale before changing levels")]
    [SerializeField] private float playerScaleThresh = default;
    [Tooltip ("Maximum size of the camera before changing levels")]
    [SerializeField] private float maxCameraSize = default;

    // Speed at which camera changes size
    [Header ("Visuals")]
    [Tooltip ("Speed at which camera will zoom in/out")]
    [SerializeField] private float cameraLerpSpeed = 2.0f;

    [Header ("Level Transition")]
    [Tooltip ("How many increment cycles to have")]
    [SerializeField] private int incrementCount = 10;
    [Tooltip ("How much to scale player size each increment cycle")]
    [SerializeField] private float playerIncrementScale = 1.1f;
    [Tooltip ("How much to scale camera size each increment cycle")]
    [SerializeField] private float cameraIncrementScale = 1.1f;
    [Tooltip ("Delay between increment cycles")]
    [SerializeField] private float waitTime = 0.05f;

    // Initial size of the camera
    private float cameraInitSize;
    // Size camera should be at
    private float cameraTargetSize;

    // Pause variables
    private float savedTimeScale;
    private bool paused = false;
    public bool Paused {
        get { return paused; }
    }

    // Currently changing levels
    private bool transititoning = false;

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
        // Check if size exceeds maximum player size
        if(scalar >= playerScaleThresh ) {
            if (!transititoning) {
                player.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
                StartCoroutine (ProgressLevel ());
                transititoning = true;
            }
            return;
        }

        // Set the camera's target zoom
        float zoom = (scalar / playerScaleThresh) * (maxCameraSize - cameraInitSize);
        cameraTargetSize = cameraInitSize + zoom;
    }

    void Update () {
        // Zoom the camera towards it's target zoom
        mainCamera.orthographicSize = Mathf.Lerp (mainCamera.orthographicSize, cameraTargetSize, cameraLerpSpeed * Time.deltaTime);
    }

    IEnumerator ProgressLevel () {
        for(int i = 0; i < incrementCount; ++i) {
            player.gameObject.transform.localScale = new Vector3 (
                player.gameObject.transform.localScale.x * playerIncrementScale,
                player.gameObject.transform.localScale.y * playerIncrementScale, 
                player.gameObject.transform.localScale.z
            );

            mainCamera.orthographicSize = mainCamera.orthographicSize * cameraIncrementScale;

            yield return new WaitForSeconds (waitTime);
        }
        SceneManager.LoadScene (nextScene);
    }

    public void Pause (float duration = 0) {
        if (!paused) {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            paused = true;

            if(duration > 0) {
                Invoke ("UnPause", duration);
            }
        }
    }

    public void UnPause () {
        if (paused) {
            Time.timeScale = savedTimeScale;
            paused = false;
        }
    }
}
