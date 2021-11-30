using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int _currentQuestID = 0 ;
    int _currentSubQuestID = 0;
    //int _currentDidaID = 0;
    public Button _uiRegieButton;
    public float _waitBefore = 1.0f;
    public Animator animator;
    public DialogData.DialogState _currentDialogState;
   // DialogData.DialogState _currentDidascalieState;

    public List<DialogData> _dialogsList;
    float _tiggerAutoTimer = 0;
    [HideInInspector] public Dictionary<int, List<DialogData>> _dialogDico;
    [HideInInspector] public Dictionary<int, List<bool>> _triggerOnceDico;

    bool _canCallOreillette = false;

    [Header("New Bulle")]
    public GameObject _newBulleParent;
    public TMPro.TextMeshProUGUI _newBulleText;
    public KeyCode _regieCall = KeyCode.T;

    // [HideInInspector] public Dictionary<int, List<DialogData>> _didascalieDico;
    // [HideInInspector] public Dictionary<int, List<bool>> _triggerOnceDidaDico;

    [Header("Didascalie")]
    public GameObject _didascaliePopup;
    public TMPro.TextMeshProUGUI __didascalieText;


    [Header("Other")]
    public Animator _animatorIncreaseStory;

    private static GameManager mInstance;
    public static GameManager instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            return mInstance;
        }
    }

    private void Start()
    {
        _canCallOreillette = false;
        if (_newBulleParent) _newBulleParent.SetActive(false);
       if(_didascaliePopup != null) _didascaliePopup.SetActive(false);
        _currentQuestID = 0;
        _currentSubQuestID = 0;
        //_currentDidaID = 0;
        _dialogDico = new Dictionary<int, List<DialogData>>();
        //_didascalieDico = new Dictionary<int, List<DialogData>>();
        _triggerOnceDico = new Dictionary<int, List<bool>>();
       // _triggerOnceDidaDico = new Dictionary<int, List<bool>>();
        foreach (DialogData data in _dialogsList)
        {
            //if (data._type == DialogData.DialogType.MAIN)
            {
                if (!_dialogDico.ContainsKey(data._mainStep))
                {
                    _dialogDico.Add(data._mainStep, new List<DialogData>());
                    _triggerOnceDico.Add(data._mainStep, new List<bool>());
                }

                _dialogDico[data._mainStep].Add(data);
                _triggerOnceDico[data._mainStep].Add(false);
            }
            //else if (data._type == DialogData.DialogType.DIDASCALIE)
            //{
            //    if (!_didascalieDico.ContainsKey(data._mainStep))
            //    {
            //        _didascalieDico.Add(data._mainStep, new List<DialogData>());
            //        _triggerOnceDidaDico.Add(data._mainStep, new List<bool>());
            //    }
            //    _didascalieDico[data._mainStep].Add(data);
            //    _triggerOnceDidaDico[data._mainStep].Add(false);
            //}
        }

        List<int> keys = _dialogDico.Keys.ToList();
        foreach (int key in keys)
        {
            _dialogDico[key] = _dialogDico[key].OrderBy(data => data._subStep).ToList();
        }
        _currentDialogState = DialogData.DialogState.WAIT_TRIGGER;

        //List<int> keysDidas = _didascalieDico.Keys.ToList();
        //foreach (int key in keysDidas)
        //{
        //    _didascalieDico[key] = _didascalieDico[key].OrderBy(data => data._subStep).ToList();
        //}
        //_currentDidascalieState = DialogData.DialogState.WAIT_TRIGGER;

        _tiggerAutoTimer = 0;
    }

    //public string GetCurrentObjKey()
    //{
    //    string key = "";
    //    if (_currentQuestID < _objectivesNameList.Count) key = _objectivesNameList[_currentQuestID];

    //    //Debug.Log(key);

    //    return key;
    //}

    bool CurrentDialogExists(DialogData.DialogType type)
    {
      /*if(type == DialogData.DialogType.MAIN)*/ return _dialogDico.ContainsKey(_currentQuestID) && _currentSubQuestID>=0 &&  _currentSubQuestID < _dialogDico[_currentQuestID].Count && _dialogDico[_currentQuestID][_currentSubQuestID]._type == type;
      // else if (type == DialogData.DialogType.DIDASCALIE) return _didascalieDico.ContainsKey(_currentQuestID) && _currentDidaID < _didascalieDico[_currentQuestID].Count;

       // return false;
    }

    private void Update()
    {
       
        if(CurrentDialogExists(DialogData.DialogType.MAIN))
        {
            if(_canCallOreillette && Input.GetKeyDown(_regieCall)) ButtonRegieCooldown();

            // new to avoid bug
            if (!_canCallOreillette && _currentDialogState == DialogData.DialogState.WAIT_TRIGGER && _dialogDico[_currentQuestID][_currentSubQuestID]._whoTriggers == DialogData.TriggerType.PLAYER_CLICK)
            {
                _uiRegieButton.interactable = true;
                _canCallOreillette = true;
            }
        }

       // if (CurrentDialogExists(DialogData.DialogType.MAIN)) Debug.Log(_currentQuestID + " / " + _currentSubQuestID + " --> " + _tiggerAutoTimer + " / " + _currentDialogState + " / "+ _dialogDico[_currentQuestID][_currentSubQuestID]._whoTriggers.ToString() + " / trigger once ? :"+_triggerOnceDico[_currentQuestID][_currentSubQuestID].ToString() + " / " + _dialogDico[_currentQuestID][_currentSubQuestID].name);
        //else Debug.Log("no dialog");

        // Debug.Log(CurrentDialogExists(DialogData.DialogType.MAIN) + " /  " + _currentQuestID + " / " + _currentSubQuestID + _currentDialogState.ToString() + " /" + _dialogDico[_currentQuestID][_currentSubQuestID]._whoTriggers.ToString() + " / " + _tiggerAutoTimer +  " --> " + _triggerOnceDico[_currentQuestID][_currentSubQuestID]);
        if (CurrentDialogExists(DialogData.DialogType.MAIN) && _dialogDico[_currentQuestID][_currentSubQuestID]._whoTriggers == DialogData.TriggerType.GAME && _currentDialogState == DialogData.DialogState.WAIT_TRIGGER )
        {
            if (_triggerOnceDico[_currentQuestID][_currentSubQuestID] == false) // forbid retrigger dialog by game if last of the quest before resolution
            {
                _uiRegieButton.interactable = false;
                _tiggerAutoTimer += Time.deltaTime;
                if (_tiggerAutoTimer > _dialogDico[_currentQuestID][_currentSubQuestID]._delayAfterPreviousToTriggerByGame)
                {
                    _triggerOnceDico[_currentQuestID][_currentSubQuestID] = true;
                    ButtonRegieCooldown();//MainPlayerScript.instance.PlayerAskObjective();
                    
                }
            }
        }
        else if (CurrentDialogExists(DialogData.DialogType.DIDASCALIE) && _currentDialogState == DialogData.DialogState.WAIT_TRIGGER)
        {
            //if (_triggerOnceDidaDico[_currentQuestID][_currentDidaID] == false) // forbig retrigger dialog by game if last of the quest before resolution
            if (_triggerOnceDico[_currentQuestID][_currentSubQuestID] == false)
            {
                _tiggerAutoTimer += Time.deltaTime;
                if (_tiggerAutoTimer > _dialogDico[_currentQuestID][_currentSubQuestID]._delayAfterPreviousToTriggerByGame)
                {
                    ShowDida();
                    _triggerOnceDico[_currentQuestID][_currentSubQuestID] = true;
                }
            }
        }
    }


    public void IncreaseCurrentObj()
    {
        _currentQuestID++;
        _currentSubQuestID = 0;
        //Debug.Log("IncreaseCurrentObj -> " + _currentQuestID + " / " + _currentSubQuestID);
    }

    public int GetDialogueStep()
    {
        return _currentSubQuestID;
    }

    void ButtonRegieCooldown()
    {
        //Debug.Log("ButtonRegieCooldown " + CurrentDialogExists(DialogData.DialogType.MAIN).ToString());
        if (CurrentDialogExists(DialogData.DialogType.MAIN) && _currentDialogState == DialogData.DialogState.WAIT_TRIGGER) StartCoroutine(ButtonRegieCooldownRoutine());
    }

    public void UI_CallOreillette()
    {
        if (_canCallOreillette && CurrentDialogExists(DialogData.DialogType.MAIN) /*&& _dialogDico[_currentQuestID][_currentSubQuestID]._whoTriggers == DialogData.TriggerType.PLAYER_CLICK*/)
        {
            ButtonRegieCooldown();
        }
    }

    public void ForbidCallOreillette()
    {
        _canCallOreillette = false;
    }

    void ShowDida()
    {
        StartCoroutine(ShowDidaRoutine());
    }

    IEnumerator ShowDidaRoutine()
    {
        DialogData dialog = _dialogDico[_currentQuestID][_currentSubQuestID];
        if (__didascalieText) __didascalieText.text = "";

        _currentDialogState = DialogData.DialogState.PENDING;
        _tiggerAutoTimer = 0;

        if (_didascaliePopup != null) _didascaliePopup.SetActive(true);
        if (AudioManager.instance && dialog._audioKeyDidaPopup != "") AudioManager.instance.PlaySound(dialog._audioKeyDidaPopup); 

        // play anim
        yield return new WaitForSeconds(1);

        if(__didascalieText) __didascalieText.text = dialog._text;

        // WAIT FOR TEXT END
        if(dialog._textDuration >0) yield return new WaitForSeconds(dialog._textDuration);

        _triggerOnceDico[_currentQuestID][_currentSubQuestID] = true;


        if (_didascaliePopup != null) _didascaliePopup.SetActive(false);

        if (dialog._increaseStoryAndAnimatorProgress)
        {
            IncreaseCurrentObj();
            int prevVal = _animatorIncreaseStory.GetInteger("state");
            _animatorIncreaseStory.SetInteger("state", prevVal + 1);
            //if (_animatorWhenDialogEnd && dialog._setAnimatorProgress) _animatorWhenDialogEnd.SetInteger("", 0);
        }
        else
        {
            if (_currentSubQuestID < _dialogDico[_currentQuestID].Count - 1) _currentSubQuestID++;
        }


        if (dialog._glitchID != -1)
        {
            yield return new WaitForSeconds(dialog._waitSecForFX);
            if(GlitchFX.instance != null) GlitchFX.instance.Glitch(dialog._glitchID);
        }

        _currentDialogState = DialogData.DialogState.WAIT_TRIGGER;
    }

    IEnumerator ButtonRegieCooldownRoutine()
    {
        DialogData dialog = _dialogDico[_currentQuestID][_currentSubQuestID];
        _currentDialogState = DialogData.DialogState.PENDING;
        _tiggerAutoTimer = 0;

        // audio : open channel sound
        if (AudioManager.instance && dialog._audioKeyOpenCloseChannel != "") AudioManager.instance.PlaySound(dialog._audioKeyOpenCloseChannel);

        // intro silent
        MainPlayerScript.instance._parentSilent.SetActive(true);
        animator.Play("pending");

        // WAIT FOR INTRO SILENT END
        yield return new WaitForSeconds(dialog._introSilentDuration);

        MainPlayerScript.instance._parentSilent.SetActive(false);

        if (dialog._stopPlayerInteractionWhileMessage) MainPlayerScript.instance.FreezeChar(true);

        // audio : voice sound
        if (AudioManager.instance && dialog._audioKeyDialogVoice != "") AudioManager.instance.PlaySound(dialog._audioKeyDialogVoice);

        // show text
        CharacterController2D cc2d = MainPlayerScript.instance.gameObject.GetComponent<CharacterController2D>();
        // cc2d._BGText.SetActive(true);
        //cc2d._BGText.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = dialog._text;
        //cc2d._philac.gameObject.SetActive(true);
        //obj.SetActive(true);
        _newBulleParent.SetActive(true);
        string txtfinal = dialog._text;
        while(txtfinal.Contains("*")) txtfinal = txtfinal.Replace('*','\n');
        _newBulleText.text = /*dialog.name + "\n" +*/ txtfinal;

        animator.Play("on");

        // WAIT FOR TEXT END
        yield return new WaitForSeconds(dialog._textDuration);

        // hide text
        //obj.SetActive(false);
        cc2d._BGText.SetActive(false);
        cc2d._philac.gameObject.SetActive(false);
        _newBulleParent.SetActive(false);

        MainPlayerScript.instance._parentSilent.SetActive(true);

        if (dialog._stopPlayerInteractionWhileMessage) MainPlayerScript.instance.FreezeChar(false);

        animator.Play("pending");

        // WAIT FOR OUTRO SILENCE
        yield return new WaitForSeconds(dialog._outroSilentDuration);

        MainPlayerScript.instance._parentSilent.SetActive(false);

        animator.Play("off");

        // audio : close channel sound
        if (AudioManager.instance && dialog._audioKeyOpenCloseChannel != "") AudioManager.instance.PlaySound(dialog._audioKeyOpenCloseChannel);
        _currentDialogState = DialogData.DialogState.OVER;

        yield return new WaitForSeconds(dialog._setInteractableDelay);

        _uiRegieButton.interactable = dialog._setInteractableWhenOver;
        _canCallOreillette = dialog._setInteractableWhenOver;


        //increase story progress
        if (dialog._increaseStoryAndAnimatorProgress)
        {
            IncreaseCurrentObj();
            int prevVal = _animatorIncreaseStory.GetInteger("state");
            _animatorIncreaseStory.SetInteger("state", prevVal + 1);
            //if (_animatorWhenDialogEnd && dialog._setAnimatorProgress) _animatorWhenDialogEnd.SetInteger("", 0);
        }
        else if (_currentSubQuestID < _dialogDico[_currentQuestID].Count - 1) _currentSubQuestID++;

        _currentDialogState = DialogData.DialogState.WAIT_TRIGGER;
        _tiggerAutoTimer = 0;



        if (dialog._glitchID != -1)
        {
            yield return new WaitForSeconds(dialog._waitSecForFX);
            if (GlitchFX.instance != null) GlitchFX.instance.Glitch(dialog._glitchID);
        }
    }


}
