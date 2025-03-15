using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] TMP_InputField teacherNameField;
    [SerializeField] TMP_InputField studentNameField;
    [SerializeField] TMP_InputField roomCodeField;

    [SerializeField] TextMeshProUGUI roomCodeText;

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
        string roomCode = Random.Range(1000, 9999).ToString();
        PhotonNetwork.NickName = teacherName;
        PhotonNetwork.CreateRoom(roomCode);
        roomCodeText.text = "Your room code is: " + roomCode;
        Debug.Log(roomCode);
    }

    public void OnJoinRoomBtnClick()
    {
        string studentName = studentNameField.text;
        string roomCode = roomCodeField.text;
        PhotonNetwork.NickName = studentName;
        PhotonNetwork.JoinRoom(roomCode);
    }
}

