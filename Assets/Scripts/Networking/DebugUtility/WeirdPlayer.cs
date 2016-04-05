using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeirdPlayer : WeirdNetworkPlayer
{
	override public void Update ()
    {
        base.Update();

        if (hasAuthority)
        {
            // Super cheap movement stuff for testing reasons
            Vector3 movement = Vector3.zero;
            movement += transform.forward * Input.GetAxis("Vertical") * 5f;
            movement += transform.right * Input.GetAxis("Horizontal") * 2.5f;
            rb.velocity = movement;

            Vector3 rotation = Vector3.zero;
            rotation.y = Input.GetAxis("Mouse X");
            transform.Rotate(rotation);

            rb.velocity += Vector3.up * Input.GetAxis("Jump") * 10f;


            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CmdSpawnProjectile(transform.position + transform.forward, transform.rotation);
            }
        }
    }
}
