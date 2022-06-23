using UnityEditor;
using UnityEngine;

namespace wtd.tower
{
	public class TowerTargetAdapter : MonoBehaviour
	{

		[field: SerializeField]
		public Tower Tower { get; private set; }

		[field: SerializeField]
		public TowerTarget Target { get; private set; }


		private void Start()
		{
			Setup();
		}

		private void OnValidate()
		{
			Setup();
		}

		public void Setup()
		{
			if (Tower != null)
				Tower.TargetAdapter = this;
			if (Target != null)
				Target.TargetAdapter = this;
		}

		public void ChangeTarget(TowerTarget newTarget)
		{
			if (Target != null)
			{
				GameObject.Destroy(Target);
			}
			newTarget.TargetAdapter = this;
			Target = newTarget;
		}

		private void OnDrawGizmosSelected()
		{
			if (Selection.activeGameObject != transform.gameObject) return;
			Target.DrawGizmos();
		}


	}
}
