using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.CloudAnchors;
using Photon.Pun;
using Photon.Realtime;


public class PhotonServerController : MonoBehaviourPunCallbacks
{
	#region Public Fields

	// [Tooltip("The Ui Panel to let the user enter name, connect and play")]
	// [SerializeField]
	// private GameObject controlPanel;
	// [Tooltip("The UI Label to inform the user that the connection is in progress")]
	// [SerializeField]
	// private GameObject progressLabel;

	/// <summary>
	/// A controller for managing UI associated with the example.
	/// </summary>
	public CloudAnchorUIController UIController;

	#endregion

	#region Private Serializable Fields

	/// <summary>
	/// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
	/// </summary>
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	[SerializeField]
	private byte maxPlayersPerRoom = 2;

	#endregion


	#region Private Fields

	/// <summary>
	/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
	/// </summary>
	string gameVersion = "2";

	/// <summary>
	/// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
	/// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
	/// Typically this is used for the OnConnectedToMaster() callback.
	/// </summary>
	bool isConnecting;

	string roomName = "BeamAR";

	#endregion


	#region Public Methods

	/// <summary>
	/// Start the connection process.
	/// - If already connected, we attempt joining a random room
	/// - if not yet connected, Connect this application instance to Photon Cloud Network
	/// </summary>
	public void Connect()
	{
			// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
			isConnecting = true;

			// progressLabel.SetActive(true);
			// controlPanel.SetActive(false);

			// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
			if (PhotonNetwork.IsConnected) {
					UIController.Debugger("Joining room...");
					// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
					PhotonNetwork.JoinRandomRoom();
			}
			else
	    {
	        // #Critical, we must first and foremost connect to Photon Online Server.
					UIController.Debugger("Connecting to Photon...");
	        PhotonNetwork.GameVersion = gameVersion;
	        PhotonNetwork.ConnectUsingSettings();
	    }
	}

	public void Quit()
	{
		PhotonNetwork.Disconnect();
	}

	#endregion


	#region MonoBehaviour CallBacks

	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
	/// </summary>
	void Awake()
	{
			// #Critical
			// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.AutomaticallySyncScene = true;
	}

	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity during initialization phase.
	/// </summary>
	void Start()
	{
			// progressLabel.SetActive(false);
			// controlPanel.SetActive(true);
	}

	#endregion


	#region MonoBehaviourPunCallbacks CallBacks

	public override void OnConnectedToMaster()
	{
			UIController.Debugger("Connected to Photon!");

			// we don't want to do anything if we are not attempting to join a room.
			// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
			// we don't want to do anything.
			if (isConnecting)
			{
					UIController.Debugger("Entering Lobby...");
					PhotonNetwork.JoinRandomRoom();
			}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
			UIController.Debugger("No room! Create room...");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
	}

	public override void OnJoinedRoom()
	{
			// #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				// #Critical
				// Load the Room Level.
				// PhotonNetwork.LoadLevel("BeamAR_Host-Join");
			}

			UIController.Debugger("Room joined & ready to play! ");
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
			// progressLabel.SetActive(false);
			// controlPanel.SetActive(true);

			Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
	}

	#endregion

}
