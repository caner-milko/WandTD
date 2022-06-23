using UnityEngine;

namespace wtd
{
	public class CameraManager : MonoBehaviour
	{
		public static CameraManager Instance { get; private set; }
		public Camera MainCam { get; private set; }

		public Vector3 WorldMousePos => MainCam.ScreenToWorldPoint(Input.mousePosition);

		public Ray MouseRay => MainCam.ScreenPointToRay(ScreenMousePos);

		public Vector2 ScreenMousePos => Input.mousePosition;

		void Awake()
		{
			Instance = this;
			if (MainCam == null)
				MainCam = Camera.main;
		}
	}
}
