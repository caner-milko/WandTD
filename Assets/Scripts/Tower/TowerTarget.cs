using UnityEditor;
using UnityEngine;
using wtd.spell;

namespace wtd.tower
{
	public abstract class TowerTarget : MonoBehaviour
	{
		[field: SerializeField]
		public TowerTargetAdapter TargetAdapter { get; set; }
		public Tower Owner => TargetAdapter != null ? TargetAdapter.Tower : null;


		/// <summary>
		/// </summary>
		/// <returns><see cref="ISpellTarget"/> selected as target</returns>
		public abstract bool GetTarget(out ISpellTarget target);

		public virtual void DrawGizmos()
		{
			if (Owner == null)
				return;
			if (GetTarget(out ISpellTarget pos))
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(pos.GetPosition(), 0.1f);
			}
			else
			{
				if (pos == null)
					return;
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(pos.GetPosition(), 0.1f);
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (Selection.activeGameObject != gameObject) return;
			DrawGizmos();
		}
	}
}
