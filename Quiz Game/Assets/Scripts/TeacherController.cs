using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherController : MonoBehaviourPun
{
    private int correctAnswerIndex;
    private List<int> currentOptions;

    public Text questionText;
    public Button[] answerButtons;

    private int currentQuestionIndex = 0;

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
    }

    [PunRPC]
    public void ReceiveAnswer(string studentName, int optionPressed)
    {
        if (ValidateAnswer(optionPressed))
        {
            Debug.Log(studentName + " answered correctly!");
            SendQuestion();
        }
        else
        {
            Debug.Log(studentName + " gave the wrong answer. Waiting for another attempt.");
        }
    }

    bool ValidateAnswer(int optionPressed)
    {
        return optionPressed == correctAnswerIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendQuestion();
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
}
