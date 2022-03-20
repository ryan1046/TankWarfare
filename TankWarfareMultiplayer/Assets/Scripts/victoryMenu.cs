using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class victoryMenu : MonoBehaviour
{

    public GameObject win;
    public GameObject lose;
    //Player[] players;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        changeVictory();



    }

    //[ClientRpc]
    void changeVictory()
    {
        Player[] players = GameObject.FindObjectsOfType<Player>();

        {

            foreach (Player player in players)
            {

                if (player.isLocalPlayer) ;
                {

                    if (player.score >= 3)
                    {
                        win.SetActive(true);
                        lose.SetActive(false);
                        return;
                    }
                    else
                    {
                        win.SetActive(false);
                        lose.SetActive(true);
                    }

                }
            }
        }
    }


    public void goToMainMenu()
    {
        // SceneManager.LoadScene("Scene_MainMenu");
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StopClient();
    }

}
