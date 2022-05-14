using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.tower.editor
{
	public abstract class TowerTargetEditor
	{

		public Tower Tower;

		public TowerTarget EditorTarget;

		public TowerTargetEditor(Tower tower, TowerTarget editorTarget)
		{
			this.Tower = tower;
			this.EditorTarget = editorTarget;
		}

		public virtual void CancelEditor(Vector3 position)
		{
			if (Tower.TargetAdapter.Target != EditorTarget)
			{
				GameObject.Destroy(EditorTarget);
			}
		}

		public void AcceptTarget(Vector3 position)
		{
			Tower.TargetAdapter.ChangeTarget(EditorTarget);
		}

		protected abstract void DoAcceptTarget(Vector3 position);

		public abstract bool CheckTarget(Vector3 position);


	}
}
