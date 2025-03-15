using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class StudentController : MonoBehaviourPun
{
    public Text questionText;
    public Button[] answerButtons;
    private string selectedAnswer;

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture correct index
            answerButtons[i].onClick.AddListener(() => SendAnswer(answerButtons[index].GetComponentInChildren<Text>().text));
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

