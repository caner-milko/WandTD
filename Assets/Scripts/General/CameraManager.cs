using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd
{
	public class CameraManager : MonoBehaviour
	{
		public static CameraManager instance { get; private set; }
		public Camera mainCam { get; private set; }

		public Vector3 WorldMousePos => mainCam.ScreenToWorldPoint(Input.mousePosition);

		public Ray MouseRay => mainCam.ScreenPointToRay(ScreenMousePos);

		public Vector2 ScreenMousePos => Input.mousePosition;

		void Awake()
		{
			instance = this;
			if (mainCam == null)
				mainCam = Camera.main;
		}
	}
}
