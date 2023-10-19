using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Checkpoint))]
public class CheckpointEditor : Editor
{
    private Checkpoint SelectedComponent { get { return serializedObject.targetObject as Checkpoint; } }
    private SerializedProperty beforeUnityEvent;
    private SerializedProperty afterUnityEvent;
    private SerializedProperty showGizmos;
    private SerializedProperty resize;
    private SerializedProperty tint;
    private bool foldout;
    private bool gizmoFoldout;

    void OnEnable()
    {
        beforeUnityEvent = serializedObject.FindProperty("BeforeSaving");
        afterUnityEvent = serializedObject.FindProperty("AfterSaving");
        showGizmos = serializedObject.FindProperty("showGizmos");
        resize = serializedObject.FindProperty("resize");
        tint = serializedObject.FindProperty("tint");
    }

    public override void OnInspectorGUI()
    {
        SelectedComponent.TriggerTag = EditorGUILayout.TagField(new GUIContent("TriggerTag", "The tag that triggers the checkpoint on a collision"), SelectedComponent.TriggerTag);

        foldout = EditorGUILayout.Foldout(foldout, "Events");

        if (foldout)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(beforeUnityEvent, new GUIContent("Before Saving"));
            EditorGUILayout.PropertyField(afterUnityEvent, new GUIContent("After Saving"));
            EditorGUILayout.EndVertical();
        }

        gizmoFoldout = EditorGUILayout.Foldout(gizmoFoldout, "Gizmos");

        if (gizmoFoldout)
        {
            EditorGUILayout.PropertyField(showGizmos, new GUIContent("show Gizmos"));
            if (showGizmos.boolValue)
            {
                EditorGUILayout.PropertyField(resize, new GUIContent("Resize Gizmos"));
                EditorGUILayout.PropertyField(tint, new GUIContent("tint Gizmos"));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
