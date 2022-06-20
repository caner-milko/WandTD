using System;
using System.Collections.Generic;
using UnityEngine;
using wtd.wand;


namespace wtd.spell.targets
{
	class StaticSpellTarget : ISpellTarget
	{
		public Vector3 position;

		public StaticSpellTarget(Vector3 position)
		{
			this.position = position;
		}

		public float DistanceToSqr(Vector3 from)
		{
			return (position - from).sqrMagnitude;
		}

		public Vector3 GetDirection(Vector3 from)
		{
			return (position - from).normalized;
		}

		public Vector3 GetPosition()
		{
			return position;
		}

		public string GetTargetType()
		{
			return "ST_static";
		}

		public Vector3 GetVelocityVector(Vector3 from, float speed)
		{
			float maxSpeed = Mathf.Min(DistanceToSqr(from), speed * speed);

			return Mathf.Sqrt(maxSpeed) * GetDirection(from);
		}
	}
}
