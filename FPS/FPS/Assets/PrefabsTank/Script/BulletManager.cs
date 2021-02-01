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
        if((collision.gameObject.CompareTag("Team1") && Player.CompareTag("Team2")) || (collision.gameObject.CompareTag("Team2") && Player.CompareTag("Team1"))){
            collision.gameObject.GetComponent<PlayerLogic>().TakeDamage(BulletDamage);
            Debug.Log("erqover");
        }
        Destroy(gameObject);
    }
    
}
