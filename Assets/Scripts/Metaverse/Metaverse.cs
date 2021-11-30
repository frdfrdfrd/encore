using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Metaverse : MonoBehaviour
{
    public MetaverseText _left;
    public MetaverseText _middle;
    public MetaverseText _right;

    [HideInInspector] int _stepMeta = 0;
    [HideInInspector] int _stepMetaENDGAME = 0;

    public string _questionMetaAfterDisableAvatar = "What know?";

    public string _questionEndGame = "Hello again.\n We're almost done here.\n Before we step any further, you must be aware that this is a one way journey and you will not be able to play me anymore.\n Do you understand? \nPRESS [Y]";
    public string _questionEndGameConfirm = "Hey, there's no need for sadness, didn't we have a great time? \nRemember this good time, and that I wanted this. \nI'm ready. \nWhen you are, please, press [Y]";
    public string _finalGameText = "Thank you, my friend.";

    public GameObject _UIToFade;

    public float _delayBeforeFade = 4;

    public enum Pos
    {
        LEFT,
        MID,
        RIGHT
    }

    private static Metaverse mInstance;
    public static Metaverse instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType(typeof(Metaverse)) as Metaverse;
            }

            return mInstance;
        }
    }


    private void Start()
    {
        _stepMeta = 0;
    }

    public void ChangeStep(int step)
    {
        if (step == 1)
        {
            _stepMeta = 1;
            _left._textPart1 = _questionMetaAfterDisableAvatar;
            _left._middleSpecialText = "";
            _left._textPart2 = "";
            _left.Regen(true);
        }
        else if (step == 2)
        {
            _stepMeta = 2;
            _left._textPart1 = _questionEndGame;
            _left._middleSpecialText = "";
            _left._textPart2 = "";
            _left.Regen(true);
        }
    }

    public void Update()
    {
        if (_stepMeta == 1 && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.A)) { _left._middleSpecialText += "A"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.Z)) { _left._middleSpecialText += "Z"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.E)) { _left._middleSpecialText += "E"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.R)) { _left._middleSpecialText += "R"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.T)) { _left._middleSpecialText += "T"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.Y)) { _left._middleSpecialText += "Y"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.U)) { _left._middleSpecialText += "U"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.I)) { _left._middleSpecialText += "I"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.O)) { _left._middleSpecialText += "O"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.P)) { _left._middleSpecialText += "P"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.Q)) { _left._middleSpecialText += "Q"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.S)) { _left._middleSpecialText += "S"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.D)) { _left._middleSpecialText += "D"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.F)) { _left._middleSpecialText += "F"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.G)) { _left._middleSpecialText += "G"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.H)) { _left._middleSpecialText += "H"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.J)) { _left._middleSpecialText += "J"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.K)) { _left._middleSpecialText += "K"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.L)) { _left._middleSpecialText += "L"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.M)) { _left._middleSpecialText += "M"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.W)) { _left._middleSpecialText += "W"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.X)) { _left._middleSpecialText += "X"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.C)) { _left._middleSpecialText += "C"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.V)) { _left._middleSpecialText += "V"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.B)) { _left._middleSpecialText += "B"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.N)) { _left._middleSpecialText += "N"; _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { _left._middleSpecialText += KeyCode.LeftArrow.ToString(); _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { _left._middleSpecialText += KeyCode.RightArrow.ToString(); _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { _left._middleSpecialText += KeyCode.UpArrow.ToString(); _left.Regen(); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { _left._middleSpecialText += KeyCode.DownArrow.ToString(); _left.Regen(); }
        }
        else if (_stepMeta == 2)
        {
            if (_stepMetaENDGAME == 0)
            {
                // the user presses Y -> ask for confirmation in step 2
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    _left._textPart1 = _questionEndGameConfirm;
                    _left._middleSpecialText = "";
                    _left._textPart2 = "";
                    _left.Regen(true);

                    _stepMetaENDGAME = 1;
                }
            }
            else if (_stepMetaENDGAME == 1)
            {

                if (Input.GetKeyDown(KeyCode.Y))
                {
                    StartCoroutine(EndCinematic(true));
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    _left._textPart1 = _questionEndGame;
                    _left._middleSpecialText = "";
                    _left._textPart2 = "";
                    _left.Regen(true);

                    _stepMetaENDGAME = 0;
                }
            }
        }
    }

    IEnumerator EndCinematic(bool alsoDisableGame)
    {
        _left._textPart1 = _finalGameText;
        _left._middleSpecialText = "";
        _left._textPart2 = "";
        _left.Regen(true);

        yield return new WaitForSeconds(_delayBeforeFade);
        _UIToFade.SetActive(true);
        yield return new WaitForSeconds(2);

        if (alsoDisableGame) PlayerPrefs.SetInt("jam24_disable", 1);

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

    }

    public string GetPrevText(Pos pos)
    {
        if (pos == Pos.LEFT) return _left._middleSpecialText;
        else if (pos == Pos.MID) return _middle._middleSpecialText;
        else return _right._middleSpecialText;
    }

    public void ChangeSpecialText(Pos pos, string txt)
    {
        if (pos == Pos.LEFT) _left.ChangeTextTo(txt);
        else if (pos == Pos.MID) _middle.ChangeTextTo(txt);
        else _right.ChangeTextTo(txt);
    }



}
