using System;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	/// <summary>
	/// A spell group has passive spells and it might have a parent
	/// Can be either <see cref="SingleSpellGroup"/> or <see cref="MultiSpellGroup"/>
	/// </summary>
	public abstract class SpellGroupBase
	{
		/// <summary>
		/// <see cref="PassiveSpell"/>s of the spell group
		/// </summary>
		public List<PassiveSpell> Passives { get; private set; } = new List<PassiveSpell>();

		/// <summary>
		/// Parent of group of the spell group, can be null
		/// </summary>
		public SpellGroupBase Parent { get; private set; }

		public bool HasParent
		{
			get
			{
				return Parent != null;
			}
		}

		protected readonly CastedSpell castedPrefab;

		/// <summary>
		/// Caster of the spell group
		/// </summary>
		public ISpellCaster Caster { get; private set; }

		protected SpellGroupBase(ISpellCaster caster, CastedSpell castedPrefab, List<PassiveSpell> passives)
		{
			this.Caster = caster;
			this.castedPrefab = castedPrefab;
			this.Passives = passives;
		}

		/// <summary>
		/// Total cast delay effect of the group, child groups don't effect the cast delay
		/// </summary>
		/// <returns>Cast delay of the group</returns>
		public abstract float GetCastDelay();

		/// <summary>
		/// Total cast delay effect of the group, child groups effect the recharge delay
		/// </summary>
		/// <returns>Recharge Delay of the group</returns>
		public abstract float GetRechargeDelay();

		/// <summary>
		/// Might return a single casted spell or multiple
		/// </summary>
		/// <param name="target">Target of the spell group</param>
		/// <returns>Casted spells of the group, might return a single casted spell if the group is <see cref="SingleSpellGroup"/></returns>
		public abstract List<CastedSpell> Cast(Vector3 position, ISpellTarget target);


		/// <summary>
		/// Set the parent of an unparented group
		/// </summary>
		/// <param name="parent">Parent of the group</param>
		public void SetParent(SpellGroupBase parent)
		{
			if (this.Parent != null)
			{
				throw new NotSupportedException("Trying to change the parent of a already parented group.");
			}
			this.Parent = parent;
		}

	}
}
