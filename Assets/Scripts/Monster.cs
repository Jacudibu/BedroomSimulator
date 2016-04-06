using UnityEngine;
using System.Collections;

public class Monster : NetworkObject
{

	public override void Start ()
    {
        base.Start();
    }
	
	public override void Update ()
    {
        base.Update();

        if (!hasAuthority)
            return;

        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime);
	}
}
