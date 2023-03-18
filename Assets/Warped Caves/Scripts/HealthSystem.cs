using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject gameoverTextObject;
    private Animator anim;

    public Image[] hearts;

    private void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth--;

        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("Hurt");
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
        gameoverTextObject.SetActive(true);
    }
}
