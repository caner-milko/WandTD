using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{

	/// <summary>
	/// Contains a single <see cref="ActiveSpell"/><br/>
	/// Might contain a child group if the active spell is a trigger spell<br/>
	/// Inherits from <see cref="SpellGroupBase"/>
	/// </summary>
	public class SingleSpellGroup : SpellGroupBase
	{
		/// <summary>
		/// Active spell of the group
		/// </summary>
		public ActiveSpell active { get; private set; }

		/// <summary>
		/// Child group of the group, might be null
		/// </summary>
		public SpellGroupBase childGroup { get; private set; }

		internal SingleSpellGroup(ISpellCaster caster, List<PassiveSpell> passives, ActiveSpell active) : base(caster, passives)
		{
			this.active = active;
			this.childGroup = null;
		}

		internal SingleSpellGroup(ISpellCaster caster, List<PassiveSpell> passives, ActiveSpell active, SpellGroupBase childGroup) : base(caster, passives)
		{
			this.active = active;
			this.childGroup = childGroup;
		}
		/// <summary>
		/// </summary>
		/// <returns>Cast delay total of the active spell and passive spells</returns>
		public override float GetCastDelay()
		{
			float delay = 0;
			foreach (PassiveSpell spell in passives)
			{
				delay += spell.castModifier;
			}
			delay += GetCastDelayWOPassives();
			return delay;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Recharge delay total of the active spell, passive spells and the child group</returns>
		public override float GetRechargeDelay()
		{
			float delay = 0;
			foreach (PassiveSpell spell in passives)
			{
				delay += spell.castModifier;
			}
			delay += GetRechargeDelayWOPassives();
			return delay;
		}

		/// <summary>
		/// Used if the spell group is a child of <see cref="MultiSpellGroup"/>
		/// </summary>
		/// <returns>Cast delay of the active spell</returns>
		public float GetCastDelayWOPassives()
		{
			return active.castModifier;
		}

		/// <summary>
		/// Used if the spell group is a child of <see cref="MultiSpellGroup"/>
		/// </summary>
		/// <returns>Recharge delay total of the active spell and the child group</returns>
		public float GetRechargeDelayWOPassives()
		{
			return (childGroup != null ? childGroup.GetRechargeDelay() : 0.0f) + active.rechargeModifier;
		}

		/// <summary>
		/// Cast the active of the group, return a list with a single item
		/// </summary>
		/// <param name="target">Target of the spell</param>
		/// <returns>A list with a single <see cref="CastedSpell"/></returns>
		public override List<CastedSpell> Cast(CastedSpell castedPrefab, ISpellTarget target)
		{
			List<CastedSpell> castedSpells = new List<CastedSpell>();
			castedSpells.Add(CastSingle(castedPrefab, target));
			return castedSpells;
		}

		public CastedSpell CastSingle(CastedSpell castedPrefab, ISpellTarget target)
		{
			CastedSpell casted = GameObject.Instantiate<CastedSpell>(castedPrefab);
			casted.Init(target, this);
			casted.Cast();
			return casted;
		}

	}
}
