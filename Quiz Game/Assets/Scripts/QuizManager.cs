using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Photon.Realtime;
using TMPro;

public class QuizManager : MonoBehaviourPunCallbacks
{
    private int correctAnswerIndex;
    private List<int> currentOptions;

    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI playerListText;

    private int currentQuestionIndex = 0;

    private float timer = 0f;
    private bool isTimerRunning = false;
    private float questionInterval = 10f;
    private int lastSentSecond = 0;

    [SerializeField] GameObject teacherPanel;
    [SerializeField] GameObject studentPanel;
    [SerializeField] GameObject blockOptions;
    [SerializeField] Text timerText;
    [SerializeField] Text scoreText;
    [SerializeField] TextMeshProUGUI roomCodeText;
    [SerializeField] Sprite redBtnSprite;
    [SerializeField] Sprite greenBtnSprite;
    [SerializeField] Sprite blueBtnSprite;
    public static int numberOfQuestions;
    private int count;

    public GameObject rowPrefab;
    public Transform contentParent;
    private Dictionary<string, LeaderboardEntry> leaderboardEntries = new Dictionary<string, LeaderboardEntry>();

    //Dictionary<string, int> scores = new Dictionary<string, int>();

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        count = 0;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => SendAnswer(index));
        }
        if (PhotonNetwork.IsMasterClient)
        {
            teacherPanel.SetActive(true);
            studentPanel.SetActive(false);
            roomCodeText.text = "Room Code: " + MainMenuManager.roomCode;
        }
        else
        {
            teacherPanel.SetActive(false);
            studentPanel.SetActive(true);
        }
    }
    List<object> Question_Generator()
    {
        int num1 = Random.Range(10, 100);
        int num2 = Random.Range(10, 100);
        char[] oper = { '+', '-', '*', '/' };
        int index = Random.Range(0, oper.Length);
        char ope = oper[index];
        string que = "";
        int ans = 0;

        if (ope == '/')
        {
            int mul = num1 * num2;
            que = mul.ToString() + " " + ope + " " + num2.ToString();
            num1 = mul;
        }
        else
        {
            que = num1.ToString() + " " + ope + " " + num2.ToString();
        }

        switch (ope)
        {
            case '+': ans = num1 + num2; break;
            case '-': ans = num1 - num2; break;
            case '*': ans = num1 * num2; break;
            case '/': ans = num1 / num2; break;
        }

        return new List<object> { que, ans };
    }

    (List<int>, int) AnswerOptions(int ans)
    {
        List<int> opt = new List<int>();
        List<int> optionsRange = new List<int>(){-40, -30, -20, -10, 10, 20, 30, 40};
        while (opt.Count < 3)
        {
            int randomIndex = Random.Range(0, optionsRange.Count);
            int randomOption = optionsRange[randomIndex];
            optionsRange.Remove(randomOption);
            opt.Add(ans+randomOption);
        }

        int correctIndex = Random.Range(0, 4);
        opt.Insert(correctIndex, ans);

        return (opt, correctIndex);
    }

    

    public void SendQuestion()
    {
        List<object> question = Question_Generator();
        (currentOptions, correctAnswerIndex) = AnswerOptions((int)question[1]);
        photonView.RPC("ReceiveQuestion", RpcTarget.Others, question[0], currentOptions.ToArray(), correctAnswerIndex);

        timer = 0f;
        lastSentSecond = 0;
        isTimerRunning = true;
        count++;
    }

    [PunRPC]
    public void ReceiveAnswer(string studentName, int optionPressed)
    {
        if (ValidateAnswer(optionPressed))
        {
            AddOrUpdateEntry(studentName, 10*(int)Mathf.Ceil(questionInterval - timer));
        }
        else
        {
            AddOrUpdateEntry(studentName, -2*(int)Mathf.Ceil(questionInterval - timer));
        }
        Player currentPlayer = null;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName.Equals(studentName))
            {
                currentPlayer = player;
                break;
            }
        }
        photonView.RPC("ReceiveScore", currentPlayer, leaderboardEntries[studentName].GetScore());
    }
    [PunRPC]
    public void ReceiveScore(int score)
    {
        scoreText.text = score.ToString();
    }

    bool ValidateAnswer(int optionPressed)
    {
        return optionPressed == correctAnswerIndex;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isTimerRunning)
            {
                timer += Time.deltaTime;
                int currentSecond = Mathf.FloorToInt(timer);
                if (currentSecond > lastSentSecond)
                {
                    lastSentSecond = currentSecond;
                    photonView.RPC("ReceiveTimerSecond", RpcTarget.Others, currentSecond);
                }
                if (timer >= questionInterval && count < numberOfQuestions)
                {
                    isTimerRunning = false;
                    SendQuestion();
                }
                if (count == numberOfQuestions)
                {
                    // Declare Winner
                }
            }
        }
    }

    [PunRPC]
    void ReceiveTimerSecond(int second)
    {
        timerText.text = ((int)questionInterval - second).ToString();
    }

    [PunRPC]
    public void ReceiveQuestion(string question, int[] options, int correctIndex)
    {
        blockOptions.SetActive(false);
        questionText.text = question;
        for (int i = 0; i < 4; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = options[i].ToString();
            answerButtons[i].GetComponent<Image>().sprite = blueBtnSprite;
        }
        correctAnswerIndex = correctIndex;
    }

    public void SendAnswer(int optionNumber)
    {
        blockOptions.SetActive(true);
        if (ValidateAnswer(optionNumber))
        {
            answerButtons[optionNumber].GetComponent<Image>().sprite = greenBtnSprite;
        }
        else
        {
            answerButtons[correctAnswerIndex].GetComponent<Image>().sprite = greenBtnSprite;
            answerButtons[optionNumber].GetComponent<Image>().sprite = redBtnSprite;

        }
        photonView.RPC("ReceiveAnswer", RpcTarget.MasterClient, PhotonNetwork.NickName, optionNumber);
    }
    /*
    private void UpdatePlayerList()
    {
        
        StringBuilder playerList = new StringBuilder("Connected Players:\n");
        
        foreach(string playerName in scores.Keys)
        {
            playerList.AppendLine(playerName + ": "+scores[playerName].ToString());
        }

        playerListText.text = playerList.ToString();
    }
*/

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddOrUpdateEntry(newPlayer.NickName, 0);
    }

    

    public void OnStartClick()
    {
        SendQuestion();
    }

    public void AddOrUpdateEntry(string roll, int score)
    {
        if (leaderboardEntries.ContainsKey(roll))
        {
            leaderboardEntries[roll].UpdateScore(leaderboardEntries[roll].GetScore() + score);
        }
        else
        {
            GameObject row = Instantiate(rowPrefab, contentParent);
            LeaderboardEntry entry = row.GetComponent<LeaderboardEntry>();
            entry.SetData(roll, score);
            leaderboardEntries.Add(roll, entry);
        }
    }

}
