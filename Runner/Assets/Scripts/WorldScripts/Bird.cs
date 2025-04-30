using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Disable when off-screen (adjust -12 to fit your level width)
        if (transform.position.x < -12f)
        {
            gameObject.SetActive(false);
        }
    }

    public void Fly(Vector2 startPos)
    {
        transform.position = startPos;
        gameObject.SetActive(true);
    }
}
