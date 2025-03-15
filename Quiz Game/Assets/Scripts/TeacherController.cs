using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TeacherController : MonoBehaviourPun
{
    public string[] questions = { "What is 2+2?", "What is the capital of France?" };
    public string[][] answers = {
        new string[] { "3", "4", "5", "6" },
        new string[] { "Berlin", "Madrid", "Paris", "Rome" }
    };
    public Text questionText;
    public Button[] answerButtons;
    private string selectedAnswer;

    private int currentQuestionIndex = 0;

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture correct index
            answerButtons[i].onClick.AddListener(() => SendAnswer(answerButtons[index].GetComponentInChildren<Text>().text));
        }
    }
    public void SendQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            photonView.RPC("ReceiveQuestion", RpcTarget.Others, questions[currentQuestionIndex], answers[currentQuestionIndex][0], answers[currentQuestionIndex][1], answers[currentQuestionIndex][2], answers[currentQuestionIndex][3]);
        }
    }

    [PunRPC]
    public void ReceiveAnswer(string studentName, string answer)
    {
        Debug.Log(studentName + " answered: " + answer);

        if (answer == answers[currentQuestionIndex][1]) // Assuming correct answer is at index 1
        {
            Debug.Log("Correct answer! Sending next question.");
            currentQuestionIndex++;
            SendQuestion();
        }
        else
        {
            Debug.Log("Wrong answer. Waiting for another attempt.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendQuestion();
        }
    }


    [PunRPC]
    public void ReceiveQuestion(string question, string answer1, string answer2, string answer3, string answer4)
    {
        questionText.text = question;
        answerButtons[0].GetComponentInChildren<Text>().text = answer1;
        answerButtons[1].GetComponentInChildren<Text>().text = answer2;
        answerButtons[2].GetComponentInChildren<Text>().text = answer3;
        answerButtons[3].GetComponentInChildren<Text>().text = answer4;
    }

    public void SendAnswer(string answer)
    {
        photonView.RPC("ReceiveAnswer", RpcTarget.MasterClient, PhotonNetwork.NickName, answer);
    }
}
