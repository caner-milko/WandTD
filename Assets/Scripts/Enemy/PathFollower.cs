using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd
{
	public class PathFollower : MonoBehaviour
	{
		[field: SerializeField]
		public EnemyPath path { get; private set; }
		public int lastPoint = -1;
		public bool Finished => lastPoint >= path.positions.Count - 1;
		public Vector3 NextPosition
		{
			get
			{
				return Finished ? path.positions[lastPoint] : path.positions[lastPoint + 1];
			}
		}

		public float speed = 3f;

		void Start()
		{
			if (path != null && path.positions.Count > 0)
			{
				lastPoint = 0;
				transform.position = path.positions[0];
			}
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			if (!Finished)
			{
				if (CheckNext())
				{
					lastPoint++;
				}
				TranslateToNext();
			}
		}

		private void TranslateToNext()
		{
			Vector3 dif = NextPosition - transform.position;
			dif = dif.normalized * speed * Time.fixedDeltaTime;
			transform.Translate(dif);
		}

		private bool CheckNext()
		{
			Vector3 dif = NextPosition - transform.position;
			return dif.sqrMagnitude <= 0.03f;
		}

	}
}
