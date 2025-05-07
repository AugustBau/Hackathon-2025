using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected BoxCollider2D boxCollider;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Start()
    {
    }
}
