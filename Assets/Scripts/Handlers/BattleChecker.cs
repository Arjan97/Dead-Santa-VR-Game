using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChecker : MonoBehaviour
{
    [SerializeField] private bool isInBattle = false;
    private float musicPlaybackPosition = 0f;
    private int enemyCountInArea = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Enemy_Flying"))
        {

            enemyCountInArea++;
            if (!isInBattle)
            {
                Debug.Log("Player sees enemies");
                UpdateBattleState();
                SoundManager.Instance.PlayMusic("battlemusic", 0.4f, musicPlaybackPosition);
                isInBattle = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Enemy_Flying"))
        {
            enemyCountInArea--;

            if (isInBattle)
            {
                Debug.Log("Player no longer sees enemies");
                UpdateBattleState();
            }
        }
    }

    void UpdateBattleState()
    {
        // Check if there are still enemies in the area
        if (enemyCountInArea == 0)
        {
            Debug.Log("No more enemies in the battle area");
            musicPlaybackPosition = SoundManager.Instance.GetMusicPlaybackPosition();
            SoundManager.Instance.PlayMusic("abnormal", 0.4f, musicPlaybackPosition);
            isInBattle = false;
        }
    }

    public void OnEnemyDestroyed()
    {
        Debug.Log("Enemy destroyed");
        enemyCountInArea--;
        UpdateBattleState();
    }
}
