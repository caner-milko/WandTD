using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.enemy
{
	public class EnemyPath : MonoBehaviour
	{
		[field: SerializeField]
		public List<Vector3> positions { get; private set; } = new List<Vector3>();

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.815f, 0.360f, 0.086f);
			foreach (Vector3 pos in positions)
			{
				Gizmos.DrawSphere(pos, 0.15f);
			}
			for (int i = 1; i < positions.Count; i++)
			{
				Gizmos.DrawLine(positions[i - 1], positions[i]);
			}
		}
	}
}
