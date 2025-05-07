using UnityEngine;

public class Ventilator : Obstacle
{
    protected override void Start()
    {
        boxCollider.size = new Vector2(0.4459451f, 0.3887237f);
        boxCollider.offset = new Vector2(-0.009951413f, 0.04353791f);
        Debug.Log("Ventilator collider set");
    }
}

