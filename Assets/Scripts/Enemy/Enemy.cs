using UnityEngine;
using wtd.effect;
using wtd.stat;
namespace wtd.enemy
{
	[RequireComponent(typeof(PathFollower), typeof(EffectHolder), typeof(StatHolderComp))]
	public class Enemy : MonoBehaviour, IDamageable, IStatUser
	{
		[field: SerializeField, ReadOnly]
		public float Health { get; private set; }

		[SerializeField, AutoCopyStat(StatNames.MAX_HEALTH)]
		private Stat maxHealth = new(StatNames.MAX_HEALTH, 20.0f);

		private StatHolderComp statHolderComp;

		public float GetHealth()
		{
			return Health;
		}

		public float InflictDamage(StatHolder damageStats)
		{
			float damage = damageStats.GetStatValue(StatNames.DAMAGE);
			this.Health -= damage;
			return damage;
		}

		public float Heal(StatHolder healStats)
		{
			float heal = healStats.GetStatValue(StatNames.HEAL);
			this.Health += heal;
			return heal;
		}

		private void Awake()
		{
			statHolderComp = GetComponent<StatHolderComp>();
			((IStatUser)this).SetupStats();
		}

		private void Start()
		{
			this.Health = maxHealth.Value;
		}

		public StatHolder GetStatHolder()
		{
			return statHolderComp.RealHolder;
		}
	}
}
