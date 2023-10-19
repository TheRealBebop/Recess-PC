using System.Collections.Generic;
using UnityEngine;
using Extensions.ClassExtensions;

/// <summary>
/// Attach this object to any GameObject to select the data you want to store
/// <para>While in runtime this should work for any type of data</para>
/// <para>Do not try to save this data, since not all Unitydata can get serialized by default!</para>
/// <para>Testing: Works for int, float, string, bool, VectorX, Quaternion, GameObject, Transform </para>
/// </summary>
[AddComponentMenu("Shattered Bytes/Checkpoint/Checkpointdata Component")]
[DisallowMultipleComponent]
public class CheckpointComponent : MonoBehaviour
{
    /// <summary>
    /// This data is used as a basis for the creation of the ManagerData
    /// <para>The ManagerData contains only selected AttributeEntries</para>
    /// </summary>
    public List<ComponentEntry> DataToStore = new List<ComponentEntry>();

    /// <summary>
    /// This data is intended to get send to the CheckpointManager
    /// </summary>
    private List<ComponentData> ManagerData;

    /// <summary>
    /// Used for user defined data to be saved on the checkpoint
    /// </summary>
    private List<ComponentData> AdditionalManagerData;

    #region Storing/Loading Methods
    /// <summary>
    /// Store all data that is meant to be stored in the ManagerData object
    /// </summary>
    private void GetLocalData()
    {
        if (this.DataToStore == null || this.DataToStore.Count == 0)
        {
            return;
        }

        ManagerData = new List<ComponentData>();
        foreach (ComponentEntry entry in this.DataToStore)
        {
            if (entry.ComponentName.Equals("GameObject")) //Store GameObject data
            {
                foreach (AttributeEntry attribute in entry.entries)
                {
                    if (attribute.selected)
                    {
                        GameObject comp = gameObject; //Get the component
                        object value = null;

                        switch (attribute.attributeName) //Get the data
                        {
                            case "enabled": value = gameObject.activeSelf; break;
                            case "tag": value = gameObject.tag; break;
                            case "layer": value = gameObject.layer; break;
                            case "name": value = gameObject.name; break;
                            default: break;
                        }

                        //If we found something we can write it
                        if (comp != null)
                            ManagerData.Add(new ComponentData(entry.ComponentName, attribute.attributeName, value));
                    }
                }
            }
            else //Store other data
            {
                foreach (AttributeEntry attribute in entry.entries)
                {
                    if (attribute.selected)
                    {
                        Component comp = GetComponent(entry.ComponentName); //Get the component
                        object value = comp?.TryGetValue(attribute.attributeName); //Using reflection we read the data

                        //if needed, change the data here

                        //If we found something we can write it
                        if (comp != null)
                            ManagerData.Add(new ComponentData(entry.ComponentName, attribute.attributeName, value));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets an additional set of data to be stored when a checkpoint is reached
    /// </summary>
    /// <param name="entry">The entry you want to store</param>
    public void SetAdditionalLocalData(ComponentData entry)
    {
        Component comp = GetComponent(entry.componentName);
        if (comp == null)
        {
            Debug.LogError("No component found with the name: " + entry.componentName + " at: " + name);
            return;
        }

        if (comp.GetType().GetField(entry.memberName) == null &&
           comp.GetType().GetProperty(entry.memberName) == null)
        {
            Debug.LogError("No member found with the name: " + entry.memberName + " in the component: " + entry.componentName);
            return;
        }

        if (AdditionalManagerData != null)
            AdditionalManagerData.Add(entry);
        else
        {
            AdditionalManagerData = new List<ComponentData>
            {
                entry
            };
        }
    }

    /// <summary>
    /// Sets an additional set of data to be stored when a checkpoint is reached
    /// </summary>
    /// <param name="componentName">The name of the component to store</param>
    /// <param name="memberName">The name of the member to store</param>
    /// <param name="overwriteData">If true the value of <see cref="inputValue"/> will be used instead of the read value</param>
    /// <param name="inputValue">The value of the member you want to store. This will only be used when <see cref="overwriteData"/> is set to true</param>
    public void SetAdditionalLocalData(string componentName, string memberName, bool overwriteData = false, object inputValue = null)
    {
        if (!overwriteData) //If we are not overwriting, we read the member given to us for a value
        {
            inputValue = GetComponent(componentName).TryGetValue(memberName);
        }

        SetAdditionalLocalData(new ComponentData(componentName, memberName, inputValue));
    }

    /// <summary>
    /// Sets an additional set of data to be stored when a checkpoint is reached
    /// </summary>
    /// <param name="comp">The component you want to store</param>
    /// <param name="memberName">The name of the member</param>
    /// <param name="overwriteData">If true the value of <see cref="inputValue"/> will be used instead of the read value</param>
    /// <param name="inputValue">The value of the member you want to store. This will only be used when <see cref="overwriteData"/> is set to true</param>
    public void SetAdditionalLocalData(Component comp, string memberName, bool overwriteData = false, object inputValue = null)
    {
        SetAdditionalLocalData(comp.GetType().Name, memberName, overwriteData, inputValue);
    }

    /// <summary>
    /// Store the data of the components attached to this object
    /// </summary>
    public void StoreData()
    {
        GetLocalData();

        if (AdditionalManagerData != null && AdditionalManagerData.Count > 0)
            ManagerData.AddRange(AdditionalManagerData);

        CheckpointManager.SetData(gameObject.name, ManagerData);
    }

    /// <summary>
    /// Reload stored data to the components attached
    /// </summary>
    public void ReloadData()
    {
        ManagerData = CheckpointManager.GetData(gameObject.name);
        if (ManagerData != null && ManagerData.Count > 0)
        {
            foreach (ComponentData entry in ManagerData)
            {
                SetEntry(entry);
            }
        }
    }

    /// <summary>
    /// Sets the field/property of a component using an entry
    /// </summary>
    /// <param name="entry"></param>
    private void SetEntry(ComponentData entry)
    {
        if (entry.componentName.Equals("GameObject"))
        {
            //Debug.Log("Found GameObject");

            //Get the component
            GameObject comp = gameObject;
            object value = entry.data;

            switch (entry.memberName) //Get the data
            {
                case "enabled": gameObject.SetActive((bool)value); break;
                case "tag": gameObject.tag = (string)value; break;
                case "layer": gameObject.layer = (int)value; break;
                //case "name": gameObject.name = (string)value; break;
                default: break;
            }
        }
        else
        {
            //Get the component
            Component comp = GetComponent(entry.componentName);

            //Set its value
            if (comp != null)
                comp.TrySetValue(entry.memberName, entry.data);
        }
    }
    #endregion

    #region Manager Logic
    /// <summary>
    /// When the gameobject gets created we would like to add it to the Manager
    /// </summary>
    private void Awake()
    {
        //Register component
        Register();
    }

    /// <summary>
    /// When the gameobject gets destroyed we would like to remove it from the Manager
    /// </summary>
    private void OnDestroy()
    {
        //Unregister component
        Unregister();
    }

    /// <summary>
    /// Register this object to the CheckpointManager
    /// </summary>
    private void Register()
    {
        CheckpointManager.RegisterGameObject(gameObject);
    }

    /// <summary>
    /// Unregister this object from the CheckpointManager
    /// </summary>
    private void Unregister()
    {
        CheckpointManager.UnregisterGameObject(gameObject);
    }
    #endregion

    #region Entries
    [System.Serializable]
    /// <summary>
    /// Contains the name of the component and all its writeable fields and properties
    /// </summary>
    public class ComponentEntry
    {
        /// <summary>
        /// The name of the component
        /// </summary>
        public string ComponentName;
        /// <summary>
        /// A list of all allowed attributes and if they are selected for storing
        /// </summary>
        public List<AttributeEntry> entries;

        /// <summary>
        /// Create a new ComponentEntry
        /// </summary>
        /// <param name="compName">The name of the component</param>
        /// <param name="ent">The attribute entries</param>
        public ComponentEntry(string compName, params AttributeEntry[] ent)
        {
            this.ComponentName = compName != string.Empty ? compName : "<empty>";
            this.entries = new List<AttributeEntry>(ent);
        }

        /// <summary>
        /// Print all attribute entries to the console
        /// </summary>
        public void Print()
        {
            Debug.Log(this.ComponentName);
            foreach (AttributeEntry ent in this.entries)
            {
                Debug.Log(ent.attributeName);
            }
            Debug.Log("---");
        }
    }

    [System.Serializable]
    /// <summary>
    /// Contains the name of an attribute and wether it is selcted or not
    /// </summary>
    public class AttributeEntry
    {
        /// <summary>
        /// The name of the attribute
        /// </summary>
        public string attributeName;
        /// <summary>
        /// Whether or not this attribute should be saved
        /// </summary>
        public bool selected;

        /// <summary>
        /// Create a new AttributeEntry
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="sel">The default state wether to save it or not</param>
        public AttributeEntry(string name = "", bool sel = false)
        {
            attributeName = name;
            selected = sel;
        }
    }
    #endregion
}
