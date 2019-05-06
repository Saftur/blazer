using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrowShrinkTransition : MonoBehaviour {

    [Header ("References")]
    [Tooltip ("Object to Grow/Shrink for transition")]
    [SerializeField]
    private GameObject target = default;
    [SerializeField]
    private string nextScene = default;

    [Header ("Transition Out")]
    [SerializeField]
    private Vector2 outTargetScale = default;
    [SerializeField]
    private Vector2 outTargetPosition = default;
    [Tooltip ("How long before scene transition finishes (seperate for end/start)")]
    [SerializeField]
    private float outDuration = 1f;

    [Header ("Transition In")]
    [Tooltip ("Play transition on scene start")]
    [SerializeField]
    private bool transitionIn = true;
    [Tooltip ("Size of target before it begins to transition in")]
    [SerializeField]
    private Vector2 inStartingScale = default;
    [SerializeField]
    private Vector2 outStartingPosition = default;
    [Tooltip ("How long before scene transition finishes (seperate for end/start)")]
    [SerializeField]
    private float inDuration = 1f;

    [Header ("Camera Zoom")]
    [SerializeField]
    private Camera mainCamera = default;
    [SerializeField]
    private bool zoomCamera = default;
    [SerializeField]
    private float cameraScale = default;

    private void Start () {
        if (transitionIn) { Transition (false); }
    }

    public void Transition (bool transitionOut = true) {

        if (!transitionOut) {
            Vector2 oldScale = target.transform.localScale;
            Vector2 oldPosition = target.transform.localPosition;
            target.transform.localScale = inStartingScale;
            target.transform.localPosition = outStartingPosition;
            StartCoroutine (ScaleSize (oldScale, oldPosition, inDuration, null));
        } else {
            StartCoroutine (ScaleSize (outTargetScale, outTargetPosition, outDuration, nextScene));
        }
    }

    private IEnumerator ScaleSize (Vector2 targetScale, Vector2 targetPosition, float duration, string nextScene) {
        target.GetComponent<BoxCollider2D> ().enabled = false;

        float progress = 0;
        Vector2 initialScale = target.transform.localScale;
        Vector2 initialPosition = target.transform.localPosition;
        float initialCameraZoom = zoomCamera ? mainCamera.orthographicSize : default;

        while (progress <= 1) {
            target.transform.localScale = Vector2.Lerp (initialScale, targetScale, progress);
            target.transform.localPosition = Vector2.Lerp (initialPosition, targetPosition, progress);
            if (zoomCamera) {
                mainCamera.orthographicSize = Mathf.Lerp (initialCameraZoom, cameraScale, progress);
            }
            progress += Time.deltaTime / duration;
            yield return null;
        }

        target.transform.localPosition = targetPosition;
        target.transform.localScale = targetScale;
        if (zoomCamera) {
            mainCamera.orthographicSize = cameraScale;
        }

        if (nextScene != null) {
            SceneManager.LoadScene (nextScene);
        } else {
            target.GetComponent<BoxCollider2D> ().enabled = true;
        }
    }
}
