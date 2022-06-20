using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wand;

namespace wtd.spell.targets
{
	public class FollowingSpellTarget : ISpellTarget
	{
		// Start is called before the first frame update
		public Transform following;

		public FollowingSpellTarget(Transform following)
		{
			this.following = following;
		}

		public float DistanceToSqr(Vector3 from)
		{
			return (following.position - from).sqrMagnitude;
		}

		public Vector3 GetDirection(Vector3 from)
		{
			return (following.position - from).normalized;
		}

		public Vector3 GetPosition()
		{
			return following.position;
		}

		public string GetTargetType()
		{
			return "ST_following";
		}

		public Vector3 GetVelocityVector(Vector3 from, float speed)
		{
			float maxSpeed = Mathf.Min(DistanceToSqr(from), speed * speed);

			return Mathf.Sqrt(maxSpeed) * GetDirection(from);

		}
	}
}
