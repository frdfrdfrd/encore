using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelUI : MonoBehaviour
{
    public void LoadLevel(int i)
    {
        StartCoroutine(WaitAndLoadRoutine(i));
    }

    public void LoadGame()
    {
        StartCoroutine(WaitAndLoadRoutine(2));
    }

    public void LoadMainMenu()
    {
        if (_loading == false)
        {
            StartCoroutine(WaitAndLoadRoutine(1));
        }
    }


    bool _loading = false;
    IEnumerator WaitAndLoadRoutine(int id)
    {
        _loading = true;
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);

    }
}
