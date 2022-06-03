using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.stat;
namespace wtd.enemy
{
	public class PathFollower : MonoBehaviour
	{
		[SerializeField]
		private StatHolder holder;

		private Stat speedStat => holder.GetStat("speed", false);

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

		[SerializeField]
		private bool loop = false;

		private void Awake()
		{
			if (holder == null || !holder)
				holder = GetComponent<StatHolder>();
		}

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
			else if (loop)
			{
				lastPoint = -1;
			}
		}

		private void TranslateToNext()
		{
			Vector3 dif = NextPosition - transform.position;
			dif = dif.normalized * speedStat.Value * Time.fixedDeltaTime;
			transform.Translate(dif);
		}

		private bool CheckNext()
		{
			Vector3 dif = NextPosition - transform.position;
			return dif.sqrMagnitude <= 0.03f;
		}

	}
}
