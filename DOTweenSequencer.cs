using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenSequencer : MonoBehaviour
{
    // [SerializeField]
    public List<DOTweenBaseAction> actions = new List<DOTweenBaseAction>();

    public void Play()
    {
        Debug.Log("Starting Sequence");
        Sequence sequence = DOTween.Sequence();
        foreach (DOTweenBaseAction action in actions)
        {
            action.addToSequence(sequence);
        }
    }
}