using HarmonyLib;
using OWML.Common;
using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CzechTranslation
{
    /// <summary>
    /// Outer Wilds keeps its Latin-script UI in baked (non-dynamic) bitmap fonts that only cover
    /// Latin-1 plus a handful of extra glyphs. Czech needs Latin Extended-A (ě š č ř ž ů ď ť ň …),
    /// which those fonts simply don't contain, so Unity drops the characters on screen.
    ///
    /// Polyglot's AddLanguageFont can't help here: it switches the game into its "non-Latin"
    /// mode where a single font replaces *everything*, including the Space Mono ship log and
    /// Brandon Grotesque dialogue fonts that already render Czech fine (see PR #27 for why that
    /// looked wrong).
    ///
    /// Instead we stay in Latin mode and only swap the handful of fonts that lack Czech glyphs for
    /// dynamic (TTF-backed) fonts that already ship with the game for other languages:
    ///   ITC Serif Gothic (title + pause + settings menus, game over)
    ///       -> "ParaType - PT Sans Pro Bold_Dynamic"  (what the game itself uses for Russian)
    ///   Gill Sans MT (popups, prompts, loading tips)
    ///       -> "ParaType - Futura PT Medium_Dynamic"  (geometric sans, close match)
    ///   VCR OSD Mono (signalscope frequency labels)
    ///       -> "SpaceMono-Regular_Dynamic"
    /// No extra assets, no asset bundle, no Unity editor required. Fonts that already contain the
    /// Czech glyphs (Brandon Grotesque dialogue, Space Mono ship log, …) are left untouched.
    /// </summary>
    [HarmonyPatch]
    public static class CzechFontFix
    {
        private static TextTranslation.Language _czech = TextTranslation.Language.UNKNOWN;

        // Fonts that lack Czech glyphs -> name fragment of the replacement dynamic font.
        private static readonly Dictionary<string, string> Replacements = new Dictionary<string, string>
        {
            { "Gill Sans MT Menu", "Futura PT Medium_Dynamic" },
            { "Gill Sans MT", "Futura PT Medium_Dynamic" },
            { "Gill Sans MT Bold", "Futura PT Medium_Dynamic" },
            { "Adobe - SerifGothicStd", "PT Sans Pro Bold_Dynamic" },
            { "Adobe - SerifGothicStd-ExtraBold", "PT Sans Pro Bold_Dynamic" },
            { "Adobe - SerifGothicStd-Black", "PT Sans Pro Bold_Dynamic" },
            { "Adobe - SerifGothicStd-ExtraBoldExtraLarge", "PT Sans Pro Bold_Dynamic" },
            { "Adobe - SerifGothicStd_GameOver", "PT Sans Pro Bold_Dynamic" },
            { "Adobe - SerifGothicStd_Dynamic", "PT Sans Pro Bold_Dynamic" },
            { "VCR_OSD_MONO", "SpaceMono-Regular_Dynamic" },   // signalscope frequency labels
        };

        private static readonly Dictionary<string, Font> _resolved = new Dictionary<string, Font>();

        // Original font of every Text we touched, so we can put it back when the player switches
        // to another language without restarting the game.
        private static readonly Dictionary<Text, Font> _originals = new Dictionary<Text, Font>();

        private static readonly HashSet<string> _warned = new HashSet<string>();

        public static void Init(string languageName)
        {
            _czech = EnumUtils.Parse<TextTranslation.Language>(languageName, TextTranslation.Language.UNKNOWN);
            if (_czech == TextTranslation.Language.UNKNOWN)
            {
                CzechTranslation.Instance.ModHelper.Console.WriteLine($"[CzechFontFix] Language \"{languageName}\" not registered, font fix disabled.", MessageType.Error);
                return;
            }

            Harmony.CreateAndPatchAll(typeof(CzechFontFix), "shippy.czech.fontfix");
            CzechTranslation.Instance.ModHelper.Console.WriteLine("[CzechFontFix] Patched.", MessageType.Success);
        }

        private static bool IsCzechActive()
        {
            if (_czech == TextTranslation.Language.UNKNOWN) return false;
            try
            {
                if (TextTranslation.Get() != null) return TextTranslation.Get().GetLanguage() == _czech;
            }
            catch (Exception) { }
            try
            {
                return PlayerData.GetSavedLanguage() == _czech;
            }
            catch (Exception) { return false; }
        }

        private static Font Resolve(string nameFragment)
        {
            if (_resolved.TryGetValue(nameFragment, out var cached) && cached != null) return cached;

            var font = Resources.FindObjectsOfTypeAll<Font>()
                .FirstOrDefault(f => f != null && f.dynamic && f.name.Contains(nameFragment));

            if (font != null) _resolved[nameFragment] = font;
            else if (_warned.Add(nameFragment))
                CzechTranslation.Instance.ModHelper.Console.WriteLine($"[CzechFontFix] Replacement font \"{nameFragment}\" not loaded (yet).", MessageType.Warning);

            return font;
        }

        /// <summary>Swap the font of <paramref name="text"/> if Czech is active and the font can't render it.</summary>
        public static void Fix(Text text)
        {
            if (text == null || text.font == null) return;

            if (!IsCzechActive())
            {
                Restore(text);
                return;
            }

            var current = text.font;
            if (!Replacements.TryGetValue(current.name, out var replacementName))
            {
                // Not in our list. Still worth knowing about baked fonts that can't do Czech.
                if (!current.dynamic && !current.HasCharacter('ř') && _warned.Add(current.name))
                    CzechTranslation.Instance.ModHelper.Console.WriteLine($"[CzechFontFix] Baked font without Czech glyphs not handled: \"{current.name}\" (on {text.gameObject.name})", MessageType.Warning);
                return;
            }

            var replacement = Resolve(replacementName);
            if (replacement == null || replacement == current) return;

            if (!_originals.ContainsKey(text)) _originals[text] = current;
            text.font = replacement; // re-enters the setter postfix, which then finds nothing to do
        }

        private static void Restore(Text text)
        {
            if (_originals.TryGetValue(text, out var original))
            {
                _originals.Remove(text);
                if (original != null && text.font != original) text.font = original;
            }
        }

        private static void Prune()
        {
            foreach (var dead in _originals.Keys.Where(t => t == null).ToList()) _originals.Remove(dead);
        }

        // Fonts assigned in prefabs/scenes never go through the setter, so catch them on enable.
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Text), "OnEnable")]
        public static void Text_OnEnable(Text __instance) => Fix(__instance);

        // Anything that assigns a font at runtime (FontAndLanguageController, UIStyleManager consumers, mods…).
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Text), nameof(Text.font), MethodType.Setter)]
        public static void Text_set_font(Text __instance) => Fix(__instance);

        // Language changed: re-run over everything the game tracks, and put originals back if we left Czech.
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FontAndLanguageController), "InitializeFont")]
        public static void FontAndLanguageController_InitializeFont()
        {
            Prune();
            if (IsCzechActive()) return;
            foreach (var text in _originals.Keys.ToList()) if (text != null) Restore(text);
        }
    }
}
