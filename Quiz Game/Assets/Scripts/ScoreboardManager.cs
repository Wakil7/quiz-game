using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreboardManager : MonoBehaviourPunCallbacks
{
    public Text leaderboardText;

    void Start()
    {
        UpdateLeaderboard();
    }

    void UpdateLeaderboard()
    {
        string leaderboard = "Leaderboard:\n";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            int score = PlayerPrefs.GetInt("Player" + player.ActorNumber, 0);
            leaderboard += player.NickName + " - " + score + "\n";
        }
        leaderboardText.text = leaderboard;
    }

    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenuScene");
    }
}

