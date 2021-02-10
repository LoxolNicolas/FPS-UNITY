using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    public static int redTeamStartPositionIndex = 0;
    public static int greenTeamStartPositionIndex = 0;

    public static List<Transform> redTeamStartPositions = new List<Transform>();
    public static List<Transform> greenTeamStartPositions = new List<Transform>();

    public override void Start()
    {
        base.Start();

        foreach(GameObject rs in GameObject.FindGameObjectsWithTag("redTeamSpawn"))
        {
            Debug.Log("red spawn found");
            redTeamStartPositions.Add(rs.transform);
        }
        
        foreach(GameObject gs in GameObject.FindGameObjectsWithTag("greenTeamSpawn"))
        {
            Debug.Log("green spawn found");
            greenTeamStartPositions.Add(gs.transform);
        }

        Debug.Log("total red spawns: " + redTeamStartPositions.Count);
        Debug.Log("total green spawns: " + greenTeamStartPositions.Count);
    }

    // for users to apply settings from their room player object to their in-game player object
    /// <summary>
    /// This is called on the server when it is told that a client has finished switching from the room scene to a game player scene.
    /// <para>When switching from the room, the room-player is replaced with a game-player object. This callback function gives an opportunity to apply state from the room-player to the game-player object.</para>
    /// </summary>
    /// <param name="conn">The connection of the player</param>
    /// <param name="roomPlayer">The room player object.</param>
    /// <param name="gamePlayer">The game player object.</param>
    /// <returns>False to not allow this player to replace the room player.</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        if(conn == null)
        {
            Debug.Log("null connection");
            return false;
        }
        gamePlayer.GetComponent<TankSetup>().playerName = roomPlayer.GetComponent<LobbyPlayer>().playerName; // transmit player name
        gamePlayer.GetComponent<TankSetup>().playerTeam = roomPlayer.GetComponent<LobbyPlayer>().playerTeam; // transmit player team

        return true;
    }

    Transform CustomGetStartPosition(string team)
    {
        Transform startPosition = null;
        switch(team)
        {
            case "red":
                if (redTeamStartPositions.Count != 0)
                    startPosition = redTeamStartPositions[redTeamStartPositionIndex];
                    redTeamStartPositionIndex = (redTeamStartPositionIndex + 1) % redTeamStartPositions.Count;
                break;

            case "green":
                if (greenTeamStartPositions.Count != 0)
                    startPosition = greenTeamStartPositions[greenTeamStartPositionIndex];
                    greenTeamStartPositionIndex = (greenTeamStartPositionIndex + 1) % greenTeamStartPositions.Count;
                break;

            default:
                break;
        }

        return startPosition;
    }

    public override void SceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer)
    {
        if (IsSceneActive(RoomScene))
        {
            // cant be ready in room, add to ready list
            PendingPlayer pending;
            pending.conn = conn;
            pending.roomPlayer = roomPlayer;
            pendingPlayers.Add(pending);
            return;
        }

        GameObject gamePlayer = OnRoomServerCreateGamePlayer(conn, roomPlayer);
        if (gamePlayer == null)
        {
            // get start position from base class
            Transform startPos = CustomGetStartPosition(roomPlayer.GetComponent<LobbyPlayer>().playerTeam);
            gamePlayer = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
            return;

        // replace room player with game player
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
    }
}
