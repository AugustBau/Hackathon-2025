using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderManager : MonoBehaviour
{
    GameManager gameManager;

    void Start ()
    {
        try
        {
            gameManager = GameManager.Instance;
        }
        catch (System.Exception e)
        {
            Debug.LogError("GameManager not found: " + e.Message);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Restart"))
        {
            StartCoroutine(PlaySound());
            gameManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
   IEnumerator PlaySound ()
    {
        SoundManager.Instance.PlayPlayerSound("GlassShatter");
        yield return null;
    }
    
}
