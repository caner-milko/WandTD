using System;
using UnityEngine;
using wtd.stat;
namespace wtd.spell
{
	/// <summary>
	/// A spell has stats(will change), a unique static name <see cref="SpellName"/> and <see cref="AddToGroup(SpellGroupBuilder)"/>, ticking and casting methods
	/// </summary>
	public abstract class Spell : MonoBehaviour, IStatUser
	{
		protected bool spellActive = false;

		[Serializable]
		public struct SpellImage
		{
			public Sprite sprite;
			public Material material;
			public Color color;
		}
		[field: SerializeField]
		public SpellImage Image { get; private set; }
		public string spellName;
		public float castModifier, rechargeModifier;
		public int mana;

		/// <summary>
		/// Called when the spelldata is added to a SpellGroupBuilder
		/// </summary>
		/// <param name="group"></param>
		public abstract void AddToGroup(SpellGroupBuilder group);

		public virtual Spell CastSpell(SingleSpellGroup group, CastedSpell casted)
		{
			this.transform.localPosition = Vector3.zero;
			Spell created = GameObject.Instantiate<Spell>(this);
			return created;
		}

		public CastedSpell CastedParent { get; private set; }

		public virtual void Setup(CastedSpell castedParent)
		{
			if (this.CastedParent != null)
			{
				throw new NotSupportedException("Cannot set casted parent of a spell twice.");
			}
			this.CastedParent = castedParent;
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

		public virtual bool HitTrigger(CollisionSpellTrigger.CollisionSpellTriggerData triggerData)
		{
			return true;
		}

		public virtual bool TimerTrigger(TimedSpellTrigger.TimedSpellTriggerData triggerData)
		{
			return true;
		}



		public void Remove()
		{
			OnRemove();
		}

		protected abstract void OnRemove();

		public StatHolder GetStatHolder()
		{
			return CastedParent.StatHolder;
		}

	}
}
