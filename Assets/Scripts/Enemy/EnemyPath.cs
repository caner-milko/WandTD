using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd
{
	public class EnemyPath : MonoBehaviour
	{
		[field: SerializeField]
		public List<Vector3> positions { get; private set; } = new List<Vector3>();
	}
}
