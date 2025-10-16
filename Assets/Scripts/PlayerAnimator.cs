using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void PlayMoveAnimation(Vector2 direction)
    {
        if (m_animator != null)
        {
            // Set the direction so the Idle and Walking blend trees know which animation to use
            m_animator.SetFloat("Horizontal", direction.x);
            m_animator.SetFloat("Vertical", direction.y);

            // Trigger the transition from Idle to Walking
            m_animator.SetTrigger("Move");
        }
    }
}