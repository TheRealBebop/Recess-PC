//If defined only serializable types will be allowed for storing (e.g. simple types)
#define ONLY_USE_SERIALIZABLE_TYPES

using System.Collections.Generic;
using UnityEngine;
using Extensions.ClassExtensions;
using EAssets.IO;

/// <summary>
/// This class handles the public saving/loading of checkpoint related data
/// </summary>
public static class CheckpointManager
{
    /// <summary>
    /// All data is stored in this stack
    /// </summary>
    private static Stack<GameState> CheckpointStates = new Stack<GameState>();

    /// <summary>
    /// Returns the count of gamestates saved
    /// </summary>
    public static int CheckpointStateCount { get { return CheckpointStates.Count; } }

    /// <summary>
    /// Stores an entry
    /// </summary>
    /// <param name="state"></param>
    private static void Save(GameState state)
    {
        CheckpointStates.Push(state);
    }

    /// <summary>
    /// Loads an entry (pops it from the stack)
    /// <para>Next time you want to load data, it will be the former gamestate</para>
    /// </summary>
    /// <returns></returns>
    public static GameState PopGameState()
    {
        if (CheckpointStates.Count > 0)
            return CheckpointStates.Pop();
        else
            return default;
    }

    /// <summary>
    /// Add data to the top state
    /// </summary>
    /// <param name="str">The (unique) name of the entry</param>
    /// <param name="cd">The list of data to store</param>
    public static void SetData(string str, List<ComponentData> cd)
    {
        GameState gs = CheckpointStates.Peek();
        if(gs == null)
        {
            CheckpointStates.Push(new GameState());
            gs = CheckpointStates.Peek();
        }

        gs.AddData(str, cd);
    }

    /// <summary>
    /// Returns the data that was stored
    /// </summary>
    /// <param name="str">The key used</param>
    /// <returns></returns>
    public static List<ComponentData> GetData(string str)
    {
        GameState gs = CheckpointStates?.Peek();
        if (gs == null)
        {
            return null;
        }

        return gs.GetData(str);
    }

    #region Global Save/Load
    /// <summary>
    /// A delegate for events happening before and after saving/loading.
    /// Remeber to remove methods before the registered script is destroyed
    /// </summary>
    public delegate void OnEvent();
    /// <summary>
    /// This event is triggered before data is saved
    /// </summary>
    public static event OnEvent OnBeforeSave;
    /// <summary>
    /// This event is triggered after data is saved
    /// </summary>
    public static event OnEvent OnAfterSave;
    /// <summary>
    /// This event is triggered before data is loaded
    /// </summary>
    public static event OnEvent OnBeforeLoad;
    /// <summary>
    /// This event is triggered after data is loaded
    /// </summary>
    public static event OnEvent OnAfterLoad;

    /// <summary>
    /// Calls every registered GameObject and lets the attached CheckpointComponent store its data
    /// </summary>
    public static void SaveNewCheckpoint()
    {
        //Create a new gamestate
        GameState state = new GameState();
        Save(state);

        OnBeforeSave?.Invoke();

        //Call every CheckpointComponent and let them store their data
        foreach (GameObject go in registeredObjects)
        {
            go.GetComponent<CheckpointComponent>().StoreData();
        }

        OnAfterSave?.Invoke();
    }

    /// <summary>
    /// Reloads all CheckpointComponents attached to the registered GameObjects
    /// </summary>
    public static void ReloadCheckpoint()
    {
        OnBeforeLoad?.Invoke();

        //Call every CheckpointComponent and let them reload their data
        foreach (GameObject go in registeredObjects)
        {
            go.GetComponent<CheckpointComponent>().ReloadData();
        }

        OnAfterLoad?.Invoke();
    }
    #endregion

    #region Register Methods
    private static List<GameObject> registeredObjects = new List<GameObject>();

    /// <summary>
    /// Register the given GameObject to the Checkpointmanager. This is needed for global saving and loading.
    /// <para>An object can only be registered when it has a Checkpoint Component attached</para>
    /// </summary>
    /// <param name="go">The Gameobject you want to register</param>
    /// <returns>True if successfully registered, else false</returns>
    public static bool RegisterGameObject(GameObject go)
    {
        if (go.GetComponent<CheckpointComponent>() == null)
            return false;

        if (!registeredObjects.Contains(go))
        {
            registeredObjects.Add(go);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register the given GameObject to the Checkpointmanager. This is needed for global saving and loading. 
    /// <para>By forcing a Checkpoint Component will be attached to the given GameObject if not attached already</para>
    /// </summary>
    /// <param name="go">The Gameobject you want to register</param>
    /// <returns></returns>
    public static void ForceRegisterGameObject(GameObject go)
    {
        if (go.GetComponent<CheckpointComponent>() == null)
            go.AddComponent<CheckpointComponent>();

        if (!registeredObjects.Contains(go))
        {
            registeredObjects.Add(go);
        }
    }

    /// <summary>
    /// Unregister a given GameObject from the Checkpointmanager
    /// </summary>
    /// <param name="go">The Gameobject you want to unregister</param>
    public static void UnregisterGameObject(GameObject go)
    {
        //Remove registered object
        if (registeredObjects.Contains(go))
            registeredObjects.Remove(go);
    }
    #endregion

    #region Save data

#if ONLY_USE_SERIALIZABLE_TYPES
    /// <summary>
    /// Store the current game states
    /// </summary>
    public static void StoreGameStates()
    {
        if(CheckpointStates != null && CheckpointStates.Count > 0)
        {
            DataIO.WriteDataJSON(CheckpointStates, "CheckpointData", "Checkpoints");
        }
    }

    /// <summary>
    /// Read the stored game state
    /// </summary>
    public static void LoadGameStates()
    {
        if(DataIO.ReadDataJSON(out Stack<GameState> data, "CheckpointData", "Checkpoints"))
        {
            CheckpointStates = data;
        }
    }
#endif

    #endregion
}

/// <summary>
/// This class contains one gamestate
/// </summary>
[System.Serializable]
public class GameState
{
    /// <summary>
    /// The gamestate data
    /// </summary>
    private Dictionary<string, List<ComponentData>> stateData;

    /// <summary>
    /// Create a new gamestate
    /// </summary>
    public GameState()
    {
        stateData = new Dictionary<string, List<ComponentData>>();
    }

    /// <summary>
    /// Add data to the gamestate
    /// </summary>
    /// <param name="str">The unique name of the entry</param>
    /// <param name="cd">The component data you want to add</param>
    public void AddData(string str, List<ComponentData> cd)
    {
        stateData.Add(str, cd);
    }

    /// <summary>
    /// Returns the data of the gamestate, stored for a given string
    /// </summary>
    /// <param name="str">The string that was used as key</param>
    /// <returns></returns>
    public List<ComponentData> GetData(string str)
    {
        return stateData[str];
    }

    /// <summary>
    /// Remove a given key, data pair from the gamestate
    /// </summary>
    /// <param name="str"></param>
    public void RemoveData(string str)
    {
        if (stateData.ContainsKey(str))
            stateData.Remove(str);
    }
}

/// <summary>
/// This struct contains the name of a component, an attribute name and the data of the attribute
/// </summary>
[System.Serializable]
public struct ComponentData
{
    /// <summary>
    /// The name of the component
    /// </summary>
    public string componentName;
    /// <summary>
    /// The name of the attribute of the component
    /// </summary>
    public string memberName;
    /// <summary>
    /// The data you want to store
    /// </summary>
    public object data;

    /// <summary>
    /// Create a new set of component data
    /// </summary>
    /// <param name="componentName">The name of the component</param>
    /// <param name="attributeName">The name of the attribute</param>
    /// <param name="data">The data to store</param>
    public ComponentData(string componentName, string attributeName, object data)
    {
        this.componentName = componentName;
        this.memberName = attributeName;
        this.data = data;

#if ONLY_USE_SERIALIZABLE_TYPES
        if (!IsSerializable(data))
        {
            throw new System.ArgumentException("The given data is non serializable: Type: " + data.GetType());
        }
#endif
    }

    /// <summary>
    /// Create a new set of component data using the name of the given component
    /// </summary>
    /// <param name="component">The component you want to use</param>
    /// <param name="attributeName">The name of the attribute to use</param>
    /// <param name="data">The data to store</param>
    public ComponentData(Component component, string attributeName, object data)
    {
        this.componentName = component.GetType().Name;
        this.memberName = attributeName;
        this.data = data;

#if ONLY_USE_SERIALIZABLE_TYPES
        if (!IsSerializable(data))
        {
            throw new System.ArgumentException("The given data is non serializable: Type: " + data.GetType());
        }
#endif
    }

    /// <summary>
    /// Create a new set of component data which gets its data directly from the component
    /// </summary>
    /// <param name="component">The component you want to use</param>
    /// <param name="attributeName">The name of the attribute to use</param>
    /// <param name="flags">The flags if needed in a different configuration</param>
    public ComponentData(Component component, string attributeName, System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
    {
        this.componentName = component.GetType().Name;
        this.memberName = attributeName;
        this.data = component.TryGetValue(attributeName, flags);

#if ONLY_USE_SERIALIZABLE_TYPES
        if (!IsSerializable(data))
        {
            throw new System.ArgumentException("The given data is non serializable: Type: " + data.GetType());
        }
#endif
    }

#if ONLY_USE_SERIALIZABLE_TYPES
    /// <summary>
    /// All allowed (serializable) types are listed here
    /// </summary>
    private static List<System.Type> additionalAllowedTypes = new List<System.Type>()
    {
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Rect),
        typeof(Matrix4x4),
        typeof(Vector2Int),
        typeof(Vector3Int),
        typeof(Quaternion),
        typeof(Color),
        typeof(Color32),
        typeof(LayerMask),
        typeof(AnimationCurve),
        typeof(Gradient),
        typeof(RectOffset),
        typeof(GUIStyle)
    };
    /// <summary>
    /// Returns true if the given object is serializable, else false
    /// </summary>
    /// <param name="o">The object you want to check</param>
    /// <returns></returns>
    private bool IsSerializable(object o)
    {
        if (o == null) //Null is not serializable since it could be anything
            return false;

        return o.GetType().IsSerializable || additionalAllowedTypes.Contains(o.GetType());
    }

#endif
}
