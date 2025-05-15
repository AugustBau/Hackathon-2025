using UnityEngine;

public class WaterTank : Obstacle
{
    protected override void Start()
    {
        boxCollider.size = new Vector2(0.3512521f, 0.4615769f);
        boxCollider.offset = new Vector2(0.0008747876f, 0.03847414f);
        Debug.Log("WaterTank collider set");
    }
}
