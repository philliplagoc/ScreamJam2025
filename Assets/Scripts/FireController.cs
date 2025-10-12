using System;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [Tooltip("How many seconds the fire stays large.")] 
    [SerializeField] private float m_largeFireDuration = 10f;
    
    [Tooltip("How many seconds the fire stays medium.")] 
    [SerializeField] private float m_mediumFireDuration = 7f;


    [Tooltip("How many seconds the fire stays small before dying.")] 
    [SerializeField] private float m_smallFireDuration = 3f;
    
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
            var currentAnimationState = m_animator.GetCurrentAnimatorStateInfo(0);
            float decayRate = CalculateDecayRate(m_largeFireDuration);
            
            if (currentAnimationState.IsName("LargeFireBurningAnimation"))
            {
                decayRate = CalculateDecayRate(m_largeFireDuration);
            }
            else if (currentAnimationState.IsName("MediumFireBurningAnimation"))
            {
                decayRate = CalculateDecayRate(m_mediumFireDuration);
            }
            else if (currentAnimationState.IsName("SmallFireBurningAnimation"))
            {
                decayRate = CalculateDecayRate(m_smallFireDuration);
            }
            
            m_currentHealth -= decayRate * Time.deltaTime;

            // Send the current health value to the Animator every frame
            m_animator.SetFloat("FireHealth", m_currentHealth);
        }
    }

    private float CalculateDecayRate(float duration)
    {
        return (m_maxHealth / 2f) / duration;
    }

    public void KeepFireAlive()
    {
        m_currentHealth = m_maxHealth;
        m_animator.SetFloat("FireHealth", m_currentHealth);
        Debug.Log("Restored fire!");
    }
}
