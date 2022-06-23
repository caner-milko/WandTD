using System.Collections.Generic;
using UnityEngine;
using wtd.effect;
using wtd.spell.targets;
using wtd.stat;
namespace wtd.spell
{
	/// <summary>
	/// Every spell <see cref="GameObject"/> should be a <see cref="CastedSpell"/><br/>
	/// Every casted spell contains a <see cref="SingleSpellGroup"/> in it, its target 
	/// </summary>
	public class CastedSpell : MonoBehaviour
	{


		public SingleSpellGroup SpellGroup { get; private set; }

		public Transform PassivesParent;
		public ISpellTarget Target { get; private set; }
		[field: SerializeField]
		private StatHolderComp statHolderComp;
		public StatHolder StatHolder => statHolderComp != null ? statHolderComp.RealHolder : null;
		[field: SerializeField]
		public EffectHolder EffectHolder { get; private set; }

		public bool Casted { get; private set; } = false;

		public ActiveSpell Active { get; private set; }
		public List<PassiveSpell> Passives { get; private set; } = new List<PassiveSpell>();

		public List<SpellTrigger> Triggers { get; private set; } = new List<SpellTrigger>();

		[field: ReadOnly, SerializeField,]
		public bool ToDestroy { get; private set; } = false;

		private bool didCastChild = false;

		public Transform MainMesh { get; private set; }

		private void Awake()
		{
			if (statHolderComp == null || !statHolderComp)
			{
				statHolderComp = GetComponent<StatHolderComp>();
			}
			if (EffectHolder == null || !EffectHolder)
			{
				EffectHolder = GetComponent<EffectHolder>();
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
			this.Target = target;
			this.SpellGroup = spellGroup;

			this.Active = (ActiveSpell)spellGroup.Active.CastSpell(spellGroup, this);
			name = "Casted: " + Active.spellName;
			if (spellGroup.Passives.Count > 0)
				name += " (";
			foreach (PassiveSpell passive in spellGroup.Passives)
			{
				this.Passives.Add((PassiveSpell)passive.CastSpell(spellGroup, this));
				name += passive.spellName + ", ";
			}
			if (spellGroup.Passives.Count > 0)
			{
				name = name[..^2];
				name += ")";
			}
			Active.Setup(this);
			foreach (PassiveSpell passive in Passives)
				passive.Setup(this);
		}

		public void Cast()
		{
			Active.Cast();
			foreach (PassiveSpell passive in Passives)
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
			Triggers.Add(realTrigger);
			realTrigger.Setup(this);
			return realTrigger;
		}


		public void HitTrigger(CollisionSpellTrigger.CollisionSpellTriggerData triggerData)
		{
			if (!didCastChild)
			{
				ISpellTarget target;
				if (triggerData.isTrigger)
				{
					target = this.Target;
				}
				else
				{
					target = new DirectedSpellTarget(Vector3.Reflect(-triggerData.collision.relativeVelocity, triggerData.collision.GetContact(0).normal));
				}
				ShootChild(target);
			}
			if (triggerData.weak)
				return;
			bool shouldDestroy = Active.HitTrigger(triggerData);
			foreach (PassiveSpell passive in Passives)
			{
				shouldDestroy = shouldDestroy && passive.HitTrigger(triggerData);
			}
			if (shouldDestroy)
			{
				ToDestroy = shouldDestroy;
			}
		}

		public void TimerTrigger(TimedSpellTrigger.TimedSpellTriggerData triggerData)
		{
			if (triggerData.weak)
			{
				ShootChild(this.Target);
				return;
			}
			bool shouldDestroy = Active.TimerTrigger(triggerData);
			foreach (PassiveSpell passive in Passives)
			{
				shouldDestroy = shouldDestroy && passive.TimerTrigger(triggerData);
			}
			if (shouldDestroy)
			{
				ToDestroy = shouldDestroy;
			}
		}

		public void SetMainMesh(Transform mainMesh)
		{
			if (this.MainMesh != null)
			{
				Debug.LogError("Cannot set main mesh twice in casted spell.");
				return;
			}
			this.MainMesh = mainMesh;
		}

		protected void ShootChild(ISpellTarget target)
		{
			if (didCastChild)
				return;
			this.SpellGroup.CastChild(MainMesh.position + target.GetDirection(MainMesh.position) * 0.1f, target);
			didCastChild = true;
		}

		private void LateUpdate()
		{
			if (ToDestroy)
			{
				Active.Remove();
				foreach (PassiveSpell passive in Passives)
					passive.Remove();
				ShootChild(this.Target);
				Destroy(gameObject);
			}
		}

	}
}