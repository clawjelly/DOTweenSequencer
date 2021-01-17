using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class DOTweenScale : DOTweenBaseAction
{
    #region Public
    [SerializeField]
    private GameObject actor = null;
    [SerializeField]
    private Vector3 scale = Vector3.one;
    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private float starttime = 0.0f;
    [SerializeField]
    private Ease ease = Ease.Linear;
    [SerializeField]
    private bool doReset = false;
    #endregion

    private Vector3 startScale;


#if UNITY_EDITOR
    public override string Name
    { get { return (actor!=null)? "Scale "+actor.name : "Scale"; } }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        actor = (GameObject)EditorGUILayout.ObjectField(actor, typeof(GameObject), true);
        starttime = EditorGUILayout.FloatField("Start Time", starttime);
        duration = EditorGUILayout.FloatField("Duration", duration);
        ease = (Ease)EditorGUILayout.EnumPopup("Ease Type", ease);
        scale = EditorGUILayout.Vector3Field("Scale", scale);
        doReset = EditorGUILayout.Toggle("Reset on Complete", doReset);
        EditorGUILayout.EndVertical();
    }
#endif

    public override void addToSequence(Sequence seq)
    {
        if (doReset) startScale = actor.transform.localScale;
        if (actor==null) return;
        seq.Insert(starttime, actor.transform
            .DOScale(scale, duration)
            .SetEase(ease)
            .OnComplete(reset));
    }

    public void reset()
    {
        if (doReset)
        {
            // Debug.Log($"Resetting scale for {actor.name} to {startScale}.");
            actor.transform.localScale = startScale;
        }
    }
}
