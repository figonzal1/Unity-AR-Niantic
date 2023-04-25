using UnityEngine;

public class PlaceARObjectOnHAnd : MonoBehaviour
{

	[SerializeField] private HandPositionSolver solver;
	[SerializeField] private GameObject arObject;
	[SerializeField] private float speedMovement = 0.5f;
	[SerializeField] private float speedRotation = 25.0f;

	private float minDistance = 0.05f;
	private float minAngleMagnitude = 2.0f;
	private bool shouldAdjustRotation;



	// Update is called once per frame
	void Update()
	{
		PlaceARObjectOnHand(solver.HandPosition);
	}


	private void PlaceARObjectOnHand(Vector3 handPosition)
	{
		//Distancia entre objeto y mano
		float distance = Vector3.Distance(handPosition, arObject.transform.position);

		//Mover objeto
		arObject.transform.position = Vector3.MoveTowards(arObject.transform.position, handPosition, speedMovement * Time.deltaTime);

		if (distance >= minDistance)
		{
			//Objeto orientado a la position de la mano
			arObject.transform.LookAt(handPosition);
			shouldAdjustRotation = true;
		}
		else
		{
			if (shouldAdjustRotation)
			{
				arObject.transform.rotation = Quaternion.Slerp(arObject.transform.rotation, Quaternion.identity, 2 * Time.deltaTime);

				Vector3 angles = arObject.transform.rotation.eulerAngles;

                //Verificar si el objeto requiere de rotation
				shouldAdjustRotation = angles.magnitude >= minAngleMagnitude;
			}
			else
			{
                //Rotar
				arObject.transform.Rotate(Vector3.up * speedRotation * Time.deltaTime);
			}
		}
	}
}
