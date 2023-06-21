using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float shotPower = 300;

    [SerializeField] float destroyTime = 1.5f;

    [Header("Components")]
    [SerializeField] Rigidbody rb;

    [SerializeField] Transform muzzle;


    //--------------------------------------------------

    public void Shot(Transform muzzle)
    {
		rb.AddForce(muzzle.transform.up * shotPower, ForceMode.Impulse);

        Destroy(gameObject, destroyTime);
	}
}
