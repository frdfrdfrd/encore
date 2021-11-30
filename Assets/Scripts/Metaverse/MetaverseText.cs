using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaverseText : MonoBehaviour
{
    TMPro.TextMeshProUGUI _textComp;
    public string _textPart1;
    public string _middleSpecialText;
    public string _textPart2;

    public float _durationPunctuation = 0.25f;
    public float _durationBetweenLetter = 0.01f;
    public float _percentChanceWaitForCharac = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        _textComp = this.GetComponent<TMPro.TextMeshProUGUI>();
        _textPart1 = "dfnedfnf do, df dfidf dozkrezokrko !!! dfidfijfdj.";
        Regen(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTextTo(string newText)
    {
        _middleSpecialText = newText;
        Regen();
    }

    public void Regen(bool progressive = false)
    {
        string fullText = (_textPart1 + '\n' + _middleSpecialText + '\n' + _textPart2).Replace('*', '\n');

        if (progressive) StartCoroutine(TypeSentence(fullText));
        else _textComp.text = fullText;
    }

    IEnumerator TypeSentence( string fullSentence)
    {

        _textComp.text = "";
        foreach (char letter in fullSentence)
        {
            _textComp.text += letter;

           // if (!inMarkup)
            {
                bool isdefaultchar = false;
                float waitDuration;
                switch (letter)
                {
                    case ',':
                    case '.':
                    case '?':
                    case '!':
                        waitDuration = _durationPunctuation;
                        break;
                    default:
                        isdefaultchar = true;
                        waitDuration = _durationBetweenLetter;
                        break;
                }

                if (isdefaultchar && Random.value > _percentChanceWaitForCharac) { }
                else yield return new WaitForSeconds(waitDuration);
            }
        }
    }
}
