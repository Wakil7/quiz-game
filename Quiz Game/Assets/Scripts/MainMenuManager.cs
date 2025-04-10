using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    /*
    [SerializeField] GameObject loadingTxt;
    [SerializeField] Text roomTxt;
    [SerializeField] InputField joinRoom;
    private string createdRoomCode = "";

    private void Start()
    {
        loadingTxt.SetActive(true);
        PhotonNetwork.NickName = "Wakil"; // Set player nickname
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        loadingTxt.SetActive(false);
    }

    public void CreateRoom()
    {
        createdRoomCode = Random.Range(1000, 9999).ToString();
        PhotonNetwork.CreateRoom(createdRoomCode);
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(joinRoom.text))
        {
            PhotonNetwork.JoinRoom(joinRoom.text);
        }
        else
        {
            Debug.LogWarning("Please enter a valid room code.");
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(createdRoomCode);
        PhotonNetwork.IsMessageQueueRunning = false; // Pause incoming messages
        SceneManager.LoadScene("QuizScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Failed to join room: " + message);
    }

    public void OnStartClicked()
    {
        // Implement start game logic if needed
    }
    */

    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject firstPanel;
    [SerializeField] GameObject studentPanel;
    [SerializeField] GameObject teacherPanel;
    //[SerializeField] GameObject tempObject;

    [SerializeField] TMP_InputField teacherNameField;
    [SerializeField] TMP_InputField studentNameField;
    [SerializeField] TMP_InputField roomCodeField;

    static List<object> questions;
    [SerializeField] TMP_InputField questionField;
    [SerializeField] TMP_InputField optionField0;
    [SerializeField] TMP_InputField optionField1;
    [SerializeField] TMP_InputField optionField2;
    [SerializeField] TMP_InputField optionField3;
    [SerializeField] GameObject createQuestionsPanel;
    [SerializeField] GameObject createRoomPanel;

    //[SerializeField] TextMeshProUGUI roomCodeText;
    public static string roomCode;

    private void Start()
    {
        loadingPanel.SetActive(true);
        firstPanel.SetActive(false);
        studentPanel.SetActive(false);
        teacherPanel.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = false; // Pause incoming messages
        PhotonNetwork.LoadLevel("QuizScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Failed to join room: " + message);
    }

    public override void OnJoinedLobby()
    {
        loadingPanel.SetActive(false);
        firstPanel.SetActive(true);
    }

    public void OnTeacherBtnClick()
    {
        teacherPanel.SetActive(true);
        firstPanel.SetActive(false);
    }

    public void OnStudentBtnClick() {
        studentPanel.SetActive(true);
        firstPanel.SetActive(false);
    }

    public void OnCreateRoomBtnClick()
    {
        string teacherName = teacherNameField.text;
        roomCode = Random.Range(1000, 9999).ToString();
        PhotonNetwork.NickName = teacherName;
        PhotonNetwork.CreateRoom(roomCode);
        //roomCodeText.text = "Your room code is: " + roomCode;
        Debug.Log(roomCode);
    }

    public void OnJoinRoomBtnClick()
    {
        string studentName = studentNameField.text;
        string roomCode = roomCodeField.text;
        PhotonNetwork.NickName = studentName;
        PhotonNetwork.JoinRoom(roomCode);
    }

    public void OnAddQuestionButtonClicked()
    {
        List<string> questionAnswers = new List<string>();
        string que = questionField.text.Trim();
        string opt0 = optionField0.text.Trim();
        string opt1 = optionField1.text.Trim();
        string opt2 = optionField2.text.Trim();
        string opt3 = optionField3.text.Trim();
        if (que == "" || opt0 == "" || opt1 == "" || opt2 == "" || opt3 == "")
        {
            Debug.Log("Invalid");
        }
        else
        {
            questionAnswers.Add(que);
            questionAnswers.Add(opt0);
            questionAnswers.Add(opt1);
            questionAnswers.Add(opt2);
            questionAnswers.Add(opt3);
            questions.Add(questionAnswers);
        }

    }
    public void OnDoneClick()
    {
        createQuestionsPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }
    
}

