using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace wtd.spell
{
	[Serializable]
	public abstract class SpellData : ScriptableObject
	{
		public string spellName;
		[SerializeField]
		private Spell spellPrefab;
		public float castModifier, rechargeModifier;
		public int mana;
		public Spell SpellPrefab => spellPrefab;


		/// <summary>
		/// Called when the spelldata is added to a SpellGroupBuilder
		/// </summary>
		/// <param name="group"></param>
		public abstract void addToGroup(SpellGroupBuilder group);


		public virtual Spell CastSpell(SingleSpellGroup group, CastedSpell casted)
		{
			Spell created = GameObject.Instantiate<Spell>(SpellPrefab);
			created.SpellData = this;
			return created;
		}
	}
}
