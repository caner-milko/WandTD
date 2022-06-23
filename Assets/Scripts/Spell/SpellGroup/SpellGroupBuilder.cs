using System.Collections.Generic;
using System.Linq;

namespace wtd.spell
{
	//A builder builds the spell group required for creating the active spell
	//It works recursively, adding new spell groups as childs if needed(mainly for trigger spells)
	//Every time a spell is added to the builder, a method is called in the spell
	//For example, multicast passive spells increase the remaining cast count
	public class SpellGroupBuilder
	{
		/// <summary>
		/// passives in the group
		/// </summary>
		public List<PassiveSpell> Passives { get; private set; } = new List<PassiveSpell>();
		/// <summary>
		/// active spell is null if the group is a multicast group
		/// </summary>
		public ActiveSpell Active { get; private set; }
		/// <summary>
		/// 2 cases:
		/// 1. Group is multicast, then childs are <see cref="SingleSpellGroup"/>s which will create the <see cref="MultiSpellGroup"/> at the end
		/// 2. Group is single cast, then childs contains only 1 element and it is the child group of the active spell
		/// </summary>
		public List<SpellGroupBase> ChildGroups { get; private set; } = new List<SpellGroupBase>();

		/// <summary>
		/// caster of the group
		/// </summary>
		public ISpellCaster Caster { get; private set; }

		public CastedSpell CastedPrefab { get; private set; }

		/// <summary>
		/// if group is multicast, create a MultiSpellGroup at the end, if not create a SingleSpellGroup
		/// MultiSpellGroup consists of multiple <see cref="SingleSpellGroup"> which share the same passives
		/// </summary>
		private bool multiCast;
		/// <summary>
		/// current remaining cast count
		/// </summary>
		private int remCastCount;

		public SpellGroupBuilder(ISpellCaster caster, CastedSpell castedPrefab, int castCount) : this(caster, castedPrefab, castCount, new())
		{
		}

		/// <summary>
		/// A spell group is created with a caster and given castCount
		/// </summary>
		/// <param name="caster">Caster who shoot the spell, generally a wand</param>
		/// <param name="castCount">How many spells should be cast at once in the group</param>
		public SpellGroupBuilder(ISpellCaster caster, CastedSpell castedPrefab, int castCount, List<PassiveSpell> passives)
		{
			this.Caster = caster;
			this.CastedPrefab = castedPrefab;
			///group is multicast if there are multiple spells cast
			///may become a multicastspell later with the <see cref="IncreaseRemCastCount(int)"/> method
			this.multiCast = castCount > 1;
			this.remCastCount = castCount;
			foreach (PassiveSpell spell in passives)
			{
				Passives.Add(spell);
				spell.AddToGroup(this);
			}
			StartBuild();
		}
		/// <summary>
		/// Creates a spell group from the given CasterSpell
		/// Must be a single cast group at the end
		/// </summary>
		/// <param name="spell">active spell of the group</param>
		private SpellGroupBuilder(CasterSpell spell, CastedSpell castedPrefab)
		{
			this.Caster = spell.Owner;
			this.CastedPrefab = castedPrefab;
			this.multiCast = false;
			this.remCastCount = 0;
			this.Active = (ActiveSpell)spell.Spell;
			//might be a trigger spell
			spell.Spell.AddToGroup(this);
			StartBuild();
		}

		private void StartBuild()
		{
			/// add spells until 0 remaining casts or there are no remaining spells in the caster 
			// TODO: reached mana limit
			while (remCastCount > 0)
			{
				CasterSpell selected = Caster.NextSpell();
				//no remaining spells
				if (selected == null)
					break;

				//TODO: manacheck

				//if the selected spell is active, add as a active spell in the childGroups or as the active spell
				if (selected.IsActive)
				{
					if (multiCast)
					{
						ChildGroups.Add(new SpellGroupBuilder(selected, CastedPrefab).Build());
						remCastCount--;
					}
					else
					{
						this.Active = (ActiveSpell)selected.Spell;
						selected.Spell.AddToGroup(this);
						break;
					}
				}
				//Else, add as a passive spell
				else
				{
					Passives.Add((PassiveSpell)selected.Spell);
					selected.Spell.AddToGroup(this);
				}
			}
		}
		/// <summary>
		/// Add castCount amount of spells as child group
		/// Only used for single casts
		/// Mainly used for trigger spells <see cref="ActiveSpell.AddToGroup(SpellGroupBuilder)"/> 
		///</summary>
		/// <param name="castCount">CastCount of the group to be added as child, can be 0 which will do nothing</param>
		public void AddChildSpellGroup(int castCount)
		{
			if (castCount > 0)
			{
				SpellGroupBase cgb = new SpellGroupBuilder(Caster, CastedPrefab, castCount).Build();
				if (cgb != null)
					ChildGroups.Add(cgb);
			}
		}

		/// <summary>
		/// Build the resulting <see cref="SpellGroupBase"/>
		/// </summary>
		/// <returns> Can be a <see cref="MultiSpellGroup"/> or <see cref="SingleSpellGroup"/> based on cast count</returns>
		public SpellGroupBase Build()
		{
			/// if multicast, then all child groups are <see cref="SingleSpellGroup"/>s
			/// Convert the list
			/// configure the child groups
			if (multiCast)
			{
				List<SingleSpellGroup> spellGroups = ChildGroups.Cast<SingleSpellGroup>().ToList();
				MultiSpellGroup group = new(Caster, CastedPrefab, Passives, spellGroups);
				foreach (SingleSpellGroup singleSpellGroup in spellGroups)
				{
					singleSpellGroup.Passives.AddRange(Passives);
					singleSpellGroup.SetParent(group);
				}
				return group;
			}
			else
			{
				if (Active == null)
					return null;
				///if has a child group, configure the child group then return
				if (ChildGroups.Count > 0)
				{
					SingleSpellGroup spg = new(Caster, CastedPrefab, Passives, Active, ChildGroups[0]);
					spg.ChildGroup.SetParent(spg);
					return spg;
				}
				else
				{
					return new SingleSpellGroup(Caster, CastedPrefab, Passives, Active);
				}

			}
		}

		/// <summary>
		/// increase the remaining cast count
		/// if the <paramref name="amount"/> is bigger than 0, then change the group to multicast
		/// </summary>
		/// <param name="amount">amount to increase <see cref="remCastCount"/></param>
		public void IncreaseRemCastCount(int amount)
		{
			remCastCount += amount;
			multiCast = multiCast || (amount > 0);
		}

	}
}