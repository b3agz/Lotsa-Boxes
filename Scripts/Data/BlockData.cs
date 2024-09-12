namespace LotsaBoxes.Data {

	public class BlockData {

		// TODO: Replace with "Identifier" system.
		public int ID { get; } = 0;
		public BlockData[] Neighbours { get; private set; }

		public bool Air => ID == 0;
		public bool IsSolid => ID > 0;

		private ChunkData _chunkData;

		public BlockData (int id, ChunkData chunkData) {
			ID = id;
			Neighbours = new BlockData[6];
			_chunkData = chunkData;
		}

		public void SetNeighbour(int sideIndex, BlockData blockData, bool setInverse = false) {
			Neighbours[sideIndex] = blockData;
			if (setInverse) {
				blockData.SetNeighbour(MeshRepository.InverseNeighbourOffset[sideIndex], this, false);
			}
		}

		public bool RenderFace(int sideIndex) {
			BlockData block = Neighbours[sideIndex];
			return block == null || block.Air;
		}

	}

}
