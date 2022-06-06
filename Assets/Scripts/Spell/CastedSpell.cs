using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.stat;
using wtd.effect;
namespace wtd.spell
{
	/// <summary>
	/// Every spell <see cref="GameObject"/> should be a <see cref="CastedSpell"/><br/>
	/// Every casted spell contains a <see cref="SingleSpellGroup"/> in it, its target 
	/// </summary>
	public class CastedSpell : MonoBehaviour
	{
		public SingleSpellGroup spellGroup { get; private set; }

		public Transform PassivesParent;
		public ISpellTarget target { get; private set; }
		[field: SerializeField]
		public StatHolder statHolder { get; private set; }
		[field: SerializeField]
		public EffectHolder effectHolder { get; private set; }

		public bool casted { get; private set; } = false;

		public ActiveSpell active { get; private set; }
		public List<PassiveSpell> passives { get; private set; } = new List<PassiveSpell>();

		private void Awake()
		{
			if (statHolder == null || !statHolder)
			{
				statHolder = GetComponent<StatHolder>();
			}
			if (effectHolder == null || !effectHolder)
			{
				effectHolder = GetComponent<EffectHolder>();
			}
			if (PassivesParent == null || !PassivesParent)
			{
				PassivesParent = new GameObject("Passives").transform;
				PassivesParent.parent = transform;
			}
		}

		public void Init(ISpellTarget target, SingleSpellGroup spellGroup)
		{
			this.target = target;
			this.spellGroup = spellGroup;

			this.active = (ActiveSpell)spellGroup.active.CastSpell(spellGroup, this);
			foreach (PassiveSpellData passivesData in spellGroup.passives)
			{
				this.passives.Add((PassiveSpell)passivesData.CastSpell(spellGroup, this));
			}

			active.Setup(this);
			foreach (PassiveSpell passive in passives)
				passive.Setup(this);
		}

		public void Cast()
		{
			active.Cast();
			foreach (PassiveSpell passive in passives)
			{
				passive.Cast();
			}
		}

	}
}