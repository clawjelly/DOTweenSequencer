using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DOTweenSequencer
{
    public class DOTweenSequencer : MonoBehaviour
    {
        [SerializeField]
        public List<DOTweenAction> actions = new List<DOTweenAction>();

        /**
        * <summary>
        * Plays the sequence.
        * </summary>
        */
        public void Play()
        {
            Debug.Log($"Starting Sequence for {gameObject.name}");
            Sequence sequence = DOTween.Sequence();
            foreach (DOTweenAction action in actions)
            {
                action.addToSequence(sequence);
            }
        }
    }
}