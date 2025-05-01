using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public GameObject restartButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartButton.SetActive(false);
    }

    private void RestartLevel()
    {
        GameManager.Instance.LoadScene("SampleScene");
    }
}
