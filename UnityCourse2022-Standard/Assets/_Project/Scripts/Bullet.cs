using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float time = 5f;

    void Start()
    {
        Destroy(gameObject, time);
    }
}
