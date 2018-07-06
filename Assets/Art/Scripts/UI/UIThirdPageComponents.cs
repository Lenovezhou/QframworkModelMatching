/****************************************************************************
 * 2018.7 DOGE4
 ****************************************************************************/

namespace QFramework.Example
{
	using UnityEngine;
	using UnityEngine.UI;
	using QFramework;

	public partial class UIThirdPage
    {
		public const string NAME = "UIThirdPage";
		[SerializeField] public Button MainmenuButton;
		[SerializeField] public Button ResetRotationBut;
		[SerializeField] public Button MatchingpointButton;
		[SerializeField] public Button AvoidvacancyButton;
		[SerializeField] public Button DecorativepatternButton;
		[SerializeField] public Button BandageButton;
		[SerializeField] public Button OtherButton;
		[SerializeField] public Button UserGuide;
		[SerializeField] public GameObject InfoPanel;
		[SerializeField] public GameObject MainMenuPanel;
		[SerializeField] public Text titletext;
		[SerializeField] public GameObject MatchingpointPanel;
		[SerializeField] public GameObject AvoidvacancyPanel;
		[SerializeField] public GameObject UserGuidePanel;
		[SerializeField] public Button CommitButton;
		[SerializeField] public GameObject DecorativepatternPanel;
		[SerializeField] public GameObject BandagePanel;
		[SerializeField] public GameObject OtherPanel;
		[SerializeField] public GameObject ResetNotice;
		[SerializeField] public Button UpButton;
		[SerializeField] public Button LeftButton;
		[SerializeField] public Button ForwardButton;
		[SerializeField] public Button RightButton;
		[SerializeField] public Button BackButton;
		[SerializeField] public Button DownButton;
		[SerializeField] public Toggle MainviewmenuToggle;
		[SerializeField] public GameObject MainviewmenuPanel;
		[SerializeField] public RawImage CoordinateSystemRawImage;

		protected override void ClearUIComponents()
		{
            Log.I("UIThirdPage----->>>>>>> ClearUIComponents");
			MainmenuButton = null;
			ResetRotationBut = null;
			MatchingpointButton = null;
			AvoidvacancyButton = null;
			DecorativepatternButton = null;
			BandageButton = null;
			OtherButton = null;
			UserGuide = null;
			InfoPanel = null;
			MainMenuPanel = null;
			titletext = null;
			MatchingpointPanel = null;
			AvoidvacancyPanel = null;
			UserGuidePanel = null;
			CommitButton = null;
			DecorativepatternPanel = null;
			BandagePanel = null;
			OtherPanel = null;
			ResetNotice = null;
			UpButton = null;
			LeftButton = null;
			ForwardButton = null;
			RightButton = null;
			BackButton = null;
			DownButton = null;
			MainviewmenuToggle = null;
			MainviewmenuPanel = null;
			CoordinateSystemRawImage = null;
		}
	}
}
