using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    //remember to add player sounds once made
    public Sound[] enemyJump, enemyStart, enemyRestart, enemyShaken, enemyCatch, enemyWin, enemyFall, playerSFX;
    public AudioSource musicSource, playerSource, enemySource;

    private string lastSoundEnemy = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEnemySound(string arrayName)
    {
        Sound[] selectedArray = null;

        // Match the array name to the corresponding sound array
        switch (arrayName.ToLower())
        {
            case "jumping":
                selectedArray = enemyJump;
                break;
            case "dying":
                selectedArray = enemyFall;
                break;
            case "starting":
                selectedArray = enemyStart;
                break;
            case "restarting":
                selectedArray = enemyRestart;
                break;
            case "shaken":
                selectedArray = enemyShaken;
                break;
            case "catching":
                selectedArray = enemyCatch;
                break;
            case "winning": 
                selectedArray = enemyWin;
                break;
            default:
                Debug.LogError($"Sound array '{arrayName}' not found.");
                return;
        }

        if (selectedArray == null || selectedArray.Length == 0)
        {
            Debug.LogError($"Sound array '{arrayName}' is empty or null.");
            return;
        }

        Sound selectedSoundEnemy;
        do
        {
            selectedSoundEnemy = selectedArray[UnityEngine.Random.Range(0, selectedArray.Length)];
        } while (selectedSoundEnemy.name == lastSoundEnemy && selectedArray.Length > 1);

        lastSoundEnemy = selectedSoundEnemy.name;
        enemySource.PlayOneShot(selectedSoundEnemy.clip);
    }

    public void PlayPlayerSound(string name)
    {
        Sound sound = Array.Find(playerSFX, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("SFX sound not found");
        }

        else
        {
            playerSource.PlayOneShot(sound.clip);
        }
    }
}

