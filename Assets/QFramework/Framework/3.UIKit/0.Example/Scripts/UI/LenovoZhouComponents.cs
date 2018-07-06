/****************************************************************************
 * 2018.7 DOGE4
 ****************************************************************************/

namespace QFramework.Example
{
	using UnityEngine;
	using UnityEngine.UI;
	using QFramework;

	public partial class LenovoZhou	{
		public const string NAME = "LenovoZhou";
		[SerializeField] public Button Button1;
		[SerializeField] public Button Button2;
		[SerializeField] public Button Button3;

		protected override void ClearUIComponents()
		{
			Button1 = null;
			Button2 = null;
			Button3 = null;
		}
	}
}
