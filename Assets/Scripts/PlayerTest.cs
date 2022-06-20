using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wand;
using wtd.spell;
using wtd.spell.targets;
using wtd.effect;

namespace wtd
{
	public class PlayerTest : MonoBehaviour, ISpellTarget, ISpellCaster
	{

		[field: SerializeField]
		public float EditableDistance { get; private set; } = 1f;

		public float EditableDistanceSqr => EditableDistance * EditableDistance;

		public Wand wand;

		//test variables

		// alternate between targeting types, used for test purposes
		int a;

		public EffectHolder holder;

		public Effect effectPrefab;

		public List<Spell> spells;

		void Start()
		{
			foreach (Spell spell in spells)
				wand.AddSpell(spell);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
			{
				holder.AddEffect(effectPrefab, true);
			}

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				//output casted spell list
				List<CastedSpell> casted;
				//a target is required to shoot a spell
				ISpellTarget target;
				if (a++ % 2 == 0)
				{
					target = new StaticSpellTarget(new Vector3(0.0f, 0.0f, 0.0f));
				}
				else
				{
					target = new FollowingSpellTarget(transform);
				}
				Physics.Raycast(CameraManager.instance.MouseRay, out RaycastHit hit);
				Vector3 mouseDir = hit.point - transform.position;
				mouseDir.y = 0;
				mouseDir = mouseDir.normalized;
				target = new DirectedSpellTarget(mouseDir);
				wand.Shoot(target, out casted);
			}

		}

		private void FixedUpdate()
		{


			//basic movement
			Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);

			if (Input.GetKey(KeyCode.W))
			{
				vel.z++;
				vel.x++;
			}

			if (Input.GetKey(KeyCode.D))
			{
				vel.z--;
				vel.x++;
			}

			if (Input.GetKey(KeyCode.S))
			{
				vel.z--;
				vel.x--;
			}

			if (Input.GetKey(KeyCode.A))
			{
				vel.x--;
				vel.z++;
			}

			vel.Normalize();


			transform.Translate(vel * 5.0f * Time.deltaTime);
		}

		public string CasterType()
		{
			return "SC_player";
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}

		public string GetTargetType()
		{
			return "ST_player";
		}

		public CasterSpell NextSpell()
		{
			return wand == null ? null : wand.NextSpell();
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
	}
}