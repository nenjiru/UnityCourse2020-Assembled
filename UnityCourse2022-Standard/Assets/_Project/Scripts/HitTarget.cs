using UnityEngine;
using UnityEngine.Events;

public class HitTarget : MonoBehaviour
{
    public Animator animator;
    public int score = 1;
    public UnityAction<int> Hit;

    void OnCollisionEnter(Collision other)
    {
        animator.SetTrigger("Shock");
        Hit?.Invoke(score);
    }
}
