using System.Collections;
using System.Collections.Generic;
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
			public CastedSpell casted => trigger.casted;

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
		public bool weak { get; private set; } = false;

		[field: SerializeField]
		public LayerMask layer { get; private set; }

		[field: SerializeField]
		public bool countTrigger { get; private set; } = false;

		[field: SerializeField, Range(-1, 50)]
		public int ignoreCollisionForX { get; private set; } = 0;

		private int collidedFor = 0;
		private void OnTriggerEnter(Collider other)
		{
			if (!countTrigger)
				return;
			if ((layer.value & (1 << other.gameObject.layer)) == 0)
				return;
			casted.HitTrigger(new CollisionSpellTriggerData(this, SpellCollisionType.ENEMY, weak, other));
		}

		private void OnCollisionEnter(Collision collision)
		{
			if ((layer.value & (1 << collision.gameObject.layer)) == 0)
				return;
			collidedFor++;
			if (ignoreCollisionForX != -1 && collidedFor <= ignoreCollisionForX)
				return;
			casted.HitTrigger(new CollisionSpellTriggerData(this, SpellCollisionType.ENEMY, weak, collision));
		}
	}
}
