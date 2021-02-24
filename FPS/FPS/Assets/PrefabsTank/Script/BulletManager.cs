using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletManager : MonoBehaviour
{
    public GameObject Player;
    public int BulletDamage;
    [SerializeField]
    
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
        Debug.Log("cc");
        if((collision.gameObject.CompareTag("red") && Player.CompareTag("green")) || (collision.gameObject.CompareTag("green") && Player.CompareTag("red"))){
            collision.gameObject.GetComponent<PlayerLogic>().TakeDamage(BulletDamage);
            Debug.LogError("cc2");
        }
        //collision.gameObject.GetComponent<PlayerLogic>().TakeDamage(BulletDamage);
        Destroy(gameObject);
    }   
}
