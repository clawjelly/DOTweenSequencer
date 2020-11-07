using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;

/**
* <summary>
* Custom editor for DOTweenSequencer class.
* </summary>
*/
[CustomEditor(typeof(DOTweenSequencer))] [HelpURL("http://manuals.clawjelly.net/Unity/UnityAttributes/")]
public class DOTweenSequencerEditor: Editor
{
    bool showPosition = true;
    private Vector2 scrollPos = Vector2.zero;
    public List<DOTweenBaseAction> deleteActions = new List<DOTweenBaseAction>();

    // OnInspector GUI
    public override void OnInspectorGUI() //2
    {
        DOTweenSequencer doTweenSequencer = (DOTweenSequencer) target;

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        if (GUILayout.Button("Add Move")) doTweenSequencer.actions.Add( (DOTweenBaseAction)ScriptableObject.CreateInstance("DOTweenMove") );
        if (GUILayout.Button("Add Rotate")) doTweenSequencer.actions.Add( (DOTweenBaseAction)ScriptableObject.CreateInstance("DOTweenRotate"));
        if (GUILayout.Button("Add Scale")) doTweenSequencer.actions.Add( (DOTweenBaseAction)ScriptableObject.CreateInstance("DOTweenScale"));
        if (GUILayout.Button("Add Fade")) doTweenSequencer.actions.Add( (DOTweenBaseAction)ScriptableObject.CreateInstance("DOTweenFade"));
        EditorGUILayout.EndHorizontal();

        // clean list
        if (deleteActions!=null)
        {
            foreach (DOTweenBaseAction action in deleteActions) doTweenSequencer.actions.Remove(action);
            deleteActions.Clear();
        }

        // List of Tween UI's
        scrollPos = EditorGUILayout.BeginScrollView( scrollPos, EditorStyles.helpBox, GUILayout.Height(400));
        if (doTweenSequencer.actions!=null)
        {
            foreach(DOTweenBaseAction action in doTweenSequencer.actions)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                action.showPosition=EditorGUILayout.Foldout(action.showPosition, action.Name);
                if (GUILayout.Button("X", GUILayout.Width(15), GUILayout.Width(20))) deleteActions.Add(action);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                if (action.showPosition) action.OnInspectorGUI();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        if (GUILayout.Button("Play Sequence")) doTweenSequencer.Play();
        EditorGUI.EndDisabledGroup();
    }
}