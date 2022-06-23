using System.Collections.Generic;
using UnityEngine;
using wtd.spell;
using wtd.wand;

namespace wtd.tower
{
	public class Tower : MonoBehaviour, ISpellCaster, ISpellTarget
	{
		public Wand wand;
		public List<PassiveSpell> alwaysCast = new();

		public TowerTargetAdapter TargetAdapter;
		public TowerTarget Target => TargetAdapter != null ? TargetAdapter.Target : null;

		[field: SerializeField]
		public float Range { get; private set; }
		public float RangeSqr => Range * Range;

		public string CasterType()
		{
			return "SC_tower";
		}

		public float DistanceToSqr(Vector3 from)
		{
			return (GetPosition() - from).sqrMagnitude;
		}

		public Vector3 GetDirection(Vector3 from)
		{
			return (GetPosition() - from).normalized;
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}

		public string GetTargetType()
		{
			return "ST_tower";
		}

		public Vector3 GetVelocityVector(Vector3 from, float speed)
		{
			float maxSpeed = Mathf.Min(DistanceToSqr(from), speed * speed);

			return Mathf.Sqrt(maxSpeed) * GetDirection(from);

		}

		public CasterSpell NextSpell()
		{
			return wand.NextSpell();
		}

		public virtual bool Shoot(out List<CastedSpell> casted)
		{
			if (Target.GetTarget(out ISpellTarget selTarget))
			{
				return wand.Shoot(selTarget, out casted);
			}
			casted = null;
			return false;
		}

		public SpellContainer GetSpellContainer()
		{
			return wand.GetSpellContainer();
		}

	}
}