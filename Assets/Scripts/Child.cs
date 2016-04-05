using UnityEngine;
using System.Collections;

public class Child : NetworkObject
{
    public float height = 1.5f;

	override public void Start ()
    {
        if (SuperNetworkManager.isServer)
        {
            // Player: Child

            if (hasAuthority)
            {
                // -> Script attached to Child
                base.Start();
                GetComponentInChildren<AudioListener>().enabled = true;
                GetComponentInChildren<Camera>().enabled = true;
                transform.position = transform.position + Vector3.up * height;
            }
            
        }
        else
        {
            // Player: Monster
            if (hasAuthority)
            {
                // -> Script attached to monster
                GetComponentInChildren<Light>().enabled = false;
                GetComponentInChildren<AudioListener>().enabled = false;
                enabled = false;
            }
            else
            {
                // -> Script attached to Child
                // base.Start();
                // transform.position = transform.position + Vector3.up * height;
            }
        }
    }

    override public void Update ()
    {
	    
	}
}
