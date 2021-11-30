using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = false;
        StartCoroutine(WaitAndGoToMain());
    }
    IEnumerator WaitAndGoToMain()
    {
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}
