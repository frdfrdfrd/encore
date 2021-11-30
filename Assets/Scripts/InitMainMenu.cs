using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMainMenu : MonoBehaviour
{
    public GameObject _activateIfGameEnabled;
    public GameObject _activateIfGameDisabled;

    void Awake()
    {
        if(PlayerPrefs.HasKey("jam24_disable") && PlayerPrefs.GetInt("jam24_disable") == 1)
        {
            _activateIfGameEnabled.SetActive(false);
            _activateIfGameDisabled.SetActive(true);
        }
        else
        {
            _activateIfGameEnabled.SetActive(true);
            _activateIfGameDisabled.SetActive(false);
        }
    }

    public void Reboot()
    {
        PlayerPrefs.SetInt("jam24_disable", 0);
        _activateIfGameEnabled.SetActive(true);
        _activateIfGameDisabled.SetActive(false);

        AudioManager.instance.ChangeMainMusic(AudioManager.instance._keyMainMenu);
    }
}
