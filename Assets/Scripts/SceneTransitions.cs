using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour {

    [Header ("References")]
    [SerializeField] private Scalable player = default;
    [Tooltip ("Name of next level in the game progression (or end screen)")]
    [SerializeField] private string nextScene = default;
    [SerializeField] private Animator animator;

    [Header ("Gameplay")]
    [Tooltip ("When player reaches this scale the scene transition will begin")]
    [SerializeField] private float playerScaleThresh = default;

    //[Header ("Visuals")]
    //[Tooltip ("Rather the transition should be played on scene start")]
    //[SerializeField] private bool startOnLoad = true;

    void Start () {
        player.OnScale += OnPlayerScaleEvent;
    }

    private void OnPlayerScaleEvent (float scalar) {
        if (scalar >= playerScaleThresh) {
            animator.SetTrigger ("Grow");
        }
    }

    // To be called by animator when fade out animation is finisned 
    public void OnFadeCompleted () {
        SceneManager.LoadScene (nextScene);
    }
}
