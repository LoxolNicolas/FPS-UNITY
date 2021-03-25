using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBullet : MonoBehaviour
{    
    void Start()
    {
        GetComponentInChildren<Rigidbody>().AddForce(transform.forward * 3000);
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
