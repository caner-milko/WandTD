using UnityEngine;

namespace wtd.spell.targets
{
	public class DirectedSpellTarget : ISpellTarget
	{
		public Vector3 direction;
		public DirectedSpellTarget(Vector3 direction)
		{
			this.direction = direction.normalized;
		}
		public float DistanceToSqr(Vector3 from)
		{
			return -1.0f;
		}

		public Vector3 GetDirection(Vector3 from)
		{
			return direction;
		}

		public Vector3 GetPosition()
		{
			return Vector3.zero;
		}

		public string GetTargetType()
		{
			return "ST_directed";
		}

		public Vector3 GetVelocityVector(Vector3 from, float speed)
		{
			return speed * GetDirection(from);

		}
	}
}
