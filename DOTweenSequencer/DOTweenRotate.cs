using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class DOTweenRotate : DOTweenBaseAction
{
    #region public
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
    private bool doReset = true;
    [SerializeField]
    private Ease ease = Ease.Linear;
    #endregion

    private Quaternion startRotation;


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
        // string rel = relative ? "Parent" : "World";
        rotation = EditorGUILayout.Vector3Field("Rotation", rotation);
        relative = EditorGUILayout.Toggle("Relative Rotation", relative);
        doReset = EditorGUILayout.Toggle("Reset on Complete", doReset);
        EditorGUILayout.EndVertical();
    }
#endif

    public override void addToSequence(Sequence seq)
    {
        if (doReset) startRotation = actor.transform.rotation;
        if (actor==null) return;
        if (relative)
            seq.Insert(starttime, actor.transform
                .DOLocalRotate(rotation, duration, RotateMode.FastBeyond360)
                .SetEase(ease)
                .OnComplete(reset));
        else
            seq.Insert(starttime, actor.transform
                .DORotate(rotation, duration, RotateMode.FastBeyond360)
                .SetEase(ease)
                .OnComplete(reset));
    }

    public void reset()
    {
        if (doReset)
        {
            // Debug.Log($"Resetting rotation for {actor.name} to {startRotation}.");
            actor.transform.rotation = startRotation;
        }
    }
}
