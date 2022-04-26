using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.tower.targets;
namespace wtd.tower.editor.targetEditors
{
	public class LocationTTargetEditor : TowerTargetEditor
	{
		public LocationTTargetEditor(Tower tower, TowerTarget editorTarget) : base(tower, editorTarget)
		{

		}

		public override bool CheckTarget(Vector2 position)
		{
			return ((Vector2)Tower.GetPosition() - (Vector2)position).sqrMagnitude <= Tower.RangeSqr;
		}

		protected override void DoAcceptTarget(Vector2 position)
		{
			((LocationTTarget)EditorTarget).SetPosition(position);
		}
	}
}
