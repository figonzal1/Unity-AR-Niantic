using System;
using System.Collections;
using System.Collections.Generic;
using ARDK.Extensions;
using Niantic.ARDK.AR.Awareness;
using UnityEngine;

public class HandPositionSolver : MonoBehaviour
{

	[SerializeField] private ARHandTrackingManager _handTrackingManager;
	[SerializeField] private Camera arCamera;
	[SerializeField] private float minHandConfidence = 0.85f;

	//Save hand position
	private Vector3 handPosition;

	public Vector3 HandPosition { get => handPosition; }

	void Start()
	{
		_handTrackingManager.HandTrackingUpdated += HandTrackingUpdate;
	}

	private void HandTrackingUpdate(HumanTrackingArgs handData)
	{
		var detections = handData.TrackingData?.AlignedDetections;

		//Si no detecta manos, retorna
		if (detections == null)
		{
			return;
		}

		//Si detecta manos, revisamos el bucle
		foreach (var detection in detections)
		{
			if (detection.Confidence < minHandConfidence)
			{
				return;
			}

			Vector3 detectionSize = new Vector3(detection.Rect.width, detection.Rect.height, 0);

			float depthEstimation = 0.2f + Math.Abs(1 - detectionSize.magnitude);

			handPosition = arCamera.ViewportToWorldPoint(new Vector3(detection.Rect.center.x, 1 - detection.Rect.center.y, depthEstimation));

		}
	}

	private void OnDestroy()
	{
		_handTrackingManager.HandTrackingUpdated -= HandTrackingUpdate;
	}
}
