using HutongGames.PlayMaker;

namespace HandbrakeMod {
    public class ParkbrakeFunc : IHandbrake {
        internal FsmBool brake;
        internal PlayMakerFSM drive_trigger_fsm;
        internal string drive_trigger_name;

        internal static HandbrakeMode mode;

        public void update() {
            switch (mode) {
                case HandbrakeMode.Hold:
                    hold();
                    break;
                case HandbrakeMode.Toggle:
                    toggle();
                    break;
            }
        }
        public void toggle() {
            if (cInput.GetKeyDown("Handbrake")) {
                if (!brake.Value) {
                    parkbrakeOn();
                }
                else {
                    parkbrakeOff();
                }
            }
        }
        public void hold() {
            if (cInput.GetKeyDown("Handbrake")) {
                parkbrakeOn();
            }
            else if (cInput.GetKeyUp("Handbrake")) {
                parkbrakeOff();
            }
        }

        private void parkbrakeOn() {
            if (!brake.Value) {
                drive_trigger_fsm.SendEvent(drive_trigger_name);
                brake.Value = true;
            }
        }
        private void parkbrakeOff() {
            if (brake.Value) {
                drive_trigger_fsm.SendEvent(drive_trigger_name);
                brake.Value = false;
            }
        }
    }
}
