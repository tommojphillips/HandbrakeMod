using UnityEngine;
using HutongGames.PlayMaker;
using MSCLoader;

namespace HandbrakeMod {
	public class HandbrakeMod : Mod {
		public override string ID => "HandbrakeMod";
		public override string Name => "Handbrake Mod";
		public override string Author => "tommojphillips";
		public override string Version => "1.5";
        public override string Description => "Control the handbrake in all vehicles with the handbrake keybind";

		private SettingsDropDownList handbrakeMode;
        private readonly string[] handbrakeModeNames = new string[] { "Hold", "Toggle" };

		public override void ModSetup() {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.PostLoad, Mod_PostLoad);
			SetupFunction(Setup.ModSettings, Mod_ModSettings);
			SetupFunction(Setup.ModSettingsLoaded, Mod_ModSettingsLoaded);
		}

        private void Mod_ModSettings() {
            handbrakeMode = Settings.AddDropDownList("handbrake_mode", "Handbrake Mode", handbrakeModeNames, 0, onModeChanged);
        }
        private void Mod_ModSettingsLoaded() {
            onModeChanged();
        }
        private void Mod_OnLoad() {
            setupSatsuma("SATSUMA(557kg, 248)");
            setupHayosiko("HAYOSIKO(1500kg, 250)");
            setupRuscko("RCO_RUSCKO12(270)");
            setupGifu("GIFU(750/450psi)");
            setupKekmet("KEKMET(350-400psi)");
        }
        private void Mod_PostLoad() {
            if (ModLoader.IsModPresent("ToyotaHiacePickup")) {
                setupHayosiko("TOYOTA HIACE PICKUP(1150kg, CL1980)");
            }

            if (ModLoader.IsModPresent("SecondGifuRemake")) {
                setupGifu("GIFU(650/350psi)");
            }

            ModConsole.Print($"{ID}: Loaded");
        }

        public void setupRuscko(string path) {
            AudioSource hb_on = GameObject.Find("MasterAudio/CarFoley/parkbrake_on").GetComponent<AudioSource>();
            AudioSource hb_off = GameObject.Find("MasterAudio/CarFoley/parkbrake_off").GetComponent<AudioSource>();

            GameObject ruscko = GameObject.Find(path);
            if (ruscko == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path}.");
                return;
            }

            Transform lever = ruscko.transform.Find("LOD/Dashboard/ParkingBrake");
            if (lever == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} handbrake Lever.");
                return;
            }
            
            PlayMakerFSM use = lever.GetPlayMaker("Use");
            if (use == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} Use.");
                return;
            }

            FsmFloat knobPos = use.FsmVariables.FindFsmFloat("KnobPos");
            if (knobPos == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} knobPos.");
                return;
            }

            Transform drive_trigger = ruscko.transform.Find("LOD/PlayerTrigger/DriveTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger.");
                return;
            }

            PlayMakerFSM drive_trigger_fsm = drive_trigger.GetPlayMaker("PlayerTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger FSM.");
                return;
            }

            HandbrakeFunc func = new HandbrakeFunc() {
                transform = lever,
                handbrakeOnAudio = hb_on,
                handbrakeOffAudio = hb_off,
                knobPos = knobPos,
                maxKnobPos = 0.1f,
            };

            if (!drive_trigger_fsm.FsmInject("Player in car", func.update, true, -1, false)) {
                ModConsole.Error($"[HandbrakeMod] Failed to inject {path} drive trigger.");
                return;
            }
        }
        public void setupHayosiko(string path) {
            GameObject hayosiko = GameObject.Find(path);
            if (hayosiko == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path}.");
                return;
            }

            Transform lever = hayosiko.transform.Find("LOD/Dashboard/ParkingBrake");
            if (lever == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} handbrake Lever.");
                return;
            }
            
            PlayMakerFSM use = lever.GetPlayMaker("Use");
            if (use == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} Use.");
                return;
            }

            FsmFloat knobPos = use.FsmVariables.FindFsmFloat("KnobPos");
            if (knobPos == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} knobPos.");
                return;
            }

            Transform drive_trigger = hayosiko.transform.Find("LOD/PlayerTrigger/DriveTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger.");
                return;
            }

            PlayMakerFSM drive_trigger_fsm = drive_trigger.GetPlayMaker("PlayerTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger FSM.");
                return;
            }

            HandbrakeFunc func = new HandbrakeFunc() {
                transform = lever,
                handbrakeOnAudio = null,
                handbrakeOffAudio = null,
                knobPos = knobPos,
                maxKnobPos = 0.1f,
            };
            
            if (!drive_trigger_fsm.FsmInject("Player in car", func.update, true, -1, false)) {
                ModConsole.Error($"[HandbrakeMod] Failed to inject {path} drive trigger.");
                return;
            }
        }
        public void setupSatsuma(string path) {
            AudioSource hb_on = GameObject.Find("MasterAudio/CarFoley/handbrake_on").GetComponent<AudioSource>();
            AudioSource hb_off = GameObject.Find("MasterAudio/CarFoley/handbrake_off").GetComponent<AudioSource>();

            GameObject satsuma = GameObject.Find(path);
            if (satsuma == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path}.");
                return;
            }

            Transform lever = satsuma.transform.Find("MiscParts/HandBrake/handbrake(xxxxx)/handbrake lever");
            if (lever == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} handbrake Lever.");
                return;
            }

            PlayMakerFSM use = lever.GetPlayMaker("Use");
            if (use == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} Use.");
                return;
            }

            FsmFloat knobPos = use.FsmVariables.FindFsmFloat("KnobPos");
            if (knobPos == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} KnobPos.");
                return;
            }

            Transform drive_trigger = satsuma.transform.Find("PlayerTrigger/DriveTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger.");
                return;
            }

            PlayMakerFSM drive_trigger_fsm = drive_trigger.GetPlayMaker("PlayerTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger FSM.");
                return;
            }

            HandbrakeFunc func = new HandbrakeFunc() {
                transform = lever,
                handbrakeOnAudio = hb_on,
                handbrakeOffAudio = hb_off,
                knobPos = knobPos,
                maxKnobPos = 20f,
            };

            if (!drive_trigger_fsm.FsmInject("Player in car", func.update, true, -1, false)) {
                ModConsole.Error($"[HandbrakeMod] Failed to inject {path} drive trigger.");
                return;
            }
        }
        public void setupGifu(string path) {
            GameObject gifu = GameObject.Find(path);
            if (gifu == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path}.");
                return;
            }

            Transform lever = gifu.transform.Find("Dashboard/Knobs/Parking Brake");
            if (lever == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} parkbrake Lever.");
                return;
            }

            PlayMakerFSM use = lever.GetPlayMaker("Use");
            if (use == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} Lever Use.");
                return;
            }

            FsmBool brake = use.FsmVariables.FindFsmBool("Brake");
            if (brake == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} Brake.");
                return;
            }

            Transform drive_trigger = gifu.transform.Find("LOD/PlayerTrigger/DriveTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger.");
                return;
            }

            PlayMakerFSM drive_trigger_fsm = drive_trigger.GetPlayMaker("PlayerTrigger");
            if (drive_trigger_fsm == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger FSM.");
                return;
            }

            FsmState state = use.GetState("Wait player");
            if (state == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find state 'Wait player' on {path}.");
                return;
            }

            state.AddTransition("USE", "Flip");

            ParkbrakeFunc func = new ParkbrakeFunc() {
                brake = brake,
                drive_trigger_fsm = use,
                drive_trigger_name = "USE",
            };            

            if (!drive_trigger_fsm.FsmInject("Player in car", func.update, true, -1, false)) {
                ModConsole.Error($"[HandbrakeMod] Failed to inject {path} drive trigger.");
                return;
            }
        }
        public void setupKekmet(string path) {
            GameObject kekmet = GameObject.Find(path);
            if (kekmet == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path}.");
                return;
            }

            Transform lever = kekmet.transform.Find("Dashboard/ParkingBrake");
            if (lever == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} parkbrake Lever.");
                return;
            }

            PlayMakerFSM use = lever.GetPlayMaker("Use");
            if (use == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} Lever Use.");
                return;
            }

            FsmBool brake = use.FsmVariables.FindFsmBool("Brake");
            if (brake == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} Brake.");
                return;
            }

            Transform drive_trigger = kekmet.transform.Find("LOD/PlayerTrigger/DriveTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger.");
                return;
            }

            PlayMakerFSM drive_trigger_fsm = drive_trigger.GetPlayMaker("PlayerTrigger");
            if (drive_trigger == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find {path} DriveTrigger FSM.");
                return;
            }

            FsmState state = use.GetState("Wait player");
            if (state == null) {
                ModConsole.Error($"[HandbrakeMod] Failed to find state 'Wait player' on {path}.");
                return;
            }
            state.AddTransition("USE", "Flip");

            ParkbrakeFunc func = new ParkbrakeFunc() {
                brake = brake,
                drive_trigger_fsm = use,
                drive_trigger_name = "USE"
            };

            if (!drive_trigger_fsm.FsmInject("Player in car", func.update, true, -1, false)) {
                ModConsole.Error($"[HandbrakeMod] Failed to inject {path} drive trigger.");
                return;
            }
        }

        private void onModeChanged() {
            int i = handbrakeMode.GetSelectedItemIndex();
            if (i >= 0 && i < (int)HandbrakeMode.Count) {
                HandbrakeFunc.mode = (HandbrakeMode)i;
                ParkbrakeFunc.mode = (HandbrakeMode)i;
            }
        }
    }
}
