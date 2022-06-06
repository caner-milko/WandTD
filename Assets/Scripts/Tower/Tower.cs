using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.spell;
using wtd.wand;

namespace wtd.tower
{
	public class Tower : MonoBehaviour, ISpellCaster, ISpellTarget
	{
		public Wand wand;
		public List<PassiveSpell> alwaysCast = new List<PassiveSpell>();

		[field: SerializeField]
		public TowerTargetAdapter TargetAdapter;
		public TowerTarget Target => TargetAdapter != null ? TargetAdapter.Target : null;

		[field: SerializeField]
		public float Range { get; private set; }
		public float RangeSqr => Range * Range;

		public string CasterType()
		{
			return "SC_tower";
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}

		public string GetTargetType()
		{
			return "ST_tower";
		}

		public CasterSpell NextSpell()
		{
			return wand.NextSpell();
		}

		public virtual bool shoot(out List<CastedSpell> casted)
		{
			ISpellTarget selTarget;
			if (Target.GetTarget(out selTarget))
			{
				return wand.Shoot(selTarget, out casted);
			}
			casted = null;
			return false;
		}

	}
}