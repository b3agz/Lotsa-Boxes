using Godot;
using LotsaBoxes.Extensions;

namespace LotsaBoxes.Data {

	public class ChunkData {

		private BlockData[,,] _blocks;
		public Vector3 Position { get; }

		public ChunkData (Vector3 position) {
			_blocks = new BlockData[Chunk.WIDTH, Chunk.HEIGHT, Chunk.WIDTH];
			Position = position.Floor();
		}

		/// <summary>
		/// Returns the block at the given LOCAL position. Performs check to ensure block is valid, returns null if not.
		/// </summary>
		/// <param name="localPosition">Vector3 of the LOCAL position of the block.</param>
		/// <returns>BlockData, returns null if not a valid position.</returns>
		public BlockData GetBlock(Vector3 localPosition) {

			localPosition.Floor();
			Vector3I pos = new Vector3I ((int)localPosition.X, (int)localPosition.Y, (int)localPosition.Z);
			if (!IsPosInsideChunk(pos)) {
				return null;
			}
			return _blocks[pos.X, pos.Y, pos.Z];

		}

		/// <summary>
		/// Returns BlockData from the chunk's blocks array. Does NOT check that the block is within bounds. Will throw IndexOutOfRangeException.
		/// </summary>
		/// <param name="x">The LOCAL X position of the block.</param>
		/// <param name="y">The LOCAL Y position of the block.</param>
		/// <param name="z">The LOCAL Z position of the block.</param>
		/// <returns></returns>
		public BlockData UnsafeGetBlock(int x, int y, int z) {
			return _blocks[x, y ,z];
		}

		public BlockData UnsafeGetBlock(Vector3I position) {
			return _blocks[position.X, position.Y, position.Z];
		}

		/// <summary>
		/// Returns BlockData from the chunk's blocks array. Does NOT check that the block is within bounds. Will throw IndexOutOfRangeException.
		/// </summary>
		/// <param name="position">Vector3 representing the LOCAL position of this block.</param>
		/// <returns></returns>
		public BlockData UnsafeGetBlock(Vector3 position) {
			Vector3I pos = position.FloorToInt();
			return _blocks[pos.X, pos.Y, pos.Z];
		}

		/// <summary>
		/// Populates the ChunkData's BlockData array from a 3D array of ints.
		/// </summary>
		/// <param name="blocks">int[,,] a 3 dimensional array of ints representing block IDs.</param>
		public void SetBlocks(int[,,] blocks) {

			// Loop through every position in this chunk.
			for (int x = 0; x < Chunk.WIDTH; x++) {
				for (int z = 0; z < Chunk.WIDTH; z++) {
					for (int y = 0; y < Chunk.HEIGHT; y++) {

						// Create the block at this position.
						_blocks[x,y,z] = new BlockData(blocks[x,y,z], this);

						// Loop through all the neighbour positions for this block.
						for (int i = 0; i < MeshRepository.NeighbourOffset.Length; i++) {

							// Set the neighbour position.
							Vector3I v3 = new Vector3I(x,y,z) + MeshRepository.NeighbourOffset[i];

							// If the neighbour position is inside this chunk, get the neighbour block directly and set the neighbour
							// if that neighbour exists. If it is not inside the chunk, then request it from the World class.
							if (IsPosInsideChunk(v3)) {
								BlockData neighbour = UnsafeGetBlock(v3);
								if (neighbour != null) _blocks[x,y,z].SetNeighbour(i, neighbour, true);
							} else {
								BlockData neighbour = Manager.Instance.World.GetBlock(new Vector3(x,y,z) + Position);
								if (neighbour != null) _blocks[x,y,z].SetNeighbour(i, neighbour, true);
							}
						}
					}
				}
			}
		}

		public void SetBlock (BlockData blockData, int x, int y, int z) {
			_blocks[x, y, z] = blockData;
		}

		public static bool IsPosInsideChunk(Vector3I position) => (
				position.X >= 0 && position.X < Chunk.WIDTH &&
				position.Z >= 0 && position.Z < Chunk.WIDTH &&
				position.Y >= 0 && position.Y < Chunk.HEIGHT
			);

	}

}
