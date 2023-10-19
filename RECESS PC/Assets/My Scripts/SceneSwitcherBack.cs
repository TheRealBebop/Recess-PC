using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherBack : MonoBehaviour
{
    [SerializeField] int previousSceneIndex;
    bool sceneChange = false;

    GameObject player;
    int currentSceneIndex;
    Scene scene;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scene = SceneManager.GetActiveScene();
        int currentSceneIndex = scene.buildIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sceneChange = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && sceneChange == true)
        {
            Debug.Log("Scene Change");
            // SceneManager.LoadScene(NextScene);
            StartCoroutine(ChangeSceneBack());
        }
    }

    IEnumerator ChangeSceneBack()
    {
        SceneManager.LoadScene(previousSceneIndex, LoadSceneMode.Additive);
        Scene nextScene = SceneManager.GetSceneAt(previousSceneIndex);
        SceneManager.MoveGameObjectToScene(player, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(currentSceneIndex);
    }
}