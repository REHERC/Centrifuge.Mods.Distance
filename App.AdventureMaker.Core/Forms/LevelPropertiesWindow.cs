using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using Eto.Drawing;
using System;
using App.AdventureMaker.Core.Controls;
using Distance.AdventureMaker.Common.Enums;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models.Resources;

namespace App.AdventureMaker.Core.Forms
{
	public class LevelPropertiesWindow : Dialog<CampaignLevel>
	{
		private CampaignLevel Data { get; }

		private readonly IEditor<CampaignFile> editor;

		private readonly ExtendedTabControl tabs;
		private readonly DynamicLayout generalProperties;
		private readonly DynamicLayout loadingScreenProperties;
		private readonly DynamicLayout introSequenceProperties;
		private readonly DynamicLayout gameplayProperties;

		private readonly GuidLabel propLevelGuid;
		private readonly TextBox propLevelName;
		private readonly ResourceSelector propLevelFile;
		private readonly ResourceSelector propLoadingBackground;
		private readonly StretchedImageBox propLoadingBackgroundPreview;
		private readonly BooleanSelector propLoadingOverwriteText;
		private readonly TextBox propLoadingText;
		private readonly ResourceSelector propLoadingBackgroundIcon;
		private readonly StretchedImageBox propLoadingBackgroundIconPreview;
		private readonly EnumDropDown<LevelTransitionType> propIntroType;
		private readonly TextBox propIntroLine1;
		private readonly TextBox propIntroLine2;

		public LevelPropertiesWindow(IEditor<CampaignFile> editor, CampaignLevel level)
		{
			this.editor = editor;

			Data = level.CloneObject()
			?? new CampaignLevel()
			{
				Guid = Guid.NewGuid().ToString()
			};

			Title = $"{(Equals(level, null) ? "Add" : "Edit")} level";
			Size = MinimumSize = new Size(600, 400);
			Padding = new Padding(8);

			Resizable = true;

			Content = new StackLayout()
			{
				Style = "vertical",
				Spacing = 8,

				Items =
				{
					new StackLayoutItem(tabs = new ExtendedTabControl(), true),
					new StackLayoutItem(new StackLayout()
					{
						Style = "horizontal",
						Spacing = 8,
						Padding = new Padding(0, 8, 0, 0),

						Items =
						{
							null,
							(DefaultButton = new Button(OnConfirm)
							{
								Text = "OK",
								Image = Resources.GetIcon("CheckGreen.ico", 16)
							}),
							(AbortButton = new Button(OnCancel)
							{
								Text = "Cancel",
								Image = Resources.GetIcon("CloseRed.ico", 16)
							})
						}
					}, false)
				}
			};

			#region General Tab
			tabs.AddPage("General", generalProperties = new DynamicLayout());

			generalProperties.BeginScrollable(BorderType.None);
			generalProperties.BeginVertical();

			generalProperties.AddRow("Unique ID", propLevelGuid = new GuidLabel());
			generalProperties.AddRow("Level file", propLevelFile = new ResourceSelector(editor, ResourceType.Level));
			generalProperties.AddRow("Level name", propLevelName = new TextBox());

			generalProperties.EndVertical();
			generalProperties.AddSpace();
			generalProperties.EndScrollable();
			#endregion

			//tabs.AddPage("Gameplay", gameplayProperties = new DynamicLayout());

			#region Loading Screen Tab
			tabs.AddPage("Loading screen", loadingScreenProperties = new DynamicLayout());

			loadingScreenProperties.BeginScrollable(BorderType.None);
			loadingScreenProperties.BeginVertical();

			loadingScreenProperties.AddRow("Background image", propLoadingBackground = new ResourceSelector(editor, ResourceType.Texture));

			loadingScreenProperties.AddRow(string.Empty, new StackLayout()
			{
				Style = "no-padding horizontal",

				Items =
				{
					(propLoadingBackgroundPreview = new StretchedImageBox()
					{
						Width = 160,
						Height = 90
					}),
					null
				}
			});

			loadingScreenProperties.AddRow("Overwrite loading text", propLoadingOverwriteText = new BooleanSelector());
			loadingScreenProperties.AddRow("Loading text", propLoadingText = new TextBox());
			loadingScreenProperties.AddRow("Progress indicator icon", propLoadingBackgroundIcon = new ResourceSelector(editor, ResourceType.Texture));
			loadingScreenProperties.AddRow(string.Empty, new StackLayout()
			{
				Style = "no-padding horizontal",

				Items =
				{
					(propLoadingBackgroundIconPreview = new StretchedImageBox()
					{
						Width = 64,
						Height = 64
					}),
					null
				}
			});

			loadingScreenProperties.EndVertical();
			loadingScreenProperties.AddSpace();
			loadingScreenProperties.EndScrollable();
			#endregion

			#region Intro Sequence Tab
			tabs.AddPage("Intro sequence", introSequenceProperties = new DynamicLayout());

			introSequenceProperties.BeginScrollable(BorderType.None);
			introSequenceProperties.BeginVertical();

			// Set whether or not to show the intro, the intro style and text values

			introSequenceProperties.AddRow("Intro type", propIntroType = new EnumDropDown<LevelTransitionType>());
			introSequenceProperties.AddRow("First line", propIntroLine1 = new TextBox());
			introSequenceProperties.AddRow("Second line", propIntroLine2 = new TextBox());

			introSequenceProperties.EndVertical();
			introSequenceProperties.AddSpace();
			introSequenceProperties.EndScrollable();
			#endregion

			#region Event Subscribing
			propLoadingOverwriteText.ValueChanged += OnOverwriteLoadingTextChanged;
			propIntroType.SelectedValueChanged += OnTransitionTypeChanged;
			propLoadingBackground.ResourceSelected += OnBackgroundSelected;
			propLoadingBackgroundIcon.ResourceSelected += OnLoadingIconSelected;
			#endregion

			LoadData(Data);
		}

		private void OnLoadingIconSelected(object sender, EventArgs e)
		{
			propLoadingBackgroundIconPreview.Image = (propLoadingBackgroundIcon.Resource as CampaignResource.Texture)?.AsImage(editor);
		}

		private void OnBackgroundSelected(object sender, EventArgs e)
		{
			propLoadingBackgroundPreview.Image = (propLoadingBackground.Resource as CampaignResource.Texture)?.AsImage(editor);
		}

		private void OnOverwriteLoadingTextChanged(object sender, EventArgs e)
		{
			propLoadingText.Enabled = propLoadingOverwriteText.Value;
		}

		private void OnTransitionTypeChanged(object sender, EventArgs e)
		{
			switch (propIntroType.SelectedValue)
			{
				case LevelTransitionType.None:
					propIntroLine1.Enabled = false;
					propIntroLine2.Enabled = false;
					break;
				case LevelTransitionType.Default:
					propIntroLine1.Enabled = true;
					propIntroLine2.Enabled = true;
					break;
				case LevelTransitionType.LostToEchoes:
				case LevelTransitionType.EarlyAccess:
					propIntroLine1.Enabled = true;
					propIntroLine2.Enabled = false;
					break;
			}
		}

		private void LoadData(CampaignLevel level)
		{
			propLevelGuid.Text = level.Guid;
			propLevelFile.Resource = editor.Document.GetResource(level.ResourceId, ResourceType.Level);
			propLevelName.Text = level.Name;

			propLoadingBackground.Resource = editor.Document.GetResource(level.LoadingBackground, ResourceType.Texture);
			propLoadingBackgroundIcon.Resource = editor.Document.GetResource(level.LoadingBackgroundIcon, ResourceType.Texture);
			propLoadingText.Text = level.LoadingText;
			propLoadingOverwriteText.Value = level.OverrideLoadingText;
			OnOverwriteLoadingTextChanged(propLoadingOverwriteText, EventArgs.Empty);

			propIntroType.SelectedValue = level.Transition;
			propIntroLine1.Text = level.Title;
			propIntroLine2.Text = level.TitleSmall;
		}

		private void SaveData(CampaignLevel level)
		{
			level.Guid = propLevelGuid.Text;
			level.ResourceId = propLevelFile.Resource?.guid;
			level.Name = propLevelName.Text;

			level.LoadingBackground = propLoadingBackground.Resource?.guid;
			level.LoadingBackgroundIcon = propLoadingBackgroundIcon.Resource?.guid;

			level.OverrideLoadingText = propLoadingOverwriteText.Value;
			level.LoadingText = propLoadingText.Text;

			level.Transition = propIntroType.SelectedValue;
			level.Title = propIntroLine1.Text;
			level.TitleSmall = propIntroLine2.Text;
		}

		private void OnCancel(object sender, EventArgs e)
		{
			Close(null);
		}

		private void OnConfirm(object sender, EventArgs e)
		{
			if (propLevelFile.Resource is null)
			{
				MessageBox.Show("You must specify a level file!", Constants.DIALOG_CAPTION_MISSING_FILE);
				return;
			}

			SaveData(Data);
			Close(Data);
		}
	}
}
