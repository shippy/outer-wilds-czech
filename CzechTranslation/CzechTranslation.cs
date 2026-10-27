using OWML.ModHelper;

namespace CzechTranslation
{
    public class CzechTranslation : ModBehaviour
    {
        public static CzechTranslation Instance;

        public static string languageName = "Czech";
        public static string translationFile = "translations/czech.xml";

        private void Start()
        {
            Instance = this;

            var api = ModHelper.Interaction.TryGetModApi<ILocalizationAPI>("xen.LocalizationUtility");
            api.RegisterLanguage(this, languageName, translationFile);

            // Menu / game-over fonts don't have Czech glyphs; swap them for ones that do (see CzechFontFix).
            CzechFontFix.Init(languageName);
        }
    }
}
