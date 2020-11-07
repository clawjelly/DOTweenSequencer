using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class DOTweenRotate : DOTweenBaseAction
{
    [SerializeField]
    private GameObject actor = null;
    [SerializeField]
    private Vector3 rotation = Vector3.zero;
    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private float starttime = 0.0f;
    [SerializeField]
    private bool relative = false;
    [SerializeField]
    private Ease ease = Ease.Linear;


#if UNITY_EDITOR
    public override string Name
    { get { return (actor!=null)? "Rotate "+actor.name : "Rotate"; } }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        actor = (GameObject)EditorGUILayout.ObjectField(actor, typeof(GameObject), true);
        starttime = EditorGUILayout.FloatField("Start Time", starttime);
        duration = EditorGUILayout.FloatField("Duration", duration);
        ease = (Ease)EditorGUILayout.EnumPopup("Ease Type", ease);
        string rel = relative ? "Parent" : "World";
        relative = EditorGUILayout.Toggle("Relative Rotation", relative);
        rotation = EditorGUILayout.Vector3Field("Rotation", rotation);
        EditorGUILayout.EndVertical();
    }
#endif

    public override void addToSequence(Sequence seq)
    {
        if (actor==null) return;
        if (relative)
            seq.Insert(starttime, actor.transform
                .DOLocalRotate(rotation, duration, RotateMode.FastBeyond360)
                .SetEase(ease));
        else
            seq.Insert(starttime, actor.transform
                .DORotate(rotation, duration, RotateMode.FastBeyond360)
                .SetEase(ease));
    }
}
