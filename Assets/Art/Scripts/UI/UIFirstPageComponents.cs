/****************************************************************************
 * 2018.7 DOGE4
 ****************************************************************************/

namespace QFramework.Example
{
	using UnityEngine;
	using UnityEngine.UI;
	using QFramework;

	public partial class UIFirstPage	{
		public const string NAME = "UIFirstPage";
		[SerializeField] public Button AddPlayerButton;
		[SerializeField] public ScrollRect PlayerListScrollView;
		[SerializeField] public Button Item;

		protected override void ClearUIComponents()
		{
			AddPlayerButton = null;
			PlayerListScrollView = null;
			Item = null;
		}
	}
}
