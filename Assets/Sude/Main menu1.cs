using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;





public class Mainmenu1 : MonoBehaviour
{

    public void startGame()
    {
        SceneManager.LoadScene("Oda");
    } 

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    } 

    public void optionsMenu()
    {
        SceneManager.LoadScene("Options Menu");
    }

    public void quitGame()
    {
        Application.Quit();
    }
    
}
