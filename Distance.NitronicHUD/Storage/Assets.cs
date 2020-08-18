using Reactor.API.Logging;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Distance.NitronicHUD.Storage
{
    public class Assets
    {
        private string _filePath = null;

        private string RootDirectory { get; }

        private string FileName { get; set; }

        private string FilePath => _filePath ?? Path.Combine(Path.Combine(RootDirectory, "Assets"), FileName);

        private static Log Log => Mod.Instance.Logger;

        public AssetBundle Bundle { get; private set; }

        private Assets() { }

        public Assets(string fileName)
        {
            RootDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            FileName = fileName;

            if (!File.Exists(FilePath))
            {
                Log.Error($"Couldn't find requested asset bundle at {FilePath}");
                return;
            }

            Bundle = Load();
        }

        public static Assets FromUnsafePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Log.Error($"Could not find requested asset bundle at {filePath}");
                return null;
            }

            var ret = new Assets
            {
                _filePath = filePath,
                FileName = Path.GetFileName(filePath)
            };

            ret.Bundle = ret.Load();

            if (ret.Bundle == null)
            {
                return null;
            }

            return ret;
        }

        private AssetBundle Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    throw new FileNotFoundException(FilePath);
                }

                string urlPath = FilePath.ToLowerInvariant()
                .Replace(Path.DirectorySeparatorChar, '/')
                .Replace(Path.AltDirectorySeparatorChar, '/');

                WWW www = new WWW($"file:///{urlPath}");

                var assetBundle = www.assetBundle;

                Log.Info($"Loaded asset bundle {FilePath}");

                return assetBundle;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }
    }
}
