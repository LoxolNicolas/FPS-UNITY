using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
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
        spawn = Player.transform.Find("Turret/Gun/SpawnBullet").gameObject;
        sens = Player.transform.Find("Turret/Gun").gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, spawn.transform.position, sens.transform.rotation);

            bullet.GetComponent<BulletManager>().Player = Player;          
        }
    }
}
