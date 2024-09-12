using Godot;
using LotsaBoxes;
using System;

namespace LotsaBoxes {

	public partial class Manager : Node {

		public static Manager Instance { get; private set; }

		public override void _EnterTree() {
			if (Instance != null && Instance != this) {
				QueueFree();
				return;
			} else {
				Instance = this;
			}
			base._EnterTree();
		}

		[Export] public World World { get; private set; }
		[Export] public MeshGenerator MeshGenerator { get; private set; }
		[Export] public Camera3D Camera { get; private set; }

	}
}
