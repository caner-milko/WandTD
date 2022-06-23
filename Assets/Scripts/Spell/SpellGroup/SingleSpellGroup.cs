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
		public ActiveSpell Active { get; private set; }

		/// <summary>
		/// Child group of the group, might be null
		/// </summary>
		public SpellGroupBase ChildGroup { get; private set; }

		internal SingleSpellGroup(ISpellCaster caster, CastedSpell castedPrefab, List<PassiveSpell> passives, ActiveSpell active) : base(caster, castedPrefab, passives)
		{
			this.Active = active;
			this.ChildGroup = null;
		}

		internal SingleSpellGroup(ISpellCaster caster, CastedSpell castedPrefab, List<PassiveSpell> passives, ActiveSpell active, SpellGroupBase childGroup) : base(caster, castedPrefab, passives)
		{
			this.Active = active;
			this.ChildGroup = childGroup;
		}
		/// <summary>
		/// </summary>
		/// <returns>Cast delay total of the active spell and passive spells</returns>
		public override float GetCastDelay()
		{
			float delay = 0;
			foreach (PassiveSpell spell in Passives)
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
			foreach (PassiveSpell spell in Passives)
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
			return Active.castModifier;
		}

		/// <summary>
		/// Used if the spell group is a child of <see cref="MultiSpellGroup"/>
		/// </summary>
		/// <returns>Recharge delay total of the active spell and the child group</returns>
		public float GetRechargeDelayWOPassives()
		{
			return (ChildGroup != null ? ChildGroup.GetRechargeDelay() : 0.0f) + Active.rechargeModifier;
		}

		/// <summary>
		/// Cast the active of the group, return a list with a single item
		/// </summary>
		/// <param name="target">Target of the spell</param>
		/// <returns>A list with a single <see cref="CastedSpell"/></returns>
		public override List<CastedSpell> Cast(Vector3 position, ISpellTarget target)
		{
			List<CastedSpell> castedSpells = new();
			castedSpells.Add(CastSingle(position, target));
			return castedSpells;
		}

		public CastedSpell CastSingle(Vector3 position, ISpellTarget target)
		{
			CastedSpell casted = GameObject.Instantiate<CastedSpell>(castedPrefab);
			casted.Init(position, target, this);
			casted.Cast();
			return casted;
		}

		public List<CastedSpell> CastChild(Vector3 position, ISpellTarget target)
		{
			if (ChildGroup == null)
				return new List<CastedSpell>();
			return ChildGroup.Cast(position, target);
		}

	}
}
