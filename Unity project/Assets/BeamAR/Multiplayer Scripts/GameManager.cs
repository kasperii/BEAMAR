using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.CloudAnchors;

namespace BeamAR.MultiplayerScripts
{
	public class GameManager : MonoBehaviour {

		[SerializeField]
    private CloudAnchorUIController UIController;
		[SerializeField]
		private PlayerManager playerManager;

		private Component m_cloudAnchor;

		public void setAnchor(Component anchor)
		{
			m_cloudAnchor = anchor;
			if (m_cloudAnchor == null)
			{
				UIController.Debugger("Cloud Anchor object could not be found!");
			}
			else
			{
				playerManager.setTouchLock(false);
				playerManager.setAnchor(m_cloudAnchor);
				UIController.Debugger("Found Cloud Anchor object!");
			}
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}
	}
}
