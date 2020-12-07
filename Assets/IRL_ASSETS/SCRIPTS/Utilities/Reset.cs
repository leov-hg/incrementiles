using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
    //    {
    //        RESET_PLAYER_PREFS();
    //    }
    //}

    public void RESET_PLAYER_PREFS()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RELOAD()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
