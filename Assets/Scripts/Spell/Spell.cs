using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	/// <summary>
	/// A spell has stats(will change), a unique static name <see cref="SpellName"/> and <see cref="addToGroup(SpellGroupBuilder)"/>, ticking and casting methods
	/// </summary>
	public abstract class Spell : MonoBehaviour
	{

		public CastedSpell castedParent;
		private SpellData spellData;
		public SpellData SpellData
		{
			get
			{
				return spellData;
			}

			set
			{
				spellData = value;
			}
		}

		protected bool spellActive = false;

		public void Setup(CastedSpell castedParent)
		{
			this.castedParent = castedParent;
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
