using UnityEngine;
using Mirror;

public class LobbyPlayer : NetworkRoomPlayer
{
    string localPlayerName;
    [SyncVar] public string playerName;
    [SyncVar] public string playerTeam;

    public override void OnGUI()
    {
        if (!showRoomGUI)
            return;

        NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
        if (room)
        {
            if (!room.showRoomGUI)
                return;

            if (!NetworkManager.IsSceneActive(room.RoomScene))
                return;

            DrawPlayerChangeName();
            DrawPlayerReadyState();
            DrawPlayerReadyButton();
            DrawPlayerTeam();
        }
    }


    void DrawPlayerReadyState()
    {
        GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 90f, 130f));

        GUILayout.Label(playerName);

        if (playerTeam == "red")
        {
            GUIStyle redText = new GUIStyle(GUI.skin.label);
            redText.normal.textColor = Color.red;
            GUILayout.Label("Red team", redText);
        }
        else
        {
            GUIStyle greenText = new GUIStyle(GUI.skin.label);
            greenText.normal.textColor = Color.green;
            GUILayout.Label("Green team", greenText);
        }

        if (readyToBegin)
            GUILayout.Label("Ready");
        else
            GUILayout.Label("Not Ready");

        if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("REMOVE"))
        {
            // This button only shows on the Host for all players other than the Host
            // Host and Players can't remove themselves (stop the client instead)
            // Host can kick a Player this way.
            GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
        }

        GUILayout.EndArea();
    }

    void DrawPlayerReadyButton()
    {
        if (NetworkClient.active && isLocalPlayer)
        {
            GUILayout.BeginArea(new Rect(20f, 300f, 120f, 20f));

            if (readyToBegin)
            {
                if (GUILayout.Button("Cancel"))
                    CmdChangeReadyState(false);
            }
            else
            {
                if (GUILayout.Button("Ready"))
                    CmdChangeReadyState(true);
            }

            GUILayout.EndArea();
        }
    }

    [Command]
    void CmdChangePlayerName(string newName)
    {
        playerName = newName;
    }

    void DrawPlayerChangeName()
    {
        if (NetworkClient.active && isLocalPlayer)
        {
            GUILayout.BeginArea(new Rect(20f, 130f, 90f, 130f));

            localPlayerName = GUILayout.TextField(localPlayerName, 14);

            CmdChangePlayerName(localPlayerName);

            GUILayout.EndArea();
        }
    }

    [Command]
    void CmdSetPlayerteam(string newTeam)
    {
        playerTeam = newTeam;
    }

    void DrawPlayerTeam()
    {
        if (NetworkClient.active && isLocalPlayer)
        {
            GUILayout.BeginArea(new Rect(130f, 130f, 120f, 130f));

            GUIStyle redText = new GUIStyle(GUI.skin.button);
            redText.normal.textColor = Color.red;

            GUIStyle greenText = new GUIStyle(GUI.skin.button);
            greenText.normal.textColor = Color.green;

            if (playerTeam == "red")
            {
                redText.normal.background = redText.active.background;

                GUILayout.Button("Join red team", redText);
                if (GUILayout.Button("Join green team", greenText))
                    CmdSetPlayerteam("green");
            }
            else
            {
                greenText.normal.background = greenText.active.background;

                if (GUILayout.Button("Join red team", redText))
                    CmdSetPlayerteam("red");
                GUILayout.Button("Join green team", greenText);
            }

            GUILayout.EndArea();
        }
    }

    public override void OnClientEnterRoom()
    {
        base.OnClientEnterRoom();

        localPlayerName = $"Player [{index + 1}]";
        playerName = $"Player [{index + 1}]";
        playerTeam = "red";
    }

    /*
    public override void OnStartClient()
    {
        base.OnStartClient();

        // Initially hide buttons for clients
        redTeam.gameObject.setActive(false);
        blueTeam.gameObject.setActive(false);
    }
    */

    /*
    public override void OnStartLocalClient()
    {
        base.OnStartLocalClient();

        names[index] = $"Player [{index + 1}]";

        
        // When client is set as local, enable buttons
        redTeam.gameObject.setActive(false);
        blueTeam.gameObject.setActive(false);

        // Add listeners
        redTeam.onClick.RemoveAllListeners();
        redTeam.onClick.AddListener(OnClickRed);

        blueTeam.onClick.RemoveAllListeners();
        blueTeam.onClick.AddListener(OnClickBlue);
        
    }
    */
    /*
    public void OnClickRed()
    {
        // Clicked red team (team nr 0)
        CmdSelectTeam(0);
    }

    public void OnClickBlue()
    {
        // Clicked blueteam (team nr 1)
        CmdSelectTeam(1);
    }

    [Command]
    public void CmdSelectTeam(int teamIndex)
    {
        // Set team of player on the server.
        team = teamIndex;
    }
    */

}