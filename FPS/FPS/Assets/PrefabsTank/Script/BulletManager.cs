using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletManager : NetworkBehaviour
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
    [Client]
    public void OnTriggerEnter(Collider collision)
    {
        if((collision.gameObject.CompareTag("red") && Player.CompareTag("green")) || (collision.gameObject.CompareTag("green") && Player.CompareTag("red"))){
            PlayerShot(collision.gameObject);
        }
        Destroy(gameObject);
    }   

    [Command]
    private void PlayerShot(GameObject player)
    {
        Debug.Log("touché");
        gameObject.GetComponent<PlayerLogic>().TakeDamage(BulletDamage);
    }
}
