using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.tower.editor;
namespace wtd.tower
{
	public abstract class EditableTowerTarget : TowerTarget
	{
		public abstract TowerTargetEditor GetTTargetEditor();
	}
}
