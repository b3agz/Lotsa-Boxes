using Godot;
using System;
using LotsaBoxes.Data;

namespace LotsaBoxes {

	public partial class MeshGenerator : Node {

		[Export] private Material _chunkMaterial;
		
		private int _indiceCount = 0;
		private SurfaceTool _surfaceTool = new();

		private void AddToMesh(int sideIndex, Vector3 position, int id) {

			MeshData data = MeshRepository.Cube.Sides[sideIndex];
			int count = 0;
			foreach (VertData vertData in data.Verts) {
				_surfaceTool.SetNormal(vertData.Normal);
				//_surfaceTool.SetUV(vertData.UV);
				_surfaceTool.SetUV(ConvertUV(vertData.UV, id));
				_surfaceTool.AddVertex(vertData.Position + position);
				count++;
			}
			
			foreach (int indice in data.Indices) {
				_surfaceTool.AddIndex(_indiceCount + indice);
			}
			_indiceCount += count;

		}

		public Mesh GetMesh(ChunkData chunkData, Vector3 chunkPos) {

			_indiceCount = 0;
			_surfaceTool = new();
			_surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
			
			// Loop through every position in the chunk.
			for (int x = 0; x < Chunk.WIDTH; x++) {
				for (int z = 0; z < Chunk.WIDTH; z++) {
					for (int y = 0; y < Chunk.HEIGHT; y++) {

						// Get the current block.
						BlockData block = chunkData.UnsafeGetBlock(x, y, z);
						if (!block.Air) {
							for (int i = 0; i < MeshRepository.Cube.Sides.Length; i++) {
								if (block.RenderFace(i)) {
									AddToMesh(i, new Vector3(x, y, z), block.ID);
								}
							}
						}
					}
				}
			}

			_surfaceTool.SetMaterial(_chunkMaterial);
			return _surfaceTool.Commit();

		}

		public static bool IsPosInsideChunk(Vector3I position) => (
				position.X >= 0 && position.X < Chunk.WIDTH &&
				position.Z >= 0 && position.Z < Chunk.WIDTH &&
				position.Y >= 0 && position.Y < Chunk.HEIGHT
			);


		public static Vector2 ConvertUV (Vector2 uv, int id) {

			Vector2 newUV = uv;
			newUV.X = (Chunk.TEXTURE_WIDTH * id) + (Chunk.TEXTURE_WIDTH * uv.X);
			return newUV;

		}

	}

}
