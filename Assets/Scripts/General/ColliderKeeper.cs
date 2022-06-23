using System.Collections.Generic;
using UnityEngine;

namespace wtd
{
	public class ColliderKeeper : MonoBehaviour
	{
		[field: SerializeField]
		public List<Collider> Colliders { get; private set; } = new List<Collider>();

		[field: SerializeField]
		public LayerMask[] Layers { get; private set; }

		private void OnTriggerEnter(Collider other)
		{
			bool add = false;
			foreach (LayerMask mask in Layers)
			{
				if (mask.value == other.gameObject.layer)
				{
					add = true;
					break;
				}
			}
			if (add)
				Colliders.Add(other);
		}

		private void OnTriggerExit(Collider other)
		{
			Colliders.Remove(other);
		}
		public bool IsColliding(Collider other)
		{
			return Colliders.Contains(other);
		}

		public bool IsColliding(Transform other)
		{
			foreach (Collider collider in Colliders)
				if (collider.transform == other)
					return true;
			return false;
		}
	}
}
