using Distance.TrackMusic.Models;
using System;
using System.Linq;
using UnityEngine;

namespace Distance.TrackMusic
{
    public class SoundPlayerLogic : MonoBehaviour
    {
        private Mod mod_;

        public void Awake()
        {
            mod_ = GetComponent<Mod>();
        }
        
        public void Update()
        {
            mod_.Variables.CachedMusicTrack.Update();
            mod_.Variables.CachedMusicChoice.Update();
            mod_.Variables.CachedMusicZoneData.Update();
        }

        public bool PlayTrack(string trackName, float fadeTimeMs = 2000f, bool force = false)
        {
            if (G.Sys.AudioManager_.CurrentMusicState_ == AudioManager.MusicState.CustomMusic)
            {
                return false;
            }

            if (!mod_.Enabled)
            {
                StopCustomMusic();
                return false;
            }

            if (trackName == null)
            {
                StopCustomMusic();
                return false;
            }

            var track = GetTrack(trackName);

            if (track == null || string.IsNullOrEmpty(track.FileLocation))
            {
                StopCustomMusic();
                return false;
            }

            if (!force && mod_.Variables.PlayingMusic && AudioManager.CurrentAudioFile_ != null && AudioManager.CurrentAudioFile_.FileName == track.FileLocation)
            {
                return true;
            }

            G.Sys.AudioManager_.perLevelMusicOverride_ = false;

            var failed = false;

            void local_MusicSegmentEnd(Events.Audio.MusicSegmentEnd.Data data)
            {
                failed = true;
            }

            Events.Audio.MusicSegmentEnd.Subscribe(local_MusicSegmentEnd);
            
            try
            {
                G.Sys.AudioManager_.PlayMP3(track.FileLocation, mod_.Variables.PlayingMusic ? 0f : fadeTimeMs);
            }
            catch (Exception e)
            {
                Events.StaticEvent<Events.Audio.MusicSegmentEnd.Data>.Unsubscribe(local_MusicSegmentEnd);
                Debug.Log($"Failed to play track {trackName} because: {e}");
                StopCustomMusic();
                return false;
            }

            Events.Audio.MusicSegmentEnd.Unsubscribe(local_MusicSegmentEnd);
            
            if (failed || AudioManager.CurrentAudioFile_ == null || AudioManager.CurrentAudioFile_.FileName != track.FileLocation)
            {
                Debug.Log($"Failed to play track {trackName}");
                StopCustomMusic();
                return false;
            }

            G.Sys.AudioManager_.perLevelMusicOverride_ = true;
            G.Sys.AudioManager_.currentMusicState_ = AudioManager.MusicState.PerLevel;

            mod_.Variables.CurrentTrackName = trackName;
            mod_.Variables.PlayingMusic = true;

            AudioManager.PostEvent("Mute_All_Music");

            return true;
        }

        public void StopCustomMusic()
        {
            if (mod_.Variables.PlayingMusic && G.Sys.AudioManager_.CurrentMusicState_ == AudioManager.MusicState.PerLevel)
            {
                G.Sys.AudioManager_.perLevelMusicOverride_ = false;

                G.Sys.AudioManager_.SwitchToOfficialMusic();
                mod_.Variables.PlayingMusic = false;
                mod_.Variables.CurrentTrackName = null;
            }
        }

        public string GetMusicChoiceValue(GameObject obj, string key)
        {
            var listener = obj.GetComponent<ZEventListener>();
            
            if (listener == null || !listener.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicChoice>()))
            {
                return null;
            }

            var choice = mod_.Variables.CachedMusicChoice.GetOrCreate(listener, () => MusicChoice.FromObject(listener));
            
            if (choice == null)
            {
                return null;
            }

            choice.Choices.TryGetValue(key, out MusicChoiceEntry entry);

            if (entry == null)
            {
                return null;
            }

            return entry.Track;
        }

        public void DownloadAllTracks()
        {
            var levelPath = G.Sys.GameManager_.LevelPath_;

            if (G.Sys.LevelEditor_ != null && G.Sys.LevelEditor_.Active_)
            {
                levelPath = "EditorMusic/EditorMusic";
            }

            Update();

            var tracks = mod_.Variables.CachedMusicTrack.Pairs.Select(pair => pair.Value);
            var embedded = tracks.Where(track => track.Embedded.Length > 0);
            var download = tracks.Where(track => track.Embedded.Length == 0 && !string.IsNullOrEmpty(track.DownloadUrl));

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var track in embedded.Concat(download))
            {
                DownloadTrack(track, levelPath);
                if (stopwatch.ElapsedMilliseconds >= mod_.Config.MaxMusicLevelLoadTimeMilli)
                {
                    break;
                }
            }
        }

        public MusicTrack GetTrack(string name)
        {
            foreach (var pair in mod_.Variables.CachedMusicTrack.Pairs)
            {
                var track = pair.Value;

                if (track.Name == name)
                {
                    var err = track.GetError();

                    if (err == null)
                    {
                        return track;
                    }
                }
            }
            return null;
        }

        public string DownloadTrack(MusicTrack track, string levelPath)
        {
            var err = track.GetError();

            if (err != null)
            {
                return null;
            }

            if (track.FileLocation != null || track.Attempted)
            {
                return track.FileLocation;
            }

            track.Attempted = true;
            var trackPath = $"{levelPath}.{track.FileName}{track.FileType}";
            var statePath = $"{levelPath}.{track.FileName}.musicstate";
            var upToDate = false;

            try
            {
                if (FileEx.Exists(statePath) && FileEx.Exists(trackPath))
                {
                    var stateStr = FileEx.ReadAllText(statePath);

                    if (stateStr == track.Version)
                    {
                        upToDate = true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to read music state file {statePath} because {e}");
                return null;
            }

            if (upToDate)
            {
                track.FileLocation = trackPath;
                return trackPath;
            }

            if (track.Embedded.Length > 0)
            {
                try
                {
                    FileEx.WriteAllBytes(trackPath, track.Embedded);
                    track.FileLocation = trackPath;
                }
                catch (Exception e)
                {
                    Debug.Log($"Failed to write track {trackPath} (embed) because {e}");
                    return null;
                }
            }
            else if (!string.IsNullOrEmpty(track.DownloadUrl))
            {
                var request = UnityEngine.Networking.UnityWebRequest.Get(track.DownloadUrl);
                byte[] file = new byte[0];
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                var operation = request.Send();

                while (!operation.isDone)
                {
                    if (stopwatch.ElapsedMilliseconds > mod_.Config.MaxMusicDownloadTimeMilli)
                    {
                        request.Abort();
                        Debug.Log($"Failed to download {track.Name}: it took too long!");
                        break;
                    }
                    else if (request.downloadedBytes >= (ulong)mod_.Config.MaxMusicDownloadSizeBytes)
                    {
                        request.Abort();
                        Debug.Log($"Failed to download {track.Name}: it is too big!");
                        break;
                    }
                }

                stopwatch.Stop();

                if (operation.isDone)
                {
                    if (request.isError)
                    {
                        Debug.Log($"Failed to download {track.Name}: Error {request.error}");
                    }
                    else
                    {
                        file = request.downloadHandler.data;
                    }
                }

                request.Dispose();

                if (file.Length > 0)
                {
                    try
                    {
                        FileEx.WriteAllBytes(trackPath, file);
                        track.FileLocation = trackPath;
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Failed to write track {trackPath} (download) because {e}");
                        return null;
                    }
                }
                else
                {
                    Debug.Log($"Failed to download {track.Name}: no data!");
                }
            }
            else
            {
                Debug.Log($"Impossible state when saving custom music track {track.Name}");
            }

            try
            {
                FileEx.WriteAllText(statePath, track.Version);
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to write music state file {statePath} because {e}");
            }

            return trackPath;
        }
    }
}
