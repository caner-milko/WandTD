using System.Collections;
using UnityEngine;
namespace wtd.spell
{
	[RequireComponent(typeof(Rigidbody))]
	public class CollisionSpellTrigger : SpellTrigger
	{
		public enum SpellCollisionType
		{
			ENEMY, ALLY, WALL
		}
		public struct CollisionSpellTriggerData
		{
			public readonly CollisionSpellTrigger trigger;
			public CastedSpell Casted => trigger.Casted;

			public readonly bool weak;

			public readonly bool isTrigger;

			public readonly SpellCollisionType collisionType;

			public readonly Collider otherCollider;

			public readonly Collision collision;

			public CollisionSpellTriggerData(CollisionSpellTrigger trigger, SpellCollisionType collisionType, bool weak, Collider collider)
			{
				this.trigger = trigger;
				this.isTrigger = true;
				this.collisionType = collisionType;
				this.weak = weak;
				this.otherCollider = collider;
				collision = null;
			}

			public CollisionSpellTriggerData(CollisionSpellTrigger trigger, SpellCollisionType collisionType, bool weak, Collision collision)
			{
				this.trigger = trigger;
				this.isTrigger = false;
				this.collisionType = collisionType;
				this.weak = weak;
				this.collision = collision;
				this.otherCollider = collision.collider;
			}
		}

		[field: SerializeField]
		public bool Weak { get; private set; } = false;

		[field: SerializeField]
		public LayerMask ExcludingLayers { get; private set; }

		[field: SerializeField]
		public bool CountTrigger { get; private set; } = false;

		[field: SerializeField, Range(-1, 50)]
		public int IgnoreCollisionForX { get; private set; } = 0;

		private int collidedFor = 0;

		[SerializeField]
		private float ImmuneFor = 0.1f;

		[field: SerializeField, ReadOnly]
		public bool IsImmune { get; private set; } = true;

		private void Awake()
		{
			StartCoroutine(WaitImmune());
		}

		private IEnumerator WaitImmune()
		{
			IsImmune = true;
			yield return new WaitForSeconds(ImmuneFor);
			IsImmune = false;
		}


		private void OnTriggerEnter(Collider other)
		{
			if (IsImmune)
				return;
			if (!CountTrigger)
				return;
			if ((ExcludingLayers.value & (1 << other.gameObject.layer)) != 0)
				return;
			Casted.HitTrigger(new CollisionSpellTriggerData(this, SpellCollisionType.ENEMY, Weak, other));
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (IsImmune)
				return;
			if ((ExcludingLayers.value & (1 << collision.gameObject.layer)) != 0)
				return;
			collidedFor++;
			if (IgnoreCollisionForX != -1 && collidedFor <= IgnoreCollisionForX)
				return;
			Casted.HitTrigger(new CollisionSpellTriggerData(this, SpellCollisionType.ENEMY, Weak, collision));
		}
	}
}
