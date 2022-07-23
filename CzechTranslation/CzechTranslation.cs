using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;

namespace CzechTranslation
{
    public class CzechTranslation : ModBehaviour
    {
        public static CzechTranslation Instance;

        public static string translationFile = "translations/czech.xml";

        private void Start()
        {
            var api = ModHelper.Interaction.TryGetModApi<ILocalizationAPI>("xen.LocalizationUtility");
            api.RegisterLanguage(this, "Czech", translationFile);
        }
    }
}
