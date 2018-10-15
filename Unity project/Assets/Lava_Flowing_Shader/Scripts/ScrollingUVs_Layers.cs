using UnityEngine;
using System.Collections;

/// <summary>
/// Script inputs a material's Main Texture and Bump Map, and offsets them according to a Vector2 uvAnimationRate.
/// </summary>
public class ScrollingUVs_Layers : MonoBehaviour 
{
	//public int materialIndex = 0;
	[SerializeField] private Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );   //How fast the UV is offset
	[SerializeField] private string MainTex = "_MainTex";  
    [SerializeField] private string BumpTex = "_BumpMap";
    
    Vector2 uvOffset = Vector2.zero;    //Zero vector to be incremented
	
	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset( MainTex, uvOffset );
            GetComponent<Renderer>().sharedMaterial.SetTextureOffset( BumpTex, uvOffset );
        }
	}
}