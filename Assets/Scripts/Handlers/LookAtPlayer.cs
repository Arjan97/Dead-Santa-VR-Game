using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private bool isTextInverted = false;

    void Start()
    {
        // Assuming the player has a tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure to tag your player object with the tag 'Player'.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Make this object look at the player
            transform.LookAt(playerTransform);

            // Check if the text needs to be inverted
            if (!isTextInverted)
            {
                InvertText();
                isTextInverted = true;
            }
        }
    }

    // Invert the text horizontally
    private void InvertText()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
