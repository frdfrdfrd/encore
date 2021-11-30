using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableChangeAudio : InteractableScript
{
    bool _isStarted;

    public TypeChange _type = TypeChange.CHANGE_MAIN;
    public enum TypeChange
    {
        CHANGE_MAIN,
        PLAY_SOUND,
        PARAMETER_SET,
        STOP_MUSIC
    }
    public string audioKeyToStart = "";
    public int paramVal = 0;

    public override void MyStart()
    {
        _isStarted = false;
    }

    public override void PlayerInteraction()
    {
        if (!_isStarted)
        {
            _isStarted = true;
            if (AudioManager.instance)
            {

                if(_type == TypeChange.CHANGE_MAIN) AudioManager.instance.ChangeMainMusic(audioKeyToStart);
                else if(_type == TypeChange.PLAY_SOUND) AudioManager.instance.PlaySound(audioKeyToStart);
                else if (_type == TypeChange.PARAMETER_SET) AudioManager.instance.PlaySound("Level", paramVal);
                else if (_type == TypeChange.STOP_MUSIC) AudioManager.instance.StopSound("game");
            }
        }
    }
}
