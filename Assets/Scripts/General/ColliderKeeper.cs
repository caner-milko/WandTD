using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd
{
	public class ColliderKeeper : MonoBehaviour
	{
		[field: SerializeField]
		public List<Collider> colliders { get; private set; } = new List<Collider>();

		[field: SerializeField]
		public LayerMask[] layers;

		private void OnTriggerEnter(Collider other)
		{
			bool add = false;
			foreach (LayerMask mask in layers)
			{
				if (mask.value == other.gameObject.layer)
				{
					add = true;
					break;
				}
			}
			if (add)
				colliders.Add(other);
		}

		private void OnTriggerExit(Collider other)
		{
			colliders.Remove(other);
		}
		public bool IsColliding(Collider other)
		{
			return colliders.Contains(other);
		}

		public bool IsColliding(Transform other)
		{
			foreach (Collider collider in colliders)
				if (collider.transform == other)
					return true;
			return false;
		}
	}
}
