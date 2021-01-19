using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;

namespace DOTweenSequencer
{

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
        public List<DOTweenAction> deleteActions = new List<DOTweenAction>();

        // OnInspector GUI
        public override void OnInspectorGUI() //2
        {
            DOTweenSequencer doTweenSequencer = (DOTweenSequencer) target;

            // Button Row
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            if (GUILayout.Button("Add Move"))
            {
                DOTweenAction action = new DOTweenAction();
                action.type = DOTweenActionTypes.Move;
                doTweenSequencer.actions.Add( action );
            }
            if (GUILayout.Button("Add Rotate"))
            {
                DOTweenAction action = new DOTweenAction();
                action.type = DOTweenActionTypes.Rotate;
                doTweenSequencer.actions.Add( action );
            }
            if (GUILayout.Button("Add Scale"))
            {
                DOTweenAction action = new DOTweenAction();
                action.type = DOTweenActionTypes.Scale;
                doTweenSequencer.actions.Add( action );
            }
            if (GUILayout.Button("Add Fade"))
            {
                DOTweenAction action = new DOTweenAction();
                action.type = DOTweenActionTypes.Fade;
                doTweenSequencer.actions.Add( action );
            }
            EditorGUILayout.EndHorizontal();

            // Clean list
            if (deleteActions!=null)
            {
                foreach (DOTweenAction action in deleteActions) doTweenSequencer.actions.Remove(action);
                deleteActions.Clear();
            }

            // List of Tween UI's
            scrollPos = EditorGUILayout.BeginScrollView( scrollPos, EditorStyles.helpBox, GUILayout.Height(400));
            if (doTweenSequencer.actions!=null)
            {
                foreach(DOTweenAction action in doTweenSequencer.actions)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    action.showPosition=EditorGUILayout.Foldout(action.showPosition, action.GUIName);
                    if (GUILayout.Button("X", GUILayout.Width(15), GUILayout.Width(20))) deleteActions.Add(action);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                    if (action.showPosition) action.OnInspectorGUI();
                }
            }
            EditorGUILayout.EndScrollView();

            // Play Button, only active during game time
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            if (GUILayout.Button("Play Sequence")) doTweenSequencer.Play();
            EditorGUI.EndDisabledGroup();

            // DrawDefaultInspector();
        }
    }
}