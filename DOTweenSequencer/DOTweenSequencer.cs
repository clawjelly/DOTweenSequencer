using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenSequencer : MonoBehaviour
{
    public List<DOTweenBaseAction> actions = new List<DOTweenBaseAction>();

    /**
     * <summary>
     * Plays the sequence.
     * </summary>
     */
    public void Play()
    {
        Debug.Log($"Starting Sequence for {gameObject.name}");
        Sequence sequence = DOTween.Sequence();
        foreach (DOTweenBaseAction action in actions)
        {
            action.addToSequence(sequence);
        }
    }
}