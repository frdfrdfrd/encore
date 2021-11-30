using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerScript : MonoBehaviour
{
    // Interaction with objects (either NPC or env-items)
    [HideInInspector] public InteractableScript _currentObjectInRangeToInteract;
    [HideInInspector] public List<InteractableScript> _allObjectsInRangeToInteract;

    [HideInInspector] public bool _freeze = false; // used with the FreezeChar function (do not change externally)

    //public Transform _objectiveCanvas;
    //public GameObject _parentDialog;
    public GameObject _parentSilent;


    // Quick singleton access
    private static MainPlayerScript mInstance;
    public static MainPlayerScript instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType(typeof(MainPlayerScript)) as MainPlayerScript;
            }

            return mInstance;
        }
    }

    private void Awake()
    {
        _currentObjectInRangeToInteract = null;
        _allObjectsInRangeToInteract = new List<InteractableScript>();
        _freeze = false;
    }

    // Stop the player avatar (called when some UI are opened)
    public void FreezeChar(bool freeze)
    {
        _freeze = freeze;

        this.GetComponent<CharacterController2D>().Freeze(freeze);
    }

    // Called by objects in range of player
    public void AddObjectInRange(InteractableScript obj)
    {
        if (_currentObjectInRangeToInteract == null)
        {
            _currentObjectInRangeToInteract = obj;
            ChangeUIInteract(_currentObjectInRangeToInteract, true);
        }

        if (!_allObjectsInRangeToInteract.Contains(obj)) _allObjectsInRangeToInteract.Add(obj);
    }

    // Called by objects no longer in range of player
    public void RemoveObjectInRange(InteractableScript obj)
    {
        if (_allObjectsInRangeToInteract.Contains(obj)) _allObjectsInRangeToInteract.Remove(obj);

        if (_currentObjectInRangeToInteract == obj)
        {
            ChangeUIInteract(_currentObjectInRangeToInteract, false);
            _currentObjectInRangeToInteract = null;
            foreach (InteractableScript other in _allObjectsInRangeToInteract)
            {
                _currentObjectInRangeToInteract = other;

                break;
            }
        }

        ChangeUIInteract(_currentObjectInRangeToInteract, true);
    }

    void ChangeUIInteract(InteractableScript obj, bool val)
    {
        if (obj != null)
        {
            if (obj._interactCanvas) obj._interactCanvas.SetActive(val);
            if (obj.GetComponentInChildren<Renderer>())
            {
                //Debug.Log("setting glow!");
                obj.GetComponentInChildren<Renderer>().material.SetInt("_IsPlayerClose", val ? 1 : 0);
            }
        }
    }

    public void Update()
    {
        // player interracts with the first item in range
        if (_currentObjectInRangeToInteract != null && !_freeze && Input.GetKeyDown(KeyCode.E) &&  GameManager.instance._currentDialogState == DialogData.DialogState.WAIT_TRIGGER)
        {
            _currentObjectInRangeToInteract.PlayerInteraction();
        }
    }


    //private void OnMouseDown()
    //{
    //    PlayerAskObjective();
    //}

    //public void PlayerAskObjective()
    //{
    //    GameManager.instance.ButtonRegieCooldown();
    //    //foreach (Transform t in _objectiveCanvas)
    //    //{
    //    //    if (t.name == GameManager.instance.GetCurrentObjKey())
    //    //    {
    //    //        int currentDialogStep = GameManager.instance.GetDialogueStep();

    //    //        if (currentDialogStep < t.transform.childCount)
    //    //        {
    //    //            StartCoroutine(ShowAndHide(t.GetChild(currentDialogStep).gameObject));
    //    //        }
    //    //        else
    //    //        {
    //    //            StartCoroutine(ShowAndHide(t.GetChild(t.transform.childCount-1).gameObject));
    //    //        }

    //    //        break;
    //    //    }
    //    //}
    //}

    //IEnumerator ShowAndHide(GameObject obj)
    //{
    //    yield return new WaitForSeconds(GameManager.instance._waitBefore );
    //    CharacterController2D cc2d = this.GetComponent<CharacterController2D>();

    //    cc2d._BGText.SetActive(true);
    //    cc2d._philac.gameObject.SetActive(true);

    //    obj.SetActive(true);
    //    yield return new WaitForSeconds(GameManager.instance._cooldownButtonSeconds - GameManager.instance._waitBefore);
    //    obj.SetActive(false);

    //    cc2d._BGText.SetActive(false);
    //    cc2d._philac.gameObject.SetActive(false);
    //}

    public void ShowPlayer(bool b)
    {
        this.GetComponent<CharacterController2D>().puppet.GetComponent<Renderer>().enabled = b;
    }

    public void HideAndKillControls()
    {
        this.GetComponent<CharacterController2D>().HideAndKillControls();

    }

}
