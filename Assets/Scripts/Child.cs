using UnityEngine;
using System.Collections;

public class Child : NetworkObject
{
    public float height = 1.5f;

	override public void Start ()
    {
        base.Start(); 

        if (!hasAuthority)
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
    }

    override public void Update ()
    {
        base.Update();
	}
}
