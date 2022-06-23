using System.Collections.Generic;
using UnityEngine;
using wtd.effect;
using wtd.spell;
using wtd.spell.targets;
using wtd.wand;

namespace wtd
{
	public class PlayerTest : MonoBehaviour, ISpellTarget, ISpellCaster
	{

		[field: SerializeField]
		public float EditableDistance { get; private set; } = 1f;

		public float EditableDistanceSqr => EditableDistance * EditableDistance;

		public Wand wand;

		//test variables

		public EffectHolder holder;

		public Effect effectPrefab;

		public List<Spell> spells;

		void Awake()
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

			if (Input.GetKey(KeyCode.Mouse0))
			{
				//output casted spell list
				Physics.Raycast(CameraManager.Instance.MouseRay, out RaycastHit hit);
				Vector3 mouseDir = hit.point - transform.position;
				mouseDir.y = 0;
				wand.Shoot(new DirectedSpellTarget(mouseDir), out _);
			}

		}

		private void FixedUpdate()
		{


			//basic movement
			Vector3 vel = new(0.0f, 0.0f, 0.0f);

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


			transform.Translate(5.0f * Time.deltaTime * vel);
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

		public SpellContainer GetSpellContainer()
		{
			return wand.GetSpellContainer();
		}
	}
}