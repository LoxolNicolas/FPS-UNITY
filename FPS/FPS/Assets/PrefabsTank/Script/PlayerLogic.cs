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
    public bool isDead = false;
    // Start is called before the first frame update
    public void OnStart()
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
            HealthText.text = 0 + "/" + maxHealth;
            Dead();
        }
    }

    void Dead()
    {
        DeadText.SetActive(true);
        GetComponent<TankController>().enabled = false;
        GetComponent<TankShoot>().enabled = false;
        isDead = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //TakeDamage(5);
        }
    }
}
