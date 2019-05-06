using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField]
    private Scalable player = default;
    [Tooltip("The scene transitions object should be referenced here")]
    [SerializeField]
    private GrowShrinkTransition transitionMgr = default;

    [Header ("Gameplay")]
    [Tooltip("When the player reaches this scale it will change level")]
    [SerializeField]
    private float playerWinScale = default;

    void Awake () {
        player.OnScale += OnPlayerScaleEvent;    
    }

    private void OnPlayerScaleEvent (float scalar) {
        if(scalar >= playerWinScale) {
            transitionMgr.Transition ();
        }
    }
}
