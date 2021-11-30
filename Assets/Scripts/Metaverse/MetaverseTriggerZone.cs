using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaverseTriggerZone : MonoBehaviour
{
    public Metaverse.Pos _posToChange;

    string prevText;
    public string replaceTextMid = "IMGONNA DIE";
    public bool resetPrevOnExit = true;

    private void OnTriggerEnter(Collider collision)
    {
        if (Metaverse.instance)
        {
            prevText = Metaverse.instance.GetPrevText(_posToChange);
            Metaverse.instance.ChangeSpecialText(_posToChange, replaceTextMid);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (Metaverse.instance)
        {
            if(resetPrevOnExit) Metaverse.instance.ChangeSpecialText(_posToChange, prevText);
        }
    }
}
