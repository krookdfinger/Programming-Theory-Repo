using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMainScene : MonoBehaviour
{

    private TMP_Text farmTitle;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameManager.Instance.PlayerName);
    }

    private void Awake()
    {
        farmTitle = GameObject.Find("Canvas").GetComponentInChildren<TMP_Text>();

        farmTitle.text = "Old " + GameManager.Instance.PlayerName + "'s Farm";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
