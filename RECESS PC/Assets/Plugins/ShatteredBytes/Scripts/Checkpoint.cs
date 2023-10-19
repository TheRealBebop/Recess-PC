#define DEBUG
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Shattered Bytes/Checkpoint/Checkpoint")]
[RequireComponent(typeof(Collider)), DisallowMultipleComponent]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("The tag you want to use")]
    public string TriggerTag;

    /// <summary>
    /// A flag used to determine if this checkpoint has been used yet, or not
    /// </summary>
    private bool isTriggered;

    [SerializeField, Tooltip("This event is called right before the checkpoint is saved")]
    public ColliderEvent BeforeSaving;
    [Tooltip("This event is called after the checkpoint is saved")]
    public ColliderEvent AfterSaving;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true; //Set the collider to a trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag(TriggerTag))
        {
            BeforeSaving?.Invoke(other);
            SaveCheckpoint();
            AfterSaving?.Invoke(other);
        }
    }

    /// <summary>
    /// Trigger this checkpoint
    /// </summary>
    public void SaveCheckpoint()
    {
        CheckpointManager.SaveNewCheckpoint();
        isTriggered = true; //Set the flag to true
#if DEBUG
        Debug.Log("Checkpoint saved!");
#endif
    }

    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }

#if UNITY_EDITOR
    public bool showGizmos;
    public bool resize;
    public Color tint;
    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            string[] guid = AssetDatabase.FindAssets("t:Texture2D", new string[] { "Assets/Plugins/ShatteredBytes/Icons" });
            string path = "";

            if (guid.Length > 0)
            {
                foreach (string id in guid)
                {
                    path = AssetDatabase.GUIDToAssetPath(id);

                    if (path.Contains("CheckpointIcon"))
                        break;
                    else
                        path = null;
                }
            }

            if (path != null && path != string.Empty)
                Gizmos.DrawIcon(transform.position, path, resize, tint);
        }
    }
#endif
}
