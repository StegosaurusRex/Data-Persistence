using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text Record;
    public InputField nameInput;
    public string playerName;

    public string nameRecord;
    public int highScorePoints;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadPoint();

        Record.text = "Best Score : " + nameRecord + " : " + highScorePoints;
    }

    public void newStart()
    {
        playerName = nameInput.text;

        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
    #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
    #else
                Application.Quit(); // original code to quit Unity player
    #endif
    }

    [System.Serializable]
    class SaveData
    {
        public int highScorePoint;
        public string nameRecordPlayer;
    }

    public void SavePoint()
    {
        SaveData data = new SaveData();
        data.highScorePoint = highScorePoints;
        data.nameRecordPlayer = nameRecord;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadPoint()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScorePoints = data.highScorePoint;
            nameRecord = data.nameRecordPlayer;
        }
    }

    public void ResetPoint()
    {
        SaveData data = new SaveData();
        data.highScorePoint = 0;
        data.nameRecordPlayer = "System";

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

}
