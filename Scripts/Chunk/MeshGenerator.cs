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

		public Mesh GetMesh(int[,,] map, Vector3 chunkPos) {

			_indiceCount = 0;
			_surfaceTool = new();
			_surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
			
			for (int x = 0; x < Chunk.WIDTH; x++) {
				for (int z = 0; z < Chunk.WIDTH; z++) {
					for (int y = 0; y < Chunk.HEIGHT; y++) {
						if (map[x,y,z] > 0) {
							for (int i = 0; i < MeshRepository.Cube.Sides.Length; i++) {
								Vector3I v3 = new Vector3I (x,y,z) + MeshRepository.Cube.Sides[i].Neighbour;
								if (IsPosInsideChunk(v3) && map[v3.X, v3.Y, v3.Z] == 0) {
									AddToMesh(i, new Vector3(x, y, z), map[x,y,z]);
								} else if (!IsPosInsideChunk(v3) && !Manager.Instance.World.IsBlockSolid(v3 + chunkPos)) {
									AddToMesh(i, new Vector3(x, y, z), map[x,y,z]);
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
