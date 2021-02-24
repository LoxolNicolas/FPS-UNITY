using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootBullet : NetworkBehaviour
{
    public GameObject Player;
    [SerializeField]
    private GameObject spawn;
    [SerializeField]
    private GameObject sens;
    [SerializeField]
    private GameObject bulletPrefab;

    void Start()
    {
        if(spawn == null)
        {
            this.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBullet();
        }
    }

    [Client]
    private void SpawnBullet()
    {
        if (isLocalPlayer)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawn.transform.position, sens.transform.rotation);
            bullet.GetComponent<BulletManager>().Player = Player;
        }
    }
}
