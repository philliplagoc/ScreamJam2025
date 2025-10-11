using System;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [Tooltip("How many seconds the fire stays large.")] 
    [SerializeField] private float m_largeFireDuration = 10f;

    [Tooltip("How many seconds the fire stays small before dying.")] 
    [SerializeField] private float m_smallFireDuration = 5f;
    
    private Animator m_animator;
    [SerializeField] private float m_currentHealth;
    private float m_maxHealth = 100f;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_currentHealth = m_maxHealth;
        m_animator.SetFloat("FireHealth", m_currentHealth);
    }

    private void Update()
    {
        // Only decrease health if it's above zero
        if (m_currentHealth > 0)
        {
            // Determine how fast health should decay based on the current state
            // This makes the durations you set in the inspector work correctly.
            bool isLarge = m_animator.GetCurrentAnimatorStateInfo(0).IsName("LargeFireBurning");
            float decayRate = isLarge ? (m_maxHealth / 2f) / m_largeFireDuration : (m_maxHealth / 2f) / m_smallFireDuration;
            
            m_currentHealth -= decayRate * Time.deltaTime;

            // Send the current health value to the Animator every frame
            m_animator.SetFloat("FireHealth", m_currentHealth);
        }
    }

    public void KeepFireAlive()
    {
        m_currentHealth = m_maxHealth;
        m_animator.SetFloat("FireHealth", m_currentHealth);
        Debug.Log("Restored fire!");
    }
}
