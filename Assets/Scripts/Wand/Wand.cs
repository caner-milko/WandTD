using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.spell;
using wtd.stat;

namespace wtd.wand
{
	public class Wand : MonoBehaviour, ISpellCaster, ISpellTarget, IStatUser
	{

		// Time past since start of cast and recharge delay
		public double curCastDelay = 0.0f, curRechargeDelay = 0.0f;

		public float remainingMana = 0.0f;

		[field: SerializeField, ReadOnly]
		public bool isRecharging { get; private set; } = false;
		public bool canShoot => curCastDelay <= 0.0f && !isRecharging;

		// Owner of this wand
		public ISpellCaster owner;

		// Spells in this wand's slot
		public SpellContainer spells;

		// Spells yet to be cast since last recharge
		public Queue<CasterSpell> remSpells = new Queue<CasterSpell>();

		public SpellGroupBuilder curBuilder;

		public CastedSpell castedPrefab;

		[field: SerializeField]
		public StatHolderComp holder { get; private set; }

		[SerializeField, AutoCopyStat(StatNames.WAND_MANA)]
		private Stat mana = new Stat(StatNames.WAND_MANA, null, 150);
		[SerializeField, AutoCopyStat(StatNames.WAND_MANA_RECHARGE)]
		private Stat manaRecharge = new Stat(StatNames.WAND_MANA_RECHARGE, null, 20);
		[SerializeField, AutoCopyStat(StatNames.WAND_CAPACITY)]
		private Stat capacity = new Stat(StatNames.WAND_CAPACITY, null, 4);
		[SerializeField, AutoCopyStat(StatNames.WAND_RECHARGE_DELAY)]
		private Stat rechargeDelay = new Stat(StatNames.WAND_RECHARGE_DELAY, null, 0.5f);
		[SerializeField, AutoCopyStat(StatNames.WAND_CAST_DELAY)]
		private Stat castDelay = new Stat(StatNames.WAND_CAST_DELAY, null, 0.2f);
		[SerializeField, AutoCopyStat(StatNames.WAND_CAST_COUNT)]
		private Stat castCount = new Stat(StatNames.WAND_CAST_COUNT, null, 1);

		private void Awake()
		{
			SetupStats();
			remainingMana = mana.Value;
			spells = new SpellContainer(this, (int)capacity.Value);
		}

		private void Start()
		{
			RechargeSpells();
		}

		private void SetupStats()
		{
			if (holder == null || !holder)
			{
				holder = GetComponent<StatHolderComp>();
				if (holder == null)
					holder = gameObject.AddComponent<StatHolderComp>();
			}

			StatUtils.SetupStats(this);
		}

		private void Update()
		{
			remainingMana = Mathf.Min(remainingMana + manaRecharge.Value * Time.deltaTime, mana.Value);
			if (isRecharging)
			{
				curRechargeDelay -= Time.deltaTime;
				if (curRechargeDelay <= 0.0f)
				{
					RechargeSpells();
				}
			}
			if (curCastDelay > 0.0f)
			{
				curCastDelay -= Time.deltaTime;
			}
			if (canShoot)
			{
				curCastDelay = 0.0f;
			}
		}

		/// <summary>
		/// Casts spells towards given target.
		/// </summary>
		/// <param name="target">Target to casts spells to.</param>
		/// <param name="casted">List of casted spells.</param>
		/// <returns>true if at least one spell casted, false otherwise</returns>
		public bool Shoot(ISpellTarget target, out List<CastedSpell> casted)
		{
			SpellGroupBase group;
			if (PrepareSpellGroup(new List<PassiveSpell>(), out group))
			{
				casted = group.Cast(transform.position, target);
				return true;
			}
			casted = null;
			return false;
		}

		/// <summary>
		/// Prepares and returns group of spells to be cast. 
		/// </summary>
		/// <param name="passives">passives from outside</param>
		/// <param name="group">Group of spells that are to be cast.</param>
		/// <returns>true if there is at least one spell to be cast in returned group, false otherwise.</returns>
		public bool PrepareSpellGroup(List<PassiveSpell> passives, out SpellGroupBase group)
		{
			// TODO: If there are no remaining spells return false
			if (!canShoot)
			{
				group = null;
				return false;
			}
			SpellGroupBuilder builder = new SpellGroupBuilder(this, castedPrefab, Mathf.FloorToInt(castCount.Value));

			curBuilder = builder;

			group = builder.Build();
			if (group != null)
			{
				curCastDelay = castDelay.Value + group.GetCastDelay();
				curRechargeDelay += group.GetRechargeDelay();
			}
			if (remSpells.Count == 0)
			{
				isRecharging = true;
				curRechargeDelay += rechargeDelay.Value;
			}
			return group != null;
		}

		public string CasterType()
		{
			return "CT_wand";
		}

		public void RechargeSpells()
		{
			isRecharging = false;
			curRechargeDelay = 0.0f;
			remSpells = new Queue<CasterSpell>(spells);
		}

		/// <summary>
		/// Returns the next spell to be cast.
		/// </summary>
		/// <returns>Next spell to be cast.</returns>
		public CasterSpell NextSpell()
		{
			while (remSpells.Count > 0)
			{
				CasterSpell nextSpell = remSpells.Dequeue();
				if (nextSpell.spell.mana <= this.remainingMana)
				{
					this.remainingMana -= nextSpell.spell.mana;
					return nextSpell;
				}
			}
			return null;
		}


		public CasterSpell AddSpell(Spell spell)
		{
			CasterSpell casterSpell = new CasterSpell(spell, this);
			spells.AddSpell(casterSpell);
			return casterSpell;
		}

		public float DistanceToSqr(Vector3 from)
		{
			return (GetPosition() - from).sqrMagnitude;
		}

		public Vector3 GetDirection(Vector3 from)
		{
			return (GetPosition() - from).normalized;
		}

		public Vector3 GetVelocityVector(Vector3 from, float speed)
		{
			float maxSpeed = Mathf.Min(DistanceToSqr(from), speed * speed);

			return Mathf.Sqrt(maxSpeed) * GetDirection(from);
		}

		public string GetTargetType()
		{
			return "ST_wand";
		}

		public Vector3 GetPosition()
		{
			return owner.GetPosition();
		}

		public StatHolder GetStatHolder()
		{
			return holder.statHolder;
		}

		public SpellContainer GetSpellContainer()
		{
			return spells;
		}
	}
}
