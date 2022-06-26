using System.Collections.Generic;
using UnityEngine;
using wtd.effect;
using wtd.spell;
using wtd.spell.targets;
using wtd.ui.spell;
using wtd.wand;
namespace wtd
{
	public class PlayerTest : MonoBehaviour, ISpellTarget, ISpellCaster
	{

		[field: SerializeField]
		public float EditableDistance { get; private set; } = 1f;

		public float EditableDistanceSqr => EditableDistance * EditableDistance;

		public WandContainer wandContainer;

		//TODO add wand scrolling change later
		public Wand SelectedWand => wandContainer.First;

		//test variables

		public EffectHolder holder;

		public Effect effectPrefab;

		[SerializeField]
		private List<Spell> spells;

		void Start()
		{
			if (SelectedWand != null)
				foreach (Spell spell in spells)
					SelectedWand.AddSpell(spell);
		}

		void Update()
		{
			if (WandEditorManager.instance.IsEditing)
				return;
			if (Input.GetKeyDown(KeyCode.K))
			{
				holder.AddEffect(effectPrefab, true);
			}

			if (Input.GetKey(KeyCode.Mouse0) && SelectedWand != null)
			{
				//output casted spell list
				Physics.Raycast(CameraManager.Instance.MouseRay, out RaycastHit hit);
				Vector3 mouseDir = hit.point - transform.position;
				mouseDir.y = 0;
				SelectedWand.Shoot(new DirectedSpellTarget(mouseDir), out _);
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
			return SelectedWand == null ? null : SelectedWand.NextSpell();
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
			return SelectedWand.GetSpellContainer();
		}
	}
}