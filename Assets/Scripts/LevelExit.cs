using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D player)
    {
        if (this.name == "Quit Game Portal")
            StartCoroutine(QuitGame());
        else
        StartCoroutine(LoadNextLevel());

        player.GetComponent<Player>().exitPortalCoordinates = this.transform.position;
        player.transform.localScale = new Vector3(1, 1, 0);
        player.GetComponent<Player>().enetringPortalToNextLevel = true;
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitUntil(() => !FindObjectOfType<Player>());

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
            FindObjectOfType<GameSession>().GetComponent<GameSession>().gameIsFinnished = true;
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
            FindObjectOfType<GameSession>().GetComponent<GameSession>().gameIsFinnished = false;
        }
    }

    IEnumerator QuitGame()
    {
        yield return new WaitUntil(() => !FindObjectOfType<Player>());

        Application.Quit();
    }
}
