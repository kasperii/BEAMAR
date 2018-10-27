using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;

public class ARSurfaceManager : MonoBehaviour
{
	[SerializeField] Material[] m_surfaceMaterial;
	List<TrackedPlane> m_newPlanes = new List<TrackedPlane>();

    public Renderer rend;

    Material m_Material;

    private int floorInt = 0;
    [SerializeField] private GameObject m_GameView;

    void Start()
    {
        //rend = GetComponent<Renderer>();
        m_Material = rend.sharedMaterial;
        rend.enabled = true;
        //rend.material = material[index];
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
