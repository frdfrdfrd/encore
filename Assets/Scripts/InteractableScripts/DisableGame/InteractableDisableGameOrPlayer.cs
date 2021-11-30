using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableDisableGameOrPlayer : InteractableScript
{
    bool _isStarted;
    public GameObject _UIChoiceToShow;
    public GameObject _UIToFade;


    public override void MyStart()
    {
        _isStarted = false;
    }

    public override void PlayerInteraction()
    {
        if (!_isStarted)
        {
            base.PlayerInteraction();
            _isStarted = true;

            _UIChoiceToShow.SetActive(true);

        }
    }

    public void DisableGame()
    {
        StartCoroutine(EndCinematic(true));
    }
    public void DisablePlayer()
    {
        StartCoroutine(EndCinematic(false));
    }

    IEnumerator EndCinematic(bool alsoDisableGame)
    {
        _UIToFade.SetActive(true);
        yield return new WaitForSeconds(2);

        if (alsoDisableGame) PlayerPrefs.SetInt("jam24_disable", 1);

       SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

    }
}
