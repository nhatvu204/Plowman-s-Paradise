using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    private void OnTriggerEnter(Collider other)
    {
        //Check if the collider belongs to the player
        if (other.tag == "Player")
        {
            //Switch scene
            SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
        }
    }
}
