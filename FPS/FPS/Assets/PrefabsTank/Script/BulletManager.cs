using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletManager : MonoBehaviour
{/*
    public GameObject Player;
    public int BulletDamage;
    [SerializeField]
   */ 
    // Start is called before the first frame update
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
    
    public void OnTriggerEnter(Collider collision)
    {
        Destroy(gameObject);
    }   
    /*
    [Command]
    private void PlayerShot(GameObject player)
    {
        gameObject.GetComponent<PlayerLogic>().TakeDamage(BulletDamage);
    }
    */
}
