using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Extensions.ClassExtensions;
using System.Reflection;

[CustomEditor(typeof(CheckpointComponent))]
public class CheckpointComponentEditor : Editor
{
    /// <summary>
    /// The currently selected component
    /// </summary>
    private CheckpointComponent SelectedComponent { get { return serializedObject.targetObject as CheckpointComponent; } }

    //private bool restrictedMode;

    #region Global BindingFlags
    private BindingFlags bf = BindingFlags.Public | BindingFlags.Instance; //If you dont know anything about reflection, dont change this
    /// <summary>
    /// If set to true, private members will be displayed as well
    /// </summary>
    private readonly bool UnrestrictedKey = false;
    #endregion

    #region Component Entries
    /// <summary>
    /// A list of fields/properties that should not get shown as saveable
    /// </summary>
    private readonly List<string> invalidAttributes = new List<string>()
    { "useGUILayout","runInEditMode", "hideFlags", "name", "tag" };

    /// <summary>
    /// Create a ComponentEntry for each component attached to the GameObject
    /// </summary>
    private void CreateEntries()
    {
        //UnrestrictedKey = EditorPrefs.GetBool("UnrestrictedKey");

        if (UnrestrictedKey)
        {
            bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            Debug.Log("Unrestricted Mode enabled");
        }
        else
        {
            bf = BindingFlags.Public | BindingFlags.Instance;
        }

        if (SelectedComponent.DataToStore == null)
            SelectedComponent.DataToStore = new List<CheckpointComponent.ComponentEntry>();

        //Add the gameobject
        CheckpointComponent.AttributeEntry[] attarr = new CheckpointComponent.AttributeEntry[]
        {
            new CheckpointComponent.AttributeEntry("enabled"),
            //new CheckpointComponent.AttributeEntry("name"),
            new CheckpointComponent.AttributeEntry("tag"),
            new CheckpointComponent.AttributeEntry("layer")
        };
        SelectedComponent.DataToStore.Add(new CheckpointComponent.ComponentEntry("GameObject", attarr));

        CheckpointComponent.ComponentEntry entry;
        List<CheckpointComponent.AttributeEntry> attEntries = new List<CheckpointComponent.AttributeEntry>();

        foreach (Component comp in AttachedComponents)
        {
            if (comp.GetType().Equals(typeof(CheckpointComponent)))
                continue;

            attEntries.Clear();
            foreach (string str in comp.GetAllWritableMembers(bf))
            {
                if (!invalidAttributes.Contains(str))
                {
                    attEntries.Add(new CheckpointComponent.AttributeEntry(str));
                }
            }
            //Sort the output
            attEntries.Sort((c1, c2) => { return c1.attributeName[0].CompareTo(c2.attributeName[0]); });

            entry = new CheckpointComponent.ComponentEntry(comp.GetType().Name, attEntries.ToArray());

            SelectedComponent.DataToStore.Add(entry);
        }

        if (foldoutState.Length != SelectedComponent.DataToStore.Count)
            foldoutState = new bool[SelectedComponent.DataToStore.Count];

        EditorUtility.SetDirty(target);
        EditorUtility.SetDirty(SelectedComponent);
    }

    private Component[] AttachedComponents;
    /// <summary>
    /// Returns all components attached to the gameobject this script is attached to (including itself)
    /// </summary>
    /// <returns></returns>
    private Component[] GetAllComponents()
    {
        return SelectedComponent.gameObject.GetComponents<Component>();
    }

    #region Draw methods
    /// <summary>
    /// The state of the created foldouts
    /// </summary>
    private bool[] foldoutState;
    private void DrawComponentEntries()
    {
        if (SelectedComponent.DataToStore == null || SelectedComponent.DataToStore.Count == 0) //Dont waste time drawing empty entries
            return;

        for (int i = 0; i < SelectedComponent.DataToStore.Count; i++)
        {
            CheckpointComponent.ComponentEntry currentEntry = SelectedComponent.DataToStore[i];
            foldoutState[i] = EditorGUILayout.Foldout(foldoutState[i], currentEntry.ComponentName);

            if (foldoutState[i])
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                foreach (CheckpointComponent.AttributeEntry attributeEntry in currentEntry.entries)
                {
                    DrawAttributeEntry(attributeEntry);
                }

                EditorGUILayout.EndVertical();
            }
        }
    }

    /// <summary>
    /// Draw a given attribute entry
    /// </summary>
    /// <param name="entry">The entry you want to draw</param>
    private void DrawAttributeEntry(CheckpointComponent.AttributeEntry entry)
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);

        EditorGUILayout.LabelField(entry.attributeName);
        entry.selected = EditorGUILayout.Toggle(entry.selected);

        EditorGUILayout.EndHorizontal();
    }
    #endregion
    #endregion

    public override void OnInspectorGUI()
    {
        if (SelectedComponent == null) //If there is no component, we dont need to draw anything
            return;

        serializedObject.Update();

        #region Foldout Entries
        if (foldoutState == null)
        {
            if (SelectedComponent.DataToStore != null)
                foldoutState = new bool[SelectedComponent.DataToStore.Count];
        }


        if (SelectedComponent.DataToStore != null)
        {
            EditorGUILayout.LabelField(new GUIContent("Data To Store", "Tick all the data that you want to store when the player hits a checkpoint"), EditorStyles.boldLabel);
            DrawComponentEntries();
        }
        #endregion

        #region Buttons
        if (GUILayout.Button("Refresh"))
        {
            //Clear old entries
            SelectedComponent.DataToStore = new List<CheckpointComponent.ComponentEntry>(); ;

            //Get all components once
            AttachedComponents = GetAllComponents();

            //Create all needed entries
            CreateEntries();
        }
        #endregion

        serializedObject.ApplyModifiedProperties();

        Repaint();
    }

}

