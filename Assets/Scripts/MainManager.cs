using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public static MainManager Instance;

    private void Awake()
    {
        UIManager.Instance.LoadPoint();
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGame();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void GenerateGame()
    {
        m_Started = false;
        m_GameOver = false;
        ScoreText.text = UIManager.Instance.playerName + ": " + m_Points;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        HighScoreText.text = "Best Score : " + UIManager.Instance.nameRecord + " : " + UIManager.Instance.highScorePoints;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        if(UIManager.Instance.playerName == null)
        {
            ScoreText.text = $"Score : {m_Points}";
        }
        else
        {
            ScoreText.text = UIManager.Instance.playerName + ": " + m_Points;
        }
    }

    public void GameOver()
    {
        if(UIManager.Instance.highScorePoints < m_Points)
        {
            NewHighScore();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void NewHighScore()
    {
        UIManager.Instance.highScorePoints = m_Points;
        UIManager.Instance.nameRecord = UIManager.Instance.playerName;
        HighScoreText.text = "Best Score : " + UIManager.Instance.nameRecord + " : " + UIManager.Instance.highScorePoints;
        UIManager.Instance.SavePoint();
    }
}
