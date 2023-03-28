using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    private const int maxHealth = 3;
    private int currentHealth;
    private string currentSceneName;
    [SerializeField] private GameObject gameoverTextObject;
    [SerializeField] private GameObject restartTextObject;

    public UnityEvent onDeath { get; private set; } = new UnityEvent();

    public Image[] hearts;

    private void Start()
    {
        currentHealth = maxHealth;
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void TakeDamage()
    {
        currentHealth--;

        UpdateHearts();

        if (currentHealth <= 0)
        {
            Death();
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

    private void Death()
    {
        onDeath.Invoke();
        gameoverTextObject.SetActive(true);
        restartTextObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(currentSceneName);
    }

}
