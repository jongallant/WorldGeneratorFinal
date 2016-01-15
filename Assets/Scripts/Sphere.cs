using UnityEngine;

public class Sphere : MonoBehaviour {

	[SerializeField]
	float RotationSpeed = 15;
	
	void Start () {
		
	}
	
	void Update () {
		transform.Rotate (Vector3.up, RotationSpeed * Time.deltaTime);
	}
}
