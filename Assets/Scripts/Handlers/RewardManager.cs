using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{
    public GameObject congratulationPanel;
    public float congratulationDuration = 10f;

    void Start()
    {
        congratulationPanel.SetActive(false);
    }

    public void ShowReward()
    {
        StartCoroutine(ShowCongratulationPanel());
    }

    IEnumerator ShowCongratulationPanel()
    {
        congratulationPanel.SetActive(true);
        SoundManager.Instance.PlaySound("win");
        yield return new WaitForSecondsRealtime(congratulationDuration);

        // Hide the congratulation panel
        congratulationPanel.SetActive(false);

        // Return to the "Menu" scene
        SceneManager.LoadScene("Menu");
    }
}
