using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerZone : MonoBehaviour
{
    public string audioKeyToStart = "";
    [HideInInspector] public bool _isActivated = true;
    public bool _stopOnExitTriggerZone = true;

    private void OnTriggerEnter(Collider collision)
    {
        if (AudioManager.instance && _isActivated) AudioManager.instance.PlaySound(audioKeyToStart);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (AudioManager.instance && _isActivated && _stopOnExitTriggerZone) AudioManager.instance.StopSound(audioKeyToStart);
    }
}
