using UnityEngine;
using UnityEngine.UI;
using wtd.wand;
namespace wtd.ui.spell
{
	public class UIWand : MonoBehaviour
	{


		[field: SerializeField]
		public Wand RealWand { get; private set; }

		[field: SerializeField]
		public UISpellContainer SpellContainer { get; private set; }

		[SerializeField]
		private Image wandRenderer;

		public void Setup(Wand wand)
		{
			this.RealWand = wand;
			SpellContainer.Setup(RealWand.GetSpellContainer());
			RefreshImage();
		}

		void RefreshImage()
		{
			Wand.WandImage wandImage = RealWand.Image;
			wandRenderer.sprite = wandImage.sprite;
			wandRenderer.material = wandImage.material;
			wandRenderer.color = wandImage.color;
		}
	}
}
