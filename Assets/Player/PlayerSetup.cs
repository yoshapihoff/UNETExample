using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
	void Start ()
    {
        if (isLocalPlayer)
        {
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerRotation>().enabled = true;
            GetComponentInChildren<Camera>().enabled = true;
            GetComponentInChildren<AudioListener>().enabled = true;
        }
	}
}
