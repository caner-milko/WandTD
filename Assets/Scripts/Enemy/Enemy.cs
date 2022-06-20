using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.effect;
using wtd.stat;
namespace wtd.enemy
{
	[RequireComponent(typeof(PathFollower), typeof(EffectHolder), typeof(StatHolderComp))]
	public class Enemy : MonoBehaviour, IDamageable
	{
		[field: SerializeField, ReadOnly]
		public float health { get; private set; }
		public Stat maxHealth;

		[SerializeField]
		private StatHolderComp statHolderComp;
		public StatHolder statHolder => statHolderComp == null ? null : statHolderComp.statHolder;

		public float GetHealth()
		{
			return health;
		}

		public float InflictDamage(StatHolder damageStats)
		{
			float damage = damageStats.GetStatValue("damage");
			this.health -= damage;
			return damage;
		}

		public float Heal(StatHolder healStats)
		{
			float heal = healStats.GetStatValue("heal");
			this.health += heal;
			return heal;
		}

		private void Awake()
		{
			statHolderComp = GetComponent<StatHolderComp>();

		}

		private void Start()
		{
			maxHealth = statHolder.GetStat("maxHealth", false, 10.0f);
			this.health = maxHealth.Value;
		}
	}
}
