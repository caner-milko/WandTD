using UnityEngine;
using wtd.tower.targets;
namespace wtd.tower.editor.targetEditors
{
	public class LocationTTargetEditor : TowerTargetEditor
	{
		public LocationTTargetEditor(Tower tower, TowerTarget editorTarget) : base(tower, editorTarget)
		{

		}

		public override bool CheckTarget(Vector3 position)
		{
			return (Tower.GetPosition() - position).sqrMagnitude <= Tower.RangeSqr;
		}

		protected override void DoAcceptTarget(Vector3 position)
		{
			((LocationTTarget)EditorTarget).SetPosition(position);
		}
	}
}
