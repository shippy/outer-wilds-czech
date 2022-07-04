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

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            WriteLine($"My mod {nameof(CzechTranslation)} is loaded!");
        }

        public static void WriteLine(string msg, MessageType type = MessageType.Info)
        {
            Instance.ModHelper.Console.WriteLine($"{type}: {msg}", type);
        }

        public static void WriteError(string msg) => WriteLine(msg, MessageType.Error);
    }
}
