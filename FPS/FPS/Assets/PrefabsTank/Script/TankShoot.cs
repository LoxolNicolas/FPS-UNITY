using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TankShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject spawnBullet;

    public int damage;
    public float range;

    [SerializeField]
    private LayerMask mask;

    private GameObject SmokeDestroy;
    private GameObject FireA;
    private GameObject FireB;

    void Start()
    {
        if (cam == null)
            this.enabled = false;

        SmokeDestroy = GameObject.Find("Fumee");
        FireA = GameObject.Find("FlameA");
        FireB = GameObject.Find("FlameB");

        SmokeDestroy.SetActive(false);
        FireA.SetActive(false);
        FireB.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && isLocalPlayer)
        {
            Instantiate(bulletPrefab, spawnBullet.transform.position, cam.transform.rotation);
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, mask))
        {
            Debug.Log("Touché");
            if ((hit.collider.gameObject.CompareTag("red") && gameObject.CompareTag("green")) || (hit.collider.gameObject.CompareTag("green") && gameObject.CompareTag("red"))){
                PlayerShot(hit.collider.gameObject);
            }
        }
    }

    [Command]
    private void PlayerShot(GameObject player)
    {
        Debug.LogError(gameObject.name);
        PlayerLogic p = player.GetComponent<PlayerLogic>();
        if (p && !p.isDead)
        {
            p.TakeDamage(damage);
        }
    }
}
