using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class DOTweenBaseAction : ScriptableObject
{
    #region EditorOnly
    #if UNITY_EDITOR
    [HideInInspector]
    public bool showPosition = true;
    public abstract string Name { get; }
    public abstract void OnInspectorGUI();
    #endif
    #endregion
    
    /**
     * <summary>
     * Adds the action to a DOTween-sequence
     * </summary>
     * <param name="seq"></param>
     */
    public abstract void addToSequence(Sequence seq);
}
