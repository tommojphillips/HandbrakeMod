using UnityEngine;
using HutongGames.PlayMaker;

namespace HandbrakeMod {

	internal enum HandbrakeMode {
		Hold,
		Toggle,
		Count,
	}

	public interface IHandbrake {
		void update();
		void toggle();
		void hold();
	}

    public class HandbrakeFunc : IHandbrake {
		internal Transform transform;
		internal AudioSource handbrakeOffAudio;
		internal AudioSource handbrakeOnAudio;
		internal FsmFloat knobPos;
		internal float maxKnobPos;

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
				if (knobPos.Value == 0f) {
					increaseHandbrake();
				}
				else {
					decreaseHandbrake();
				}
			}
		}
        public void hold() {
			if (cInput.GetKeyDown("Handbrake")) {
                increaseHandbrake();
			}
			else if (cInput.GetKeyUp("Handbrake")) {
				decreaseHandbrake();
			}
		}

		private void increaseHandbrake() {
			if (knobPos.Value != maxKnobPos) {
				if (handbrakeOnAudio != null) {
					handbrakeOnAudio.transform.position = transform.position;
					handbrakeOffAudio?.Stop();
					handbrakeOnAudio.Play();
				}
				knobPos.Value = maxKnobPos;
			}
		}
		private void decreaseHandbrake() {
			if (knobPos.Value != 0f) {
				if (handbrakeOffAudio != null) {
					handbrakeOffAudio.transform.position = transform.position;
					handbrakeOnAudio?.Stop();
					handbrakeOffAudio.Play();
				}
				knobPos.Value = 0f;
			}
		}
	}
}
