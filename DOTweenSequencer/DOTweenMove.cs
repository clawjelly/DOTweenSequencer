using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using DG.Tweening;

public class DOTweenMove : DOTweenBaseAction
{
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
    private Ease ease = Ease.Linear;

#if UNITY_EDITOR

    public override string Name
    {
        get { return (actor==null)? "Move" : "Move "+actor.name; }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        actor = (GameObject)EditorGUILayout.ObjectField("Object", actor, typeof(GameObject), true);
        starttime = EditorGUILayout.FloatField("Start", starttime);
        duration = EditorGUILayout.FloatField("Duration", duration);
        ease = (Ease)EditorGUILayout.EnumPopup("Ease Type", ease);
        string rel = relative ? "Parent" : "World";
        relative = EditorGUILayout.Toggle("Relative to "+rel, relative);
        position = EditorGUILayout.Vector3Field("Position", position);
        EditorGUILayout.EndVertical();
    }

#endif

    public override void addToSequence(Sequence seq)
    {
        if (actor==null) return;
        if (relative)
            seq.Insert(starttime, actor.transform
                    .DOLocalMove(position, duration)
                    .SetEase(ease));
        else
            seq.Insert(starttime, actor.transform
                    .DOMove(position, duration)
                    .SetEase(ease));       
    }
}
