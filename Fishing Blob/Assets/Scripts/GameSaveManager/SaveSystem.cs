using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public int HighScore { get; set; }
    public int HighScoreDays { get; set; } = 1;
    public int MoneyGainedOverallHighScore { get; set; }
    private static SaveSystem Instance { get; set; }
    private string SavePath => Path.Combine(Application.persistentDataPath + "save.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Load();
        }
        
    }

    public void Save(int highscore, int highScoreDays, int moneyGainedOverallHighscore)
    {
        HighScoreObject highScoreObject = new HighScoreObject()
        {
            Highscore = highscore,
            HighScoreDays = highScoreDays,
            MoneyGainedOverallHighscore = moneyGainedOverallHighscore
        };
        File.WriteAllText(SavePath, JsonUtility.ToJson(highScoreObject));
        Debug.Log("Saved!");
    }

    public void Load()
    {
        if (File.Exists(SavePath))
        {
            HighScoreObject loadedHighScoreObject = JsonUtility.FromJson<HighScoreObject>(File.ReadAllText(SavePath));
            HighScore = loadedHighScoreObject.Highscore;
            HighScoreDays = loadedHighScoreObject.HighScoreDays;
            MoneyGainedOverallHighScore = loadedHighScoreObject.MoneyGainedOverallHighscore;
        }
    }

    private class HighScoreObject
    {
        public int Highscore;
        public int HighScoreDays;
        public int MoneyGainedOverallHighscore;
    }
}