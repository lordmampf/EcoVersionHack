using Asphalt;
using Asphalt.Util;
using Eco.Core.Plugins.Interfaces;
using Eco.Plugins.Networking;
using Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EcoVersionHack
{
    [AsphaltPlugin(nameof(EcoVersionHack))]
    class EcoVersionHack : IModKitPlugin
    {
        public static HarmonyInstance Harmony { get; protected set; }

        public void OnEnable()
        {
            Harmony = HarmonyInstance.Create("com.eco.mods.version.hack");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public string GetStatus()
        {
            return "running...";
        }
    }

    [HarmonyPatch(typeof(Client), "Authorize")]
    internal class VersionCheckPatcher
    {
        public static IEnumerable<CodeInstruction> Transpiler(ILGenerator ilGenerator, MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            //    File.WriteAllLines("before.txt", codes.Select(c => c.ToString()));

            int index = codes.FindIndex(c => c.operand is string && ((string)c.operand).Contains("with incompatible version"));

            AsphaltLog.WriteLine("[EcoVersionHack] Found first index to patch: " + index);

            int i = 1;

            while (codes[index].opcode.Name != "br" && codes[index].opcode.Name != "ret")
            {
                codes.RemoveAt(index);
                i++;
            }

            codes.RemoveAt(index);

            AsphaltLog.WriteLine($"[EcoVersionHack] Removed {i} instructions!");

            //      File.WriteAllLines("after.txt", codes.Select(c => c.ToString()));

            return codes.AsEnumerable();
        }
    }
}
