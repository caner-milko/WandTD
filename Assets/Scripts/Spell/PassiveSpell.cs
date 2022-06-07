using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	/// <summary>
	/// Every passive spell should be inherited from this class
	/// </summary>
	public abstract class PassiveSpell : Spell
	{
		public override Spell CastSpell(SingleSpellGroup group, CastedSpell casted)
		{
			PassiveSpell passiveCreated = (PassiveSpell)base.CastSpell(group, casted);
			passiveCreated.transform.parent = casted.PassivesParent;
			return passiveCreated;
		}

		public override void addToGroup(SpellGroupBuilder group)
		{

		}
	}
}
