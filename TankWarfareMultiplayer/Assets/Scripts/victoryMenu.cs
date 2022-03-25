using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class victoryMenu : NetworkBehaviour
{

    public GameObject win;
    public GameObject lose;


    Text text;

    //Player[] players;

    // Start is called before the first frame update
    void Start()
    {

        text = win.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        changeVictory();



    }

    //[ClientRpc]
    void changeVictory()
    {
        PlayerLobby[] players = GameObject.FindObjectsOfType<PlayerLobby>();

        

            foreach (PlayerLobby player in players)
            {

                 if (player.hasAuthority) 
                  {

                      if (player.score >= 3)
                      {
                          // win.SetActive(true);
                          // lose.SetActive(false);
                          
                          text.text = "You won";
                          return;
                      }
                      else
                      {
                          //win.SetActive(false);
                          //lose.SetActive(true);
                          text.text = "You lost";
                      }

                  }
                
                /*
                if (isServer)
                {
                   
                        if (player.score >= 3)
                        {
                            text.text = "You won";
                            return;
                        }
                        else
                        {
                            text.text = "You lost";
                        }
                    


                }
                */
               
            }
        
        
    }


    public void goToMainMenu()
    {
        // SceneManager.LoadScene("Scene_MainMenu");
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StopClient();
    }

}
