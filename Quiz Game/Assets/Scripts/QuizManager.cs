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

    public Text questionText;
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


    Dictionary<string, int> scores = new Dictionary<string, int>();

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

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => SendAnswer(index));
        }
        if (PhotonNetwork.IsMasterClient)
        {
            teacherPanel.SetActive(true);
            studentPanel.SetActive(false);
        }
        else
        {
            teacherPanel.SetActive(false);
            studentPanel.SetActive(true);
        }
    }

    public void SendQuestion()
    {
        List<object> question = Question_Generator();
        (currentOptions, correctAnswerIndex) = AnswerOptions((int)question[1]);
        photonView.RPC("ReceiveQuestion", RpcTarget.Others, question[0], currentOptions.ToArray(), correctAnswerIndex);

        timer = 0f;
        lastSentSecond = 0;
        isTimerRunning = true;
    }

    [PunRPC]
    public void ReceiveAnswer(string studentName, int optionPressed)
    {
        if (ValidateAnswer(optionPressed))
        {
            UpdateScores(studentName, 100);
        }
        else
        {
            UpdateScores(studentName, -25);
        }
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
                if (timer >= questionInterval)
                {
                    isTimerRunning = false;
                    SendQuestion();
                }
            }
        }
    }

    [PunRPC]
    void ReceiveTimerSecond(int second)
    {
        timerText.text = "Timer: " + (10-second);
    }

    [PunRPC]
    public void ReceiveQuestion(string question, int[] options, int correctIndex)
    {
        blockOptions.SetActive(false);
        questionText.text = question;
        for (int i = 0; i < 4; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = options[i].ToString();
        }
        correctAnswerIndex = correctIndex;
    }

    public void SendAnswer(int optionNumber)
    {
        blockOptions.SetActive(true);
        if (ValidateAnswer(optionNumber))
        {
            
        }
        else
        {

        }
        photonView.RPC("ReceiveAnswer", RpcTarget.MasterClient, PhotonNetwork.NickName, optionNumber);
    }

    private void UpdatePlayerList()
    {
        
        StringBuilder playerList = new StringBuilder("Connected Players:\n");
        /*
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.AppendLine(player.NickName);
        }
        */
        foreach(string playerName in scores.Keys)
        {
            playerList.AppendLine(playerName + ": "+scores[playerName].ToString());
        }

        playerListText.text = playerList.ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        scores.Add(newPlayer.NickName, 0);
        UpdatePlayerList();
    }

    private void UpdateScores(string name, int score)
    {
        scores[name] = scores[name] + score;
        UpdatePlayerList();
    }

    public void OnStartClick()
    {
        SendQuestion();
    }

}
