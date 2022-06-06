using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	[CreateAssetMenu(fileName = "Passive Spell Data", menuName = "Spells/Passive Spell")]
	public class PassiveSpellData : SpellData
	{
		public new PassiveSpell SpellPrefab => (PassiveSpell)base.SpellPrefab;


		public override void addToGroup(SpellGroupBuilder group)
		{
		}

		public override Spell CastSpell(SingleSpellGroup group, CastedSpell casted)
		{
			PassiveSpell passiveCreated = (PassiveSpell)base.CastSpell(group, casted);
			passiveCreated.transform.parent = casted.PassivesParent;
			return passiveCreated;
		}
	}
}
