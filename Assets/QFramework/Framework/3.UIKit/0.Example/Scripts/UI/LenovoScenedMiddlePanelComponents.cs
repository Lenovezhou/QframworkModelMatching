/****************************************************************************
 * 2018.7 DOGE4
 ****************************************************************************/

namespace QFramework.Example
{
	using UnityEngine;
	using UnityEngine.UI;
	using QFramework;

	public partial class LenovoScenedMiddlePanel	{
		public const string NAME = "LenovoScenedMiddlePanel";
		[SerializeField] public ScrollRect ScrollView;
		[SerializeField] public ScrollRect ScrollView1;
		[SerializeField] public ScrollRect ScrollView2;
		[SerializeField] public Image LenovoTopPanel;

		protected override void ClearUIComponents()
		{
			ScrollView = null;
			ScrollView1 = null;
			ScrollView2 = null;
			LenovoTopPanel = null;
		}
	}
}
