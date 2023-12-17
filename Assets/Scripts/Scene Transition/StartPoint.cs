using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StartPoint 
{
    //Location entering from
    public SceneTransitionManager.Location enteringFrom;

    //The transform the player start
    public Transform playerStart;
}
