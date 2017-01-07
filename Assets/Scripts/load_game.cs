using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Comp : System.Collections.IComparer
{
    public int Compare(object x, object y)
    {
        float yf = (float)y;
        float xf = (float)x;
        return yf.CompareTo(xf);
    }
}

public class load_game : MonoBehaviour {

    public GameObject scoreDisplay;

    private HighScores highScores = null;

	// Use this for initialization
	void Start () {
	}   

    void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/highscores.dat");
        bf.Serialize(file, highScores);
        file.Close();
    }

    void Load()
    {
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/highscores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/highscores.dat", FileMode.Open);
            highScores = (HighScores)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            highScores = new HighScores();
            highScores.scores = new ArrayList();
        }
    }
    
    void LevelLoaded(Scene scene, LoadSceneMode mode)
    {
        // if this is first load highscores will be null, load from file if exists
        if (highScores == null)
        {
            Load();
        }

        // update with score if there is a score
        if (snowman.score != -1f)
        {
            highScores.scores.Add(snowman.score);

            highScores.scores.Sort(new Comp());

            if (highScores.scores.Count > 10)
            {
                highScores.scores.RemoveRange(10, highScores.scores.Count - 10);
            }
            
        }

        // save the file with the updated scores
        Save();

        // update gui
        Text text = scoreDisplay.GetComponent<Text>();

        text.text = "";

        foreach(float score in highScores.scores)
        {
            text.text += score + "\n";
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += LevelLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelLoaded;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("play_game");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

[Serializable]
class HighScores
{
    public ArrayList scores;
}