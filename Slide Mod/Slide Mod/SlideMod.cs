using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

[BepInPlugin("com.lamia.slidemod", "SlideMod", "1.0.0")]
public class SlideMod : BaseUnityPlugin
{
    internal static ManualLogSource Log;

    private void Awake()
    {
        Log = Logger;
        Log.LogInfo("SlideMod loaded.");
        Harmony harmony = new Harmony("com.lamia.slidemod");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(Character), "Awake")]
public static class PalThrow
{
    [HarmonyPostfix]
    public static void AwakePatch(Character __instance)
    {
        if (!__instance.IsLocal)
            return;
        if ((Object)(object)((Component)__instance).GetComponent<SlideModPatch>() == (Object)null)
        {
            ((Component)__instance).gameObject.AddComponent<SlideModPatch>();
            SlideMod.Log.LogInfo((object)("SlideModPatch added to: " + ((Object)__instance).name));
        }
    }
}

public class SlideModPatch : MonoBehaviourPun
{
    private Character character;
    private CharacterMovement charMovement;
    private void Start()
    {
        character = ((Component)this).GetComponent<Character>();
        charMovement = this.GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (!character.IsLocal)
        
            return;
        if (Input.GetKey(KeyCode.LeftAlt))
            {
            Vector3 slideForce = character.data.avarageLastFrameVelocity * 55f ;
            foreach (var part in character.refs.ragdoll.partList)
            {
                part.AddForce(slideForce, ForceMode.Force);
            }
            SlideMod.Log.LogInfo("Slided with "+ slideForce);

        }
        
    }
}
