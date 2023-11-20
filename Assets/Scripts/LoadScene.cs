using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadNewScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }


    private static IEnumerator LoadLevel(string sceneName)
    {
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
    }
}
