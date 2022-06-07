using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.stat;
using System;
namespace wtd.spell
{
	/// <summary>
	/// A spell has stats(will change), a unique static name <see cref="SpellName"/> and <see cref="addToGroup(SpellGroupBuilder)"/>, ticking and casting methods
	/// </summary>
	public abstract class Spell : MonoBehaviour
	{
		protected bool spellActive = false;

		public string spellName;
		public float castModifier, rechargeModifier;
		public int mana;

		[field: SerializeField]
		public List<Stat> DefaultStats { get; private set; }

		/// <summary>
		/// Called when the spelldata is added to a SpellGroupBuilder
		/// </summary>
		/// <param name="group"></param>
		public abstract void addToGroup(SpellGroupBuilder group);

		public virtual Spell CastSpell(SingleSpellGroup group, CastedSpell casted)
		{
			Spell created = GameObject.Instantiate<Spell>(this);
			return created;
		}

		public CastedSpell castedParent { get; private set; }

		public virtual void Setup(CastedSpell castedParent)
		{
			if (this.castedParent != null)
			{
				throw new NotSupportedException("Cannot set casted parent of a spell twice.");
			}
			this.castedParent = castedParent;
			foreach (Stat stat in DefaultStats)
			{
				castedParent.statHolder.AddStat(stat, true);
			}
		}

		public void Cast()
		{
			OnCast();
			spellActive = true;
		}

		/// <summary>
		/// Called on cast
		/// </summary>
		protected abstract void OnCast();


		protected virtual void Update()
		{
			if (spellActive)
				OnUpdate();
		}

		protected abstract void OnUpdate();

		protected virtual void FixedUpdate()
		{
			if (spellActive)
				OnFixedUpdate();
		}

		protected abstract void OnFixedUpdate();

		public void Trigger(SpellTriggerData trigger)
		{

		}

		protected abstract void OnTrigger(SpellTriggerData trigger);

		public void Remove()
		{
			OnRemove();
		}

		protected abstract void OnRemove();

	}
}
