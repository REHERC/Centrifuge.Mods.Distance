using UnityEngine;

namespace Distance.TextureModifier
{
    public class Declarations
    {
        public const string CustomShadelessTexture = "Custom/Shadeless/Texture";

        public static readonly string[] textureFileExtensions = new string[]
        {
            ".bmp",
            ".png",
            ".jpg", ".jpe", ".jpeg", ".jfif",
        };

        public static readonly string[] materialBlackList = new string[]
        {
            "Light",
            "LevelEditorCenterpoint",
            "CenterpointFontMat",
            "ScreenTextMaterial",
            "Menu Atlas",
            "Default-Material",
            "Font Material",
            "DenseBoldFontMat",
            "PetitaBoldFontMat",
            "BebasFontMat",
            "[NGUI] Font Material",
            "[NGUI] Menu Atlas",
            "[NGUI] Unlit/Transparent Colored",
            "[NGUI] Unlit/Additive Colored",
            "RenderedCubeMap",
            "EmpireTeleport",
            "Teleport",
            "Null"
        };

        public static bool IsBlacklisted(Material material)
        {
            foreach (var blacklistEntry in materialBlackList)
            {
                if (material.name.ToLower().Contains(blacklistEntry.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public static readonly string[] materialTextureProperties = new string[] {
            "_Alpha",
            "_AlphaTex",
            ///"_BackTex",
            "_BumpMap",
            "_BumpSpecMap",
            "_CameraMotionTex",
            "_CarScreenTex",
            "_CityDetailTex",
            "_CloudDetailTex",
            "_ColorBuffer",
            "_Control",
            "_CoverTex",
            "_CromaTex",
            "_CutOffsetTex",
            "_DecalTex",
            "_Detail",
            "_DetailAlbedoMap",
            "_DetailMask",
            "_DetailNormalMap",
            "_DetailTex",
            "_DirectionTex",
            "_DirtyLensTexture",
            "_DispTex",
            "_DotsTex",
            ///"_DownTex",
            "_EmissionMap",
            "_Emissive",
            "_EmitDetailTex",
            "_EmitTex",
            "_FadeTex",
            "_FallOff",
            "_FgOverlap",
            "_Flare",
            "_FrameTex",
            ///"_FrontTex",
            "_FullScreenMask",
            "_GlitchTex",
            "_GlossMap",
            "_GradientTex",
            "_GrainTex",
            "_HardnessTex",
            "_Illum",
            ///"_LeftTex",
            "_LightMap",
            "_LightTex",
            "_LightTextureB0",
            "_LinesTex",
            "_LowRez",
            "_Main",
            "_MainTex2",
            "_MainTex3",
            "_MainTex",
            "_MainTexture",
            "_Mask",
            "_MetallicGlossMap",
            "_MotionTex",
            "_NoiseTex",
            "_NonBlurredTex",
            "_Normal",
            "_NormalTex",
            "_OcclusionMap",
            "_ParallaxMap",
            "_PatternBumpTex",
            "_PatternTex",
            "_RampTex",
            "_RandomTexture",
            "_ReflectMap",
            "_Refraction",
            "_RgbDepthTex",
            "_RgbTex",
            ///"_RightTex",
            "_ScratchTex",
            "_SeamlessBumpTex",
            "_SeamlessDetailTex",
            "_SeamlessTex",
            "_ShadowOffset",
            ///"_Skybox",
            "_SliceGuide",
            "_SparkleTex",
            "_Splat0",
            "_Splat1",
            "_Splat2",
            "_Splat3",
            "_SSAO",
            "_StaticTex",
            "_SubtractiveTex",
            "_TeleTex",
            "_Tex",
            "_TranslucencyMap",
            ///"_UpTex",
            "_VignetteTex",
            "_ZCurve",
            "NoiseTex",
            "StarColor",
            "StartColor"
        };
    }
}
