using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.CloudAnchors;
using Photon.Pun;

namespace Scripts.Multiplayer
{
	public class GameManager : MonoBehaviour {

		// PUBLIC VARIABLES
    [SerializeField] private UIController UIController;
		[SerializeField] private GameObject camera;
		[SerializeField] private Laser laser;
		[SerializeField] private GameObject level1;

		// PRIVATE VARIABLES
		private Component m_cloudAnchor;
		private PlayerManager playerManager;

		public void setAnchor(Component anchor)
		{
				m_cloudAnchor = anchor;
				if (m_cloudAnchor == null)
				{
						UIController.ChangeText("Cloud Anchor object could not be found!");
				}
				else
				{
						playerManager.setTouchLock(false);
						playerManager.setAnchor(m_cloudAnchor);
						UIController.ChangeText("Found Cloud Anchor object!");

						UIController.SwitchToGameView();
						// Give a reference to the cloud anchor
						laser.cloudAnchorIsSpawned(m_cloudAnchor);
				}
		}

		// Use this for initialization
		void Start () {
				playerManager =  camera.GetComponent<PlayerManager>();
		}

		// Update is called once per frame
		void Update () {

		}

		public void startGame()
		{
				var level = PhotonNetwork.Instantiate(level1.name,
																							camera.transform.position,
																							camera.transform.rotation);
				// Anchor the level objects to the cloud anchor
				level.transform.parent = m_cloudAnchor.transform;
		}
	}
}
