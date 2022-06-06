using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.spell;

namespace wtd.wand
{
	public class Wand : MonoBehaviour, ISpellCaster, ISpellTarget
	{

		// Max number of spells this wand can hold
		public int capacity;

		// Currently remaining mana
		public int mana;

		// Base cast and recharge delay time for this wand
		public double castDelay, rechargeDelay;

		// Time past since start of cast and recharge delay
		public double curCastDelay, curRechargeDelay;

		// Number of spells cast at the same time
		public int castCount;

		// Owner of this wand
		public ISpellCaster owner;

		// Spells in this wand's slot
		public SpellContainer spells;

		// Spells yet to be cast since last recharge
		public Queue<CasterSpell> remSpells = new Queue<CasterSpell>();

		public SpellGroupBuilder curBuilder;

		public CastedSpell castedPrefab;

		private void Awake()
		{
			spells = new SpellContainer(capacity);
		}

		/// <summary>
		/// Casts spells towards given target.
		/// </summary>
		/// <param name="target">Target to casts spells to.</param>
		/// <param name="casted">List of casted spells.</param>
		/// <returns>true if at least one spell casted, false otherwise</returns>
		public bool Shoot(ISpellTarget target, out List<CastedSpell> casted)
		{
			SpellGroupBase group;
			if (PrepareSpellGroup(new List<PassiveSpell>(), out group))
			{
				casted = group.Cast(castedPrefab, target);
				return true;
			}
			casted = null;
			return false;
		}

		/// <summary>
		/// Prepares and returns group of spells to be cast. 
		/// </summary>
		/// <param name="passives">TODO: Add parameter description</param>
		/// <param name="group">Group of spells that are to be cast.</param>
		/// <returns>true if there is at least one spell to be cast in returned group, false otherwise.</returns>
		public bool PrepareSpellGroup(List<PassiveSpell> passives, out SpellGroupBase group)
		{
			// TODO: If there are no remaining spells return false
			if (remSpells.Count == 0)
			{
				remSpells = new Queue<CasterSpell>(spells);
			}
			SpellGroupBuilder builder = new SpellGroupBuilder(this, castCount);

			curBuilder = builder;

			group = builder.Build();
			return group != null;
		}

		public string CasterType()
		{
			return "CT_wand";
		}

		public string GetTargetType()
		{
			return "ST_wand";
		}

		public Vector3 GetPosition()
		{
			return owner.GetPosition();
		}

		/// <summary>
		/// Returns the next spell to be cast.
		/// </summary>
		/// <returns>Next spell to be cast.</returns>
		public CasterSpell NextSpell()
		{
			if (remSpells.Count == 0)
			{
				return null;
			}
			return remSpells.Dequeue();
		}


		public CasterSpell AddSpell(SpellData spell)
		{
			CasterSpell casterSpell = new CasterSpell(spell, this, spells.Count);
			spells.AddSpell(casterSpell);
			return casterSpell;
		}

	}
}
