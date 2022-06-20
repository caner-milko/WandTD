using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	/// <summary>
	/// Contains multiple <see cref="SingleSpellGroup"/>s and their shared <see cref="PassiveSpell"/>s
	/// Inherits from <see cref="SpellGroupBase"/>
	/// </summary>
	public class MultiSpellGroup : SpellGroupBase
	{

		/// <summary>
		/// <see cref="SingleSpellGroup"/>s of the group, each has a <see cref="ActiveSpell"/>
		/// </summary>
		public List<SingleSpellGroup> spells { get; private set; } = new List<SingleSpellGroup>();

		internal MultiSpellGroup(ISpellCaster caster, CastedSpell castedPrefab, List<PassiveSpell> passives, List<SingleSpellGroup> spells) : base(caster, castedPrefab, passives)
		{
			this.spells = spells;
		}

		/// <summary>
		/// </summary>
		/// <returns>Cast delay total of the groups and the passives, <see cref="SingleSpellGroup.GetCastDelayWOPassives"/></returns>
		public override float GetCastDelay()
		{
			float delay = 0;
			foreach (PassiveSpell spell in passives)
			{
				delay += spell.castModifier;
			}
			foreach (SingleSpellGroup group in spells)
			{
				delay += group.GetCastDelayWOPassives();
			}
			return delay;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Recharge delay total of the groups and the passives <see cref="SingleSpellGroup.GetRechargeDelayWOPassives"/></returns>
		public override float GetRechargeDelay()
		{
			float delay = 0;
			foreach (PassiveSpell spell in passives)
			{
				delay += spell.castModifier;
			}
			foreach (SingleSpellGroup group in spells)
			{
				delay += group.GetRechargeDelayWOPassives();
			}
			return delay;
		}

		/// <summary>
		/// Cast each spell in the group
		/// </summary>
		/// <param name="target">Target of the spells</param>
		/// <returns>Created Casted Spells in the end</returns>
		public override List<CastedSpell> Cast(Vector3 position, ISpellTarget target)
		{
			List<CastedSpell> castedSpells = new List<CastedSpell>();
			foreach (SingleSpellGroup spg in spells)
				castedSpells.Add(spg.CastSingle(position, target));
			return castedSpells;
		}


	}
}
