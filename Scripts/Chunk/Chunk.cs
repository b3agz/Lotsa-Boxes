using Godot;
using System;

namespace LotsaBoxes {
	public partial class Chunk : Node3D {

		public const int WIDTH = 16;
		public const int HEIGHT = 16;
		public const float TEXTURE_WIDTH = 0.1f;

		[Export] private MeshInstance3D _meshInstance;
		public Vector3 PositionKey => GlobalPosition.Floor();

		private int[,,] _map;
		
		public override void _Ready() {
			//Initialise();
		}

		public void Initialise() {
			PopulateChunk();
			_meshInstance.Mesh = Manager.Instance.MeshGenerator.GetMesh(_map, PositionKey);
			Manager.Instance.World.AddChunk(this);
		}

		public void PopulateChunk() {

			_map = new int[WIDTH, HEIGHT, WIDTH];
			for (int x = 0; x < WIDTH; x++) {
				for (int z = 0; z < WIDTH; z++) {
					for (int y = 0; y < HEIGHT; y++) {
						_map[x,y,z] = (y > 10) ? 0 : 1;
						if (y == 0) _map[x,y,z] = 4;
						if (y == 10) _map[x,y,z] = 2;
					}
				}
			}
		}

		
		/// <summary>
		/// Takes in a global position and returns true if the block at that position is solid. Returns false if
		/// the given position is outside of the bounds of this chunk.
		/// </summary>
		/// <param name="position">Vector3 - the position being checked.</param>
		/// <returns>True if the given block is solid.</returns>
		public bool IsBlockSolid(Vector3 position) {

			Vector3 localPos = ConvertToLocalPos(position);
			if (IsLocalPosInsideChunk(localPos)) {
				return _map[(int)localPos.X, (int)localPos.Y, (int)localPos.Z] > 0;
			}
			return false;

		}

		/// <summary>
		/// Takes in a Vector3 position and returns that position relative to this chunk.
		/// </summary>
		/// <param name="position">Vector3 - the position being converted.</param>
		/// <returns>Vector3 - position relative to this chunk.</returns>
		public Vector3 ConvertToLocalPos(Vector3 position) => (position - GlobalPosition).Floor();

		/// <summary>
		/// Takes in a global position and returns true if that position is inside of this chunk.
		/// </summary>
		/// <param name="position">Vector3 - the global position being checked.</param>
		/// <returns>True if position is inside of this chunk.</returns>
		public bool IsGlobalPosInsideChunk(Vector3 position) { 
			Vector3 pos = ConvertToLocalPos(position);
			return (
				pos.X >= 0 && pos.X < Chunk.WIDTH &&
				pos.Z >= 0 && pos.Z < Chunk.WIDTH &&
				pos.Y >= 0 && pos.Y < Chunk.HEIGHT
			);
		}

		/// <summary>
		/// Takes in a local position and returns true if that position is within the boundaries of a chunk.
		/// </summary>
		/// <param name="localPosition">Vector3 - the local position being compared.</param>
		/// <returns>True if that position would be inside a chunk.</returns>
		public static bool IsLocalPosInsideChunk(Vector3 localPosition) => (
				localPosition.X >= 0 && localPosition.X < Chunk.WIDTH &&
				localPosition.Z >= 0 && localPosition.Z < Chunk.WIDTH &&
				localPosition.Y >= 0 && localPosition.Y < Chunk.HEIGHT
			);

	}
}
