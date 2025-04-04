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

        while (opt.Count < 3)
        {
            int randomOption = Random.Range(ans - 50, ans + 50);
            if (randomOption != ans && !opt.Contains(randomOption))
                opt.Add(randomOption);
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
    }

    public void SendQuestion()
    {
        List<object> question = Question_Generator();
        (currentOptions, correctAnswerIndex) = AnswerOptions((int)question[1]);
        photonView.RPC("ReceiveQuestion", RpcTarget.Others, question[0], currentOptions.ToArray(), correctAnswerIndex);

        timer = 0f;
        isTimerRunning = true;
    }

    [PunRPC]
    public void ReceiveAnswer(string studentName, int optionPressed)
    {
        if (ValidateAnswer(optionPressed))
        {
            UpdateScores(studentName, 10);
        }
        else
        {
            UpdateScores(studentName, -5);
        }
    }

    bool ValidateAnswer(int optionPressed)
    {
        return optionPressed == correctAnswerIndex;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            if (timer >= questionInterval)
            {
                isTimerRunning = false;
                SendQuestion();
            }
        }
    }

    [PunRPC]
    public void ReceiveQuestion(string question, int[] options, int correctIndex)
    {
        questionText.text = question;
        for (int i = 0; i < 4; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = options[i].ToString();
        }
        correctAnswerIndex = correctIndex;
    }

    public void SendAnswer(int optionNumber)
    {
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
            playerList.AppendLine(playerName + scores[playerName].ToString());
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
