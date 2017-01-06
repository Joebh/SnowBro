using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_game : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void LoadGame()
    {
        Debug.Log("test");
        SceneManager.LoadScene("play_game");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
