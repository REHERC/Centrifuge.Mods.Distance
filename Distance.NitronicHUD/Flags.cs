using Events.Game;
using Events.GameMode;
using Events.Level;
using Events.LevelEditor;
using Events.RaceEnd;
using System.Linq;

namespace Distance.NitronicHUD
{
	public static class Flags
	{
		public static bool IsModeStarted { get; private set; }

		public static bool IsPaused { get; private set; }

		public static bool IsMenuOpen => G.Sys.MenuPanelManager_?.panelStack_?.Count > 0;

		public static bool IsLoading => G.Sys.GameManager_.BlackFade_.currentState_ != BlackFadeLogic.FadeState.Idle && !G.Sys.GameManager_.IsLevelLoaded_;

		public static bool IsReplayMode => G.Sys.ReplayManager_.IsReplayMode_;

		public static GameModeID[] NonPlayModeIDS { get; }

		public static bool CurrentModeIsPlayable => !NonPlayModeIDS.Contains(G.Sys.GameManager_.ModeID_);

		static Flags()
		{
			NonPlayModeIDS = new GameModeID[]
			{
				GameModeID.Count,
				GameModeID.MainMenu,
				GameModeID.None,
			};
		}

		public static bool CanDisplayHudElements
		{
			get
			{
				return IsModeStarted && ! IsLoading && CurrentModeIsPlayable && !IsPaused && !IsMenuOpen && !IsReplayMode;
			}
		}

		internal static void SubscribeEvents()
		{
			PauseToggled.Subscribe(OnGamePauseToggled);
			ModeInitialized.Subscribe(OnGameModeInitialized);
			ModeStarted.Subscribe(OnGameModeModeStarted);
			LocalCarHitFinish.Subscribe(OnRaceEndLocalCarHitFinish);
			EnterPlayMode.Subscribe(OnLevelEditorEnterPlayMode);
			EnterEditorMode.Subscribe(OnLevelEditorEnterEditorMode);
			UninitializeOptimizations.Subscribe(OnLevelUninitializeOptimizations);
		}

		private static void OnLevelUninitializeOptimizations(UninitializeOptimizations.Data data)
		{
			IsModeStarted = false;
		}

		private static void OnGameModeModeStarted(ModeStarted.Data data)
		{
			IsPaused = false;
			IsModeStarted = true;
		}

		private static void OnLevelEditorEnterEditorMode(EnterEditorMode.Data data)
		{
			IsPaused = false;
			IsModeStarted = false;
		}

		private static void OnLevelEditorEnterPlayMode(EnterPlayMode.Data data)
		{
			IsModeStarted = true;
		}

		private static void OnRaceEndLocalCarHitFinish(LocalCarHitFinish.Data data)
		{
			IsModeStarted = false;
		}

		private static void OnGameModeInitialized(ModeInitialized.Data data)
		{
			IsPaused = false;
		}

		private static void OnGamePauseToggled(PauseToggled.Data data)
		{
			IsPaused = data.paused_;
		}
	}
}