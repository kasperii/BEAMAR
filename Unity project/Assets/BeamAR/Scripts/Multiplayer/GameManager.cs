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
		[SerializeField] private GameObject laser;
		[SerializeField] private GameObject level1;
		[SerializeField] private float offSetFromAnchorInY = 3f;

		// PRIVATE VARIABLES
		private Component m_cloudAnchor;
		private PlayerManager playerManager;
		private Laser laserScript;

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
						laserScript.coupleToAnchor(m_cloudAnchor);	// Set anchor as parent to laser
						// laser.transform.parent.gameObject.SetActive(true);
				}
		}

		// Use this for initialization
		void Start () {
				playerManager =  camera.GetComponent<PlayerManager>();
				laserScript =  laser.GetComponent<Laser>();
		}

		// Update is called once per frame
		void Update () {

		}

		public void startGame()
		{
				UIController.ButtonToGameView();

				var level = Instantiate(level1,
																m_cloudAnchor.transform.position + new Vector3(0,offSetFromAnchorInY,1),
																m_cloudAnchor.transform.rotation);
				// Anchor the level objects to the cloud anchor
				level.transform.parent = m_cloudAnchor.transform;
		}
	}
}
