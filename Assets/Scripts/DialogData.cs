using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "ScriptableObjects/DialogData", order = 1)]
public class DialogData : ScriptableObject
{

    public enum DialogType
    {
        MAIN,
        DIDASCALIE,
    }

    public enum TriggerType
    {
        PLAYER_CLICK,
        GAME,
    }

    public enum DialogState
    {
        WAIT_TRIGGER,
        PENDING,
        OVER
    }

    public DialogType _type = DialogType.MAIN;

    [Header("Order")]
    public int _mainStep = 0;
    public int _subStep = 0;

    [Header("Content")]
    public string _text;

    [Header("Audio")]
    public string _audioKeyOpenCloseChannel = "playerWalkieStatic";
    public string _audioKeyDialogVoice = "playerWalkieOn";
    public string _audioKeyDidaPopup = "didascalie";

    [Header("Trigger")]
    public TriggerType _whoTriggers = TriggerType.PLAYER_CLICK;
    public float _delayAfterPreviousToTriggerByGame = 2;

    [Header("Duration")]
    public float _introSilentDuration =1;
    public float _textDuration = 5;
    public float _outroSilentDuration=1;

    [Header("ButtonState")]
    public bool _setInteractableWhenOver = true;
    public float _setInteractableDelay= 1;

    [Header("END DIALOG OPTION")]
    public bool _increaseStoryAndAnimatorProgress = false;

    [Header("INTERACTION")]
    public bool _stopPlayerInteractionWhileMessage = true;

    [Header("INTERACTION")]
    public int _glitchID = -1;
    public float _waitSecForFX = 1;
}
