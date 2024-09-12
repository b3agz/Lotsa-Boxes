using Godot;

namespace LotsaBoxes.Extensions {

	public static class Extensions {

		public static Vector3I FloorToInt (this Vector3 v3) {

			return new Vector3I (Mathf.FloorToInt(v3.X), Mathf.FloorToInt(v3.Y), Mathf.FloorToInt(v3.Z));

		}

	}

}
