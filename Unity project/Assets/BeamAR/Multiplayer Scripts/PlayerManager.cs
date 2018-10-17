using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// using Photon.Realtime;

namespace BeamAR.MultiplayerScripts
{
	public class PlayerManager : MonoBehaviourPunCallbacks {

		[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
		public static GameObject LocalPlayerInstance;

		[SerializeField]
		private GameObject m_mirrorPrefab;

		// Max amount of mirrors to place
		int maxMirrors = 3;

		private Component m_cloudAnchor;

		// Help variables
		private int tapCount;
		private float doubleTapTimer;

		private bool touchLock = true;

		// Use this for initialization
		void Start () {
			// #Important
			// used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
			if (photonView.IsMine)
			{
					LocalPlayerInstance = gameObject;
			}
		}

		// Update is called once per frame
		void Update () {
			// If the app is locked from registering touch input
			if (touchLock)
			{
				return;
			}

			// Double tap input
			if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				tapCount++;
			}
			if (tapCount > 0)
			{
				doubleTapTimer += Time.deltaTime;
			}
			if (doubleTapTimer > 0.3f)
			{
				doubleTapTimer = 0f;
				tapCount = 0;
			}
			if (tapCount >= 2)
			{
				Handheld.Vibrate();
				var mirror = PhotonNetwork.Instantiate(m_mirrorPrefab.name, gameObject.transform.position,
																gameObject.transform.rotation);
				// Anchor the mirror with the cloud anchor object (plane)
				mirror.transform.parent = m_cloudAnchor.transform;

				//Reset
				doubleTapTimer = 0.0f;
				tapCount = 0;
			}
		}

		public void setTouchLock(bool state)
		{
			touchLock = state;
		}

		public void setAnchor(Component anchor)
		{
			m_cloudAnchor = anchor;
		}
	}
}
