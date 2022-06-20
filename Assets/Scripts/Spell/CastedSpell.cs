using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.stat;
using wtd.effect;
using wtd.spell.targets;
namespace wtd.spell
{
	/// <summary>
	/// Every spell <see cref="GameObject"/> should be a <see cref="CastedSpell"/><br/>
	/// Every casted spell contains a <see cref="SingleSpellGroup"/> in it, its target 
	/// </summary>
	public class CastedSpell : MonoBehaviour
	{


		public SingleSpellGroup spellGroup { get; private set; }

		public Transform PassivesParent;
		public ISpellTarget target { get; private set; }
		[field: SerializeField]
		private StatHolderComp statHolderComp;
		public StatHolder statHolder => statHolderComp != null ? statHolderComp.statHolder : null;
		[field: SerializeField]
		public EffectHolder effectHolder { get; private set; }

		public bool casted { get; private set; } = false;

		public ActiveSpell active { get; private set; }
		public List<PassiveSpell> passives { get; private set; } = new List<PassiveSpell>();

		public List<SpellTrigger> triggers { get; private set; } = new List<SpellTrigger>();

		[field: ReadOnly, SerializeField,]
		public bool toDestroy { get; private set; } = false;

		private bool didCastChild = false;

		public Transform mainMesh { get; private set; }

		private void Awake()
		{
			if (statHolderComp == null || !statHolderComp)
			{
				statHolderComp = GetComponent<StatHolderComp>();
			}
			if (effectHolder == null || !effectHolder)
			{
				effectHolder = GetComponent<EffectHolder>();
			}
			if (PassivesParent == null || !PassivesParent)
			{
				PassivesParent = new GameObject("Passives").transform;
				PassivesParent.parent = transform;
			}
		}

		public void Init(Vector3 position, ISpellTarget target, SingleSpellGroup spellGroup)
		{
			this.transform.position = position;
			this.target = target;
			this.spellGroup = spellGroup;

			this.active = (ActiveSpell)spellGroup.active.CastSpell(spellGroup, this);
			name = "Casted: " + active.spellName;
			if (spellGroup.passives.Count > 0)
				name += " (";
			foreach (PassiveSpell passive in spellGroup.passives)
			{
				this.passives.Add((PassiveSpell)passive.CastSpell(spellGroup, this));
				name += passive.spellName + ", ";
			}
			if (spellGroup.passives.Count > 0)
			{
				name = name.Substring(0, name.Length - 2);
				name += ")";
			}
			active.Setup(this);
			foreach (PassiveSpell passive in passives)
				passive.Setup(this);
		}

		public void Cast()
		{
			active.Cast();
			foreach (PassiveSpell passive in passives)
			{
				passive.Cast();
			}
		}

		public SpellTrigger AddTrigger(SpellTrigger trigger, bool instantiate)
		{
			SpellTrigger realTrigger = trigger;
			if (instantiate)
			{
				realTrigger = GameObject.Instantiate<SpellTrigger>(trigger, transform);
			}
			triggers.Add(realTrigger);
			realTrigger.Setup(this);
			return realTrigger;
		}


		public void HitTrigger(CollisionSpellTrigger.CollisionSpellTriggerData triggerData)
		{
			if (!didCastChild)
			{
				ISpellTarget target = null;
				if (triggerData.isTrigger)
				{
					target = this.target;
				}
				else
				{
					target = new DirectedSpellTarget(Vector3.Reflect(-triggerData.collision.relativeVelocity, triggerData.collision.GetContact(0).normal));
				}
				ShootChild(target);
			}
			if (triggerData.weak)
				return;
			bool shouldDestroy = active.HitTrigger(triggerData);
			foreach (PassiveSpell passive in passives)
			{
				shouldDestroy = shouldDestroy && passive.HitTrigger(triggerData);
			}
			if (shouldDestroy)
			{
				toDestroy = shouldDestroy;
			}
		}

		public void TimerTrigger(TimedSpellTrigger.TimedSpellTriggerData triggerData)
		{
			if (triggerData.weak)
			{
				ShootChild(this.target);
				return;
			}
			bool shouldDestroy = active.TimerTrigger(triggerData);
			foreach (PassiveSpell passive in passives)
			{
				shouldDestroy = shouldDestroy && passive.TimerTrigger(triggerData);
			}
			if (shouldDestroy)
			{
				toDestroy = shouldDestroy;
			}
		}

		public void SetMainMesh(Transform mainMesh)
		{
			if (this.mainMesh != null)
			{
				Debug.LogError("Cannot set main mesh twice in casted spell.");
				return;
			}
			this.mainMesh = mainMesh;
		}

		protected void ShootChild(ISpellTarget target)
		{
			if (didCastChild)
				return;
			this.spellGroup.CastChild(mainMesh.position + target.GetDirection(mainMesh.position) * 0.1f, target);
			didCastChild = true;
		}

		private void LateUpdate()
		{
			if (toDestroy)
			{
				active.Remove();
				foreach (PassiveSpell passive in passives)
					passive.Remove();
				ShootChild(this.target);
				Destroy(gameObject);
			}
		}

	}
}