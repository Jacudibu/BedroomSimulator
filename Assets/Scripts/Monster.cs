using UnityEngine;
using System.Collections;

public class Monster : NetworkObject
{

	public override void Start ()
    {
        if (!SuperNetworkManager.isServer)
        {
            // Player: Monster
            if (hasAuthority)
            {
                // Script is attached on Monster
                base.Start();
            }
            else
            {
                // Script is attached on Child
                enabled = false;
            }
        }
        else
        {
            // Player: Child
            if (hasAuthority)
            {
                // Script is attached on Child
                enabled = false;
            }
        }
    }
	
	public override void Update ()
    {
        if (!hasAuthority)
            return;

        transform.Translate((Vector3.right * Input.GetAxis("Horizontal") + Vector3.up * Input.GetAxis("Vertical")) * Time.deltaTime);
	}
}
