using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerScoreTxt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }


    GameManager gameManager;
    Text text;
    public int Playernum;

    // Update is called once per frame
    void Update()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager == null)
            {
                return;
            }
        }


        Player[] players = gameManager.GetAllPlayer();
        text.text = players[Playernum].score.ToString();

    }
}
