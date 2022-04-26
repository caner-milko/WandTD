using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands;
using wtd.wands.targets;
using wtd.tower.editor.targetEditors;
using wtd.tower.editor;

namespace wtd.tower.targets
{
	public class LocationTTarget : EditableTowerTarget
	{

		[SerializeField]
		private Vector3 _position;
		public Vector3 Position => local ? Owner.GetPosition() + _position : _position;

		public bool hasPosition = false;
		public bool local = true;

		public override bool GetTarget(out ISpellTarget target)
		{
			target = new StaticSpellTarget(Position);
			if (!hasPosition || (Position - Owner.GetPosition()).sqrMagnitude > Owner.RangeSqr)
			{
				return false;
			}

			return true;
		}

		public override TowerTargetEditor GetTTargetEditor()
		{
			return new LocationTTargetEditor(TargetAdapter.Tower, this);
		}

		public void SetPosition(Vector3 position)
		{
			if (local)
				_position = position - Owner.GetPosition();
			else
				_position = position;
		}

	}
}
