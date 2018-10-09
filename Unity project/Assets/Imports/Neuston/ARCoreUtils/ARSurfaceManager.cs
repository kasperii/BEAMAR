using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;

public class ARSurfaceManager : MonoBehaviour
{
	[SerializeField] Material[] m_surfaceMaterial;
	List<TrackedPlane> m_newPlanes = new List<TrackedPlane>();

    public Renderer rend;
    public bool StartFlag = false;

    [SerializeField] private GameObject PlayerMirror;
    [SerializeField] private GameObject StartBtn;

    Material m_Material;

    private int floorInt = 0;

    void Start()
    {
        //rend = GetComponent<Renderer>();
        m_Material = rend.sharedMaterial;
        rend.enabled = true;
        //rend.material = material[index];
        PlayerMirror.SetActive(false);
    }

    public void PressedStartBtn()
    {
        //floorInt = 1;
        StartFlag = true;
        PlayerMirror.SetActive(true);
        StartBtn.SetActive(false);
    }

    void Update()
	{
        m_Material = m_surfaceMaterial[floorInt];


        if (Session.Status != SessionStatus.Tracking)
		{
			return;
		}

		Session.GetTrackables(m_newPlanes, TrackableQueryFilter.New);


		foreach (var plane in m_newPlanes)
		{
			var surfaceObj = new GameObject("ARSurface");
            ((GameObject)surfaceObj).tag = "detectedPlane";     //Trying to fix collision with tag
            var arSurface = surfaceObj.AddComponent<ARSurface>();
			arSurface.SetTrackedPlane(plane, m_surfaceMaterial[floorInt]);
		}
	}
}
