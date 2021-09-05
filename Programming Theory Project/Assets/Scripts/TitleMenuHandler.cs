using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenuHandler : MonoBehaviour
{
    public void OnEndEdit(string text)
    {
        Debug.Log(text);
        GameManager.Instance.PlayerName = text;
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

}
