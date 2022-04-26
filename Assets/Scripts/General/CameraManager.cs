using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd
{
	public class CameraManager : MonoBehaviour
	{
		public static CameraManager instance { get; private set; }
		public Camera mainCam { get; private set; }

		public Vector2 WorldMousePos => mainCam.ScreenToWorldPoint(Input.mousePosition);

		public Vector2 ScreenMousePos => Input.mousePosition;

		public Vector2 MousePos;

		void Awake()
		{
			instance = this;
			if (mainCam == null)
				mainCam = Camera.main;
		}

		private void Update()
		{
			MousePos = ScreenMousePos;
		}
	}
}
