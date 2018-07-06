/****************************************************************************
 * 2018.7 DOGE4
 ****************************************************************************/

namespace QFramework.Example
{
	using UnityEngine;
	using UnityEngine.UI;
	using QFramework;

	public partial class UICreateIllnessPopup	{
		public const string NAME = "UICreateIllnessPopup";
		[SerializeField] public Text WarmingText;
		[SerializeField] public InputField TitleInputField;
        [SerializeField] public InputField descriptioninputfiled;
        [SerializeField] public InputField noteinputfiled;
        [SerializeField] public Dropdown injury_positionDropdown;
		[SerializeField] public Dropdown directionDropdown;
		[SerializeField] public Dropdown protector_shapeDropdown;
		[SerializeField] public Button CommitButton;
		[SerializeField] public Button ShowFileButton;
		[SerializeField] public Button CancleButton;

		protected override void ClearUIComponents()
		{
			WarmingText = null;
			TitleInputField = null;
			injury_positionDropdown = null;
			directionDropdown = null;
			protector_shapeDropdown = null;
			descriptioninputfiled = null;
			noteinputfiled = null;
			CommitButton = null;
			ShowFileButton = null;
			CancleButton = null;
		}
	}
}
