using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        var spawn = GameObject.FindWithTag("SpawnPoint");
        if (spawn == null)
        {
            Debug.Log("Pas de spawn point defini, spawn au pif ig");
        }
        transform.position = spawn.transform.position;
    }
}
