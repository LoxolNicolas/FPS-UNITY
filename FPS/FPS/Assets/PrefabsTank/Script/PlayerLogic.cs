using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    private int maxHealth = 200;
    private int currentHealth;
    public HealthBar healthBar;
    public GameObject DeadText;
    public Text HealthText;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        HealthText.text = currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        HealthText.text = currentHealth + "/" + maxHealth;
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        DeadText.SetActive(true);
        GetComponent<movement>().enabled = false;
        GetComponent<ShootBullet>().enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //TakeDamage(5);
        }
    }
}
