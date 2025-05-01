using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlayEnemySound("winning");
            GameManager.Instance.LoadScene("WinScene");
        }
    }
}
