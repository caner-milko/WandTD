using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.effect;
using wtd.stat;
namespace wtd.enemy
{
	[RequireComponent(typeof(PathFollower), typeof(EffectHolder), typeof(StatHolderComp))]
	public class Enemy : MonoBehaviour, IDamageable, IStatUser
	{
		[field: SerializeField, ReadOnly]
		public float health { get; private set; }

		[SerializeField, AutoCopyStat(StatNames.MAX_HEALTH)]
		private Stat maxHealth = new Stat(StatNames.MAX_HEALTH, 20.0f);

		private StatHolderComp statHolderComp;

		public float GetHealth()
		{
			return health;
		}

		public float InflictDamage(StatHolder damageStats)
		{
			float damage = damageStats.GetStatValue(StatNames.DAMAGE);
			this.health -= damage;
			return damage;
		}

		public float Heal(StatHolder healStats)
		{
			float heal = healStats.GetStatValue(StatNames.HEAL);
			this.health += heal;
			return heal;
		}

		private void Awake()
		{
			statHolderComp = GetComponent<StatHolderComp>();
			StatUtils.SetupStats(this);
		}

		private void Start()
		{
			this.health = maxHealth.Value;
		}

		public StatHolder GetStatHolder()
		{
			return statHolderComp.statHolder;
		}
	}
}
