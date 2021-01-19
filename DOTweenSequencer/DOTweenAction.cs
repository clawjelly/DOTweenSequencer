using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using TMPro;

namespace DOTweenSequencer
{
    public enum DOTweenActionTypes
    {
        Move,
        Rotate,
        Scale,
        Fade
    }

    [System.Serializable]
    public class DOTweenAction
    {
        #region Public
        public DOTweenActionTypes type;
        [SerializeField]
        private GameObject actor = null;
        [SerializeField]
        private Vector3 vecData = Vector3.zero;
        [SerializeField]
        private Vector3 startVecData = Vector3.zero;
        [SerializeField]
        private float fade = 0f;
        [SerializeField]
        [HideInInspector]
        private Component fader = null;
        [SerializeField]
        private Ease ease = Ease.Linear;
        [SerializeField]
        private float duration = 1.0f;
        [SerializeField]
        private float starttime = 0.0f;
        [SerializeField]
        private bool doReset = false;
        [SerializeField]
        private bool relative = false;
        #endregion

        #region EditorOnly
        #if UNITY_EDITOR
        /**
         *  This is the Editor GUI representation of the tween.
         */
        public bool showPosition = true;
        public string GUIName 
        {
            get { return (actor==null)? $"{type}" : $"{type} {actor.name}, start at {starttime} for {duration}s"; }
        }
        
        public void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            actor = (GameObject)EditorGUILayout.ObjectField("Object", actor, typeof(GameObject), true);
            starttime = EditorGUILayout.FloatField("Start", starttime);
            duration = EditorGUILayout.FloatField("Duration", duration);
            ease = (Ease)EditorGUILayout.EnumPopup("Ease Type", ease);
            string rel = relative ? "Relative to Parent" : "Relative to Start";
            switch (type)
            {
                case DOTweenActionTypes.Move:
                    vecData = EditorGUILayout.Vector3Field("Position", vecData);
                    relative = EditorGUILayout.Toggle(rel, relative);
                    doReset = EditorGUILayout.Toggle("Reset on Complete", doReset);
                    break;
                case DOTweenActionTypes.Rotate:
                    vecData = EditorGUILayout.Vector3Field("Rotation", vecData);
                    relative = EditorGUILayout.Toggle("Relative Rotation", relative);
                    doReset = EditorGUILayout.Toggle("Reset on Complete", doReset);
                    break;
                case DOTweenActionTypes.Scale:
                    vecData = EditorGUILayout.Vector3Field("Scale", vecData);
                    doReset = EditorGUILayout.Toggle("Reset on Complete", doReset);
                    break;
                case DOTweenActionTypes.Fade:
                    fade = EditorGUILayout.FloatField("Fade to", fade);
                    break;
            }
            EditorGUILayout.EndVertical();
        }
        #endif
        #endregion
        
        /**
        * <summary>
        * Adds the action to a DOTween-sequence
        * </summary>
        * <param name="seq"></param>
        */
        public void addToSequence(Sequence seq)
        {
            switch (type)
            {
                case DOTweenActionTypes.Move:
                    if (doReset) startVecData = actor.transform.localPosition;
                    if (actor == null) return;
                    if (relative)
                        seq.Insert(starttime, actor.transform
                                .DOLocalMove(vecData, duration)
                                .SetEase(ease)
                                .OnComplete(reset));
                    else
                        seq.Insert(starttime, actor.transform
                                .DOLocalMove(vecData + startVecData, duration)
                                .SetEase(ease)
                                .OnComplete(reset));
                    break;
                case DOTweenActionTypes.Rotate:
                    if (doReset) startVecData = actor.transform.rotation.eulerAngles;
                    if (actor == null) return;
                    if (relative)
                        seq.Insert(starttime, actor.transform
                            .DOLocalRotate(vecData, duration, RotateMode.FastBeyond360)
                            .SetEase(ease)
                            .OnComplete(reset));
                    else
                        seq.Insert(starttime, actor.transform
                            .DORotate(vecData, duration, RotateMode.FastBeyond360)
                            .SetEase(ease)
                            .OnComplete(reset));
                    break;
                case DOTweenActionTypes.Scale:
                    if (doReset) startVecData = actor.transform.localScale;
                    if (actor == null) return;
                    seq.Insert(starttime, actor.transform
                        .DOScale(vecData, duration)
                        .SetEase(ease)
                        .OnComplete(reset));
                    break;
                case DOTweenActionTypes.Fade:
                    if (actor == null) return;
                    // Get fading component
                    if (fader == null)
                    {
                        fader = (Component)actor.GetComponent<TextMeshPro>();
                        if (fader == null) fader = (Component)actor.GetComponent<SpriteRenderer>();
                        if (fader == null) fader = (Component)actor.GetComponent<Renderer>();
                    }

                    switch (fader)
                    {
                        case TextMeshPro textMeshPro:
                            seq.Insert(starttime, textMeshPro.DOFade(fade, duration).SetEase(ease).OnComplete(reset));
                            break;
                        case SpriteRenderer spriteRenderer:
                            seq.Insert(starttime, spriteRenderer.DOFade(fade, duration).SetEase(ease).OnComplete(reset));
                            break;
                        case Renderer renderer:
                            seq.Insert(starttime, renderer.material.DOFade(fade, duration).SetEase(ease).OnComplete(reset));
                            break;
                        default:
                            Debug.LogWarning($"No fader component found on {actor.name}.");
                            break;
                    }
                    break;
            }
        }

        public void reset()
        {
            if (!doReset) return;
            switch (type)
            {
                case DOTweenActionTypes.Move:
                    actor.transform.localPosition = startVecData;
                    break;
                case DOTweenActionTypes.Rotate:
                    actor.transform.rotation = Quaternion.Euler(startVecData);
                    break;
                case DOTweenActionTypes.Scale:
                    actor.transform.localScale = startVecData;
                    break;
                case DOTweenActionTypes.Fade:
                    break;
            }
        }
    }
}