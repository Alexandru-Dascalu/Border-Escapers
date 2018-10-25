using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    public void RestartGame()
    {
        //loads current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	
}
