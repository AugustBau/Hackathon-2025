using UnityEngine;

public class AutoRunPlayer : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Move the player to the right automatically
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}