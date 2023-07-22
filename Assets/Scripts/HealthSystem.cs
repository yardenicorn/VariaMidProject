using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    private const int _maxHealth = 3;
    private int _currentHealth;
    private string _currentScene;
    [SerializeField] private GameObject _gameoverTextObject;
    [SerializeField] private GameObject _restartTextObject;

    public UnityEvent onDeath { get; private set; } = new UnityEvent();
    public Image[] hearts;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _currentScene = SceneManager.GetActiveScene().name;
    }

    public void TakeDamage()
    {
        _currentHealth--;

        UpdateHearts();

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _currentHealth)
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
        _gameoverTextObject.SetActive(true);
        _restartTextObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(_currentScene);
    }

}
