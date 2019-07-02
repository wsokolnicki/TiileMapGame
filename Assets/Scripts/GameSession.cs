using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    //config
    [SerializeField] int playersLives = 3;

    //state
    public bool gameIsFinnished;

    //cache

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (!gameIsFinnished)
            return;

        GameObject.Find("Win Text").GetComponent<Text>().enabled = true;
        GameObject.Find("Start Game Text").GetComponent<Text>().text = "Play Again";
    }

    public void ProcessPlayersDeath()
    {
        if (playersLives > 1)
            StartCoroutine(TakeLife());
        else
            StartCoroutine(ResetGameSession());
    }

    IEnumerator ResetGameSession()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    IEnumerator TakeLife()
    {
        playersLives--;
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<Player>().GetComponent<Player>().PlayerSpawn();
    }
}
