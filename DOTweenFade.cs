using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using DG.Tweening;
using TMPro;

public class DOTweenFade : DOTweenBaseAction
{
    [SerializeField]
    private GameObject actor = null;
    [SerializeField]
    private float starttime = 0.0f;
    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private Vector3 position = Vector3.zero;
    [SerializeField]
    private float fade = 0f;
    [SerializeField]
    private Ease ease = Ease.Linear;
    [SerializeField][HideInInspector]
    private Component fader = null;

#if UNITY_EDITOR

    public override string Name
    {
        get { return (actor==null)? "Fade" : "Fade "+actor.name; }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        actor = (GameObject)EditorGUILayout.ObjectField("Object", actor, typeof(GameObject), true);
        starttime = EditorGUILayout.FloatField("Start", starttime);
        duration = EditorGUILayout.FloatField("Duration", duration);
        ease = (Ease)EditorGUILayout.EnumPopup("Ease Type", ease);
        fade = EditorGUILayout.FloatField("Fade to", fade);
        EditorGUILayout.EndVertical();
    }

#endif

    public override void addToSequence(Sequence seq)
    {
        if (actor==null) return;
        // Get fading component
        if (fader==null)
        {
            fader = (Component)actor.GetComponent<TextMeshPro>();
            if (fader==null) fader = (Component)actor.GetComponent<SpriteRenderer>();
            if (fader==null) fader = (Component)actor.GetComponent<Renderer>();
        }

        switch (fader)
        {
            case TextMeshPro textMeshPro:
                seq.Insert(starttime, textMeshPro.DOFade(fade, duration).SetEase(ease));
                break;
            case SpriteRenderer spriteRenderer:
                seq.Insert(starttime, spriteRenderer.DOFade(fade, duration).SetEase(ease));
                break;
            case Renderer renderer:
                seq.Insert(starttime, renderer.material.DOFade(fade, duration).SetEase(ease));
                break;
            default:
                Debug.LogWarning($"No fader component found on {actor.name}.");
                break;
        }

/*

        // Start fading tween
        if (fader!=null)
        {
            if (fader is TextMeshPro)
            {
                TextMeshPro textMeshPro = (TextMeshPro)fader;
                seq.Insert(starttime, textMeshPro.DOFade(fade, duration).SetEase(ease));
                break;
            }
            if (fader is SpriteRenderer)
            {

                seq.Insert(starttime, (SpriteRenderer)fader.DOFade(Fade, Duration).SetEase(EaseType));
                break;
            }
            if (fader is Material)
            {
                seq.Insert(starttime, (Renderer)fader.material.DOFade(Fade, Duration).SetEase(EaseType));
                break;
            }
            Debug.Warning("No fader component found on "+actor.name);
        }
*/
    }
}
