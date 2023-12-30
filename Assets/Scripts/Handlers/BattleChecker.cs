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
                SoundManager.Instance.PlayMusic("scaryviolins", 0.5f, musicPlaybackPosition);
                isInBattle = true;
            }
        }

        if (other.CompareTag("EnemyBoss"))
        {
            enemyCountInArea++;
            UpdateBattleState();
            isInBattle = true;
            Debug.Log("Player sees boss");
            SoundManager.Instance.PlayMusic("orchestra", 0.5f, musicPlaybackPosition);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Enemy_Flying") || other.CompareTag("EnemyBoss"))
        {
            enemyCountInArea--;
            UpdateBattleState();
        }
    }

    void UpdateBattleState()
    {
        // Check if there are still enemies in the area
        if (enemyCountInArea <= 0)
        {
            Debug.Log("No more enemies in the battle area");
            musicPlaybackPosition = SoundManager.Instance.GetMusicPlaybackPosition();
            SoundManager.Instance.PlayMusic("spookywind", 0.5f, musicPlaybackPosition);
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
