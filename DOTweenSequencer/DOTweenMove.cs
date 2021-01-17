using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using DG.Tweening;

public class DOTweenMove : DOTweenBaseAction
{
    #region public
    [SerializeField]
    private GameObject actor = null;
    [SerializeField]
    private Vector3 position = Vector3.zero;
    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private float starttime = 0.0f;
    [SerializeField]
    private bool relative = true;
    [SerializeField]
    private bool doReset = false;
    [SerializeField]
    private Ease ease = Ease.Linear;
    #endregion

    private Vector3 startPosition;

#if UNITY_EDITOR

    /**
     * <summary>
     * Generates a fitting name for the user interface
     * </summary>
     * <value></value>
     */
    public override string Name
    {
        get { return (actor==null)? "Move" : "Move "+actor.name; }
    }

    /**
     * <summary>
     * Generates the user interface for DOTweenSequencer.
     * 
     * Is being called by DOTweenSequencerEditor.
     * </summary>
     */
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        actor = (GameObject)EditorGUILayout.ObjectField("Object", actor, typeof(GameObject), true);
        starttime = EditorGUILayout.FloatField("Start", starttime);
        duration = EditorGUILayout.FloatField("Duration", duration);
        ease = (Ease)EditorGUILayout.EnumPopup("Ease Type", ease);
        string rel = relative ? "Relative to Parent" : "Relative to Start";
        position = EditorGUILayout.Vector3Field("Position", position);
        relative = EditorGUILayout.Toggle(rel, relative);
        doReset = EditorGUILayout.Toggle("Reset on Complete", doReset);
        EditorGUILayout.EndVertical();
    }

#endif

    public override void addToSequence(Sequence seq)
    {
        if (doReset) startPosition=actor.transform.localPosition;
        if (actor==null) return;
        if (relative)
            seq.Insert(starttime, actor.transform
                    .DOLocalMove(position, duration)
                    .SetEase(ease)
                    .OnComplete(reset));
        else
            seq.Insert(starttime, actor.transform
                    .DOLocalMove(position+startPosition, duration)
                    .SetEase(ease)
                    .OnComplete(reset));
    }

    public void reset()
    {
        if (doReset)
        {
            // Debug.Log($"Resetting position for {actor.name} to {startPosition}.");
            actor.transform.localPosition=startPosition;
        }
    }
}
