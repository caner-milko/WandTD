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
		public bool IsRecharging { get; private set; } = false;
		public bool CanShoot => curCastDelay <= 0.0f && !IsRecharging;

		[field: SerializeField]
		// Owner of this wand
		public WandContainer Container { get; private set; }

		// Spells in this wand's slot
		public SpellContainer spells;

		// Spells yet to be cast since last recharge
		public Queue<CasterSpell> remSpells = new();

		public SpellGroupBuilder curBuilder;

		public CastedSpell castedPrefab;

		[field: SerializeField]
		public StatHolderComp StatHolderC { get; private set; }

		[SerializeField, AutoCopyStat(StatNames.WAND_MANA)]
		private Stat mana = new(StatNames.WAND_MANA, null, 150);
		[SerializeField, AutoCopyStat(StatNames.WAND_MANA_RECHARGE)]
		private Stat manaRecharge = new(StatNames.WAND_MANA_RECHARGE, null, 20);
		[SerializeField, AutoCopyStat(StatNames.WAND_CAPACITY)]
		private Stat capacity = new(StatNames.WAND_CAPACITY, null, 4);
		[SerializeField, AutoCopyStat(StatNames.WAND_RECHARGE_DELAY)]
		private Stat rechargeDelay = new(StatNames.WAND_RECHARGE_DELAY, null, 0.5f);
		[SerializeField, AutoCopyStat(StatNames.WAND_CAST_DELAY)]
		private Stat castDelay = new(StatNames.WAND_CAST_DELAY, null, 0.2f);
		[SerializeField, AutoCopyStat(StatNames.WAND_CAST_COUNT)]
		private Stat castCount = new(StatNames.WAND_CAST_COUNT, null, 1);

		[System.Serializable]
		public struct WandImage
		{
			public Sprite sprite;
			public Material material;
			public Color color;
		}

		[field: SerializeField]
		public WandImage Image { get; private set; }

		private void Awake()
		{
			SetupStats();
			remainingMana = mana.Value;
			spells = new SpellContainer(this, (int)capacity.Value);
			spells.ContainerEdited.AddListener(StartRecharge);
		}

		public void ChangeOwner(WandContainer container)
		{
			this.Container = container;
		}

		private void Start()
		{
			RechargeSpells();
		}

		private void SetupStats()
		{
			if (StatHolderC == null || !StatHolderC)
			{
				StatHolderC = GetComponent<StatHolderComp>();
				if (StatHolderC == null)
					StatHolderC = gameObject.AddComponent<StatHolderComp>();
			}

			((IStatUser)this).SetupStats();
		}

		private void Update()
		{
			remainingMana = Mathf.Min(remainingMana + manaRecharge.Value * Time.deltaTime, mana.Value);
			if (IsRecharging)
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
			if (CanShoot)
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
			if (PrepareSpellGroup(new List<PassiveSpell>(), out SpellGroupBase group))
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
			if (!CanShoot)
			{
				group = null;
				return false;
			}
			SpellGroupBuilder builder = new(this, castedPrefab, Mathf.FloorToInt(castCount.Value), passives);

			curBuilder = builder;

			group = builder.Build();
			if (group != null)
			{
				curCastDelay = castDelay.Value + group.GetCastDelay();
				curRechargeDelay += group.GetRechargeDelay();
			}
			if (remSpells.Count == 0)
			{
				StartRecharge();
			}
			return group != null;
		}

		public void StartRecharge()
		{
			IsRecharging = true;
			curRechargeDelay += rechargeDelay.Value;
		}

		public string CasterType()
		{
			return "CT_wand";
		}

		public void RechargeSpells()
		{
			IsRecharging = false;
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
				if (nextSpell.Spell.mana <= this.remainingMana)
				{
					this.remainingMana -= nextSpell.Spell.mana;
					return nextSpell;
				}
			}
			return null;
		}


		public CasterSpell AddSpell(Spell spell)
		{
			CasterSpell casterSpell = new(spell, this);
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
			return Container.GetPosition();
		}

		public StatHolder GetStatHolder()
		{
			return StatHolderC.RealHolder;
		}

		public SpellContainer GetSpellContainer()
		{
			return spells;
		}
	}
}
