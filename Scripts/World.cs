using Godot;
using LotsaBoxes.Data;
using System;
using System.Collections.Generic;

namespace LotsaBoxes {

	public partial class World : Node {
		
		[Export] private PackedScene _chunkPrefab;
		public Dictionary<Vector3, Chunk> Chunks { get; private set; } = new();

		public override void _Ready() {

			for (int x = 0; x < Chunk.WIDTH * 4; x += Chunk.WIDTH) {
				for (int z = 0; z < Chunk.WIDTH * 4; z += Chunk.WIDTH) {
					Chunk chunk = _chunkPrefab.Instantiate() as Chunk;
					AddChild(chunk);
					chunk.GlobalPosition = new Vector3(x, 0f, z);
					chunk.Name = $"Chunk {x}, {z}";
					chunk.Initialise();
				}
			}

			UpdateChunks();

		}

		public override void _Process(double delta) {

			if (Input.IsPhysicalKeyPressed(Key.End)) {
				UpdateChunks();
			}

		}

		public void UpdateChunks() {
			foreach(KeyValuePair<Vector3, Chunk> chunk in Chunks) {
					chunk.Value.UpdateMesh();
			}
		}

		public bool AddChunk(Chunk chunk) {

			if (Chunks.ContainsKey(chunk.PositionKey)) {
				return false;
			}

			Chunks.Add(chunk.PositionKey, chunk);
			return true;

		}

		public Chunk GetChunkAtPos(Vector3 position) {
			
			Vector3 pos = new Vector3();
			pos.X = Mathf.Floor(position.X / Chunk.WIDTH) * Chunk.WIDTH;
			pos.Y = Mathf.Floor(position.Y / Chunk.HEIGHT) * Chunk.HEIGHT;
			pos.Z = Mathf.Floor(position.Z / Chunk.WIDTH) * Chunk.WIDTH;
			if (Chunks.ContainsKey(pos)) {
				return Chunks[pos];
			}
			GD.Print("Chunk is null " + pos);
			return null;

		}

		public bool IsBlockSolid(Vector3 position) {

			Chunk chunk = GetChunkAtPos(position);
			if (chunk == null) return false;
			return chunk.IsBlockSolid(position);

		}

		public BlockData GetBlock(Vector3 globalPosition) {
			
			Chunk chunk = GetChunkAtPos (globalPosition);
			if (chunk == null) {
				return null;
			}
			return chunk.GetBlock(chunk.ConvertToLocalPos(globalPosition));

		}

	}
}