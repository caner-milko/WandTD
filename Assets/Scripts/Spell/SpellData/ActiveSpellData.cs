using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	[CreateAssetMenu(fileName = "Active Spell Data", menuName = "Spells/Active Spell")]
	public class ActiveSpellData : SpellData
	{
		public int triggerCount = 0;
		public new ActiveSpell SpellPrefab => (ActiveSpell)base.SpellPrefab;

		public override void addToGroup(SpellGroupBuilder group)
		{
			group.AddChildSpellGroup(triggerCount);
		}

		public override Spell CastSpell(SingleSpellGroup group, CastedSpell casted)
		{
			ActiveSpell activeCreated = (ActiveSpell)base.CastSpell(group, casted);
			activeCreated.transform.parent = casted.transform;
			return activeCreated;
		}
	}
}
