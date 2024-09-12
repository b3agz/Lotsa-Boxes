using Godot;
using Godot.NativeInterop;

namespace LotsaBoxes.Data {

    public static class MeshRepository {

        public static Vector3I[] NeighbourOffset = new Vector3I[] {
            new (0,0,1),
            new (0,0,-1),
            new (1,0,0),
            new (-1,0,0),
            new (0,1,0),
            new (0,-1,0),
        };

        public static int[] InverseNeighbourOffset = new int[] {
            1, 0, 3, 2, 5, 4
        };

        public static ShapeData Cube = new ShapeData (
            "Cube",
            new MeshData (  // Front Face
            new VertData[] {
                new VertData(new Vector3(0,0,0), new Vector3(0,0,1), new Vector2(0,0)),
                new VertData(new Vector3(0,1,0), new Vector3(0,0,1), new Vector2(0,1)),
                new VertData(new Vector3(-1,1,0), new Vector3(0,0,1), new Vector2(1,1)),
                new VertData(new Vector3(-1,0,0), new Vector3(0,0,1), new Vector2(1,0))
            },
            new int[] {2,1,0,0,3,2}
            ),

            new MeshData (  // Back Face
            new VertData[] {
                new VertData(new Vector3(-1,0,-1), new Vector3(0,0,-1), new Vector2(0,0)),
                new VertData(new Vector3(-1,1,-1), new Vector3(0,0,-1), new Vector2(0,1)),
                new VertData(new Vector3(0,1,-1), new Vector3(0,0,-1), new Vector2(1,1)),
                new VertData(new Vector3(0,0,-1), new Vector3(0,0,-1), new Vector2(1,0))
            },
            new int[] {2,1,0,0,3,2}
            ),
            
            new MeshData (  // Left Face
            new VertData[] {
                new VertData(new Vector3(0,0,-1), new Vector3(-1,0,0), new Vector2(0,0)),
                new VertData(new Vector3(0,1,-1), new Vector3(-1,0,0), new Vector2(0,1)),
                new VertData(new Vector3(0,1,0), new Vector3(-1,0,0), new Vector2(1,1)),
                new VertData(new Vector3(0,0,0), new Vector3(-1,0,0), new Vector2(1,0))
            },
            new int[] {2,1,0,0,3,2}
            ),
            
            new MeshData (  // Right Face
            new VertData[] {
                new VertData(new Vector3(-1,0,0), new Vector3(1,0,0), new Vector2(0,0)),
                new VertData(new Vector3(-1,1,0), new Vector3(1,0,0), new Vector2(0,1)),
                new VertData(new Vector3(-1,1,-1), new Vector3(1,0,0), new Vector2(1,1)),
                new VertData(new Vector3(-1,0,-1), new Vector3(1,0,0), new Vector2(1,0))
            },
            new int[] {2,1,0,0,3,2}
            ),
            
            new MeshData (  // Top Face
            new VertData[] {
                new VertData(new Vector3(0,1,0), new Vector3(0,1,0), new Vector2(0,0)),
                new VertData(new Vector3(0,1,-1), new Vector3(0,1,0), new Vector2(0,1)),
                new VertData(new Vector3(-1,1,-1), new Vector3(0,1,0), new Vector2(1,1)),
                new VertData(new Vector3(-1,1,0), new Vector3(0,1,0), new Vector2(1,0))
            },
            new int[] {2,1,0,0,3,2}
            ),
            
            new MeshData (  // Bottom Face
            new VertData[] {
                new VertData(new Vector3(-1,0,0), new Vector3(0,-1,0), new Vector2(0,0)),
                new VertData(new Vector3(-1,0,-1), new Vector3(0,-1,0), new Vector2(0,1)),
                new VertData(new Vector3(0,0,-1), new Vector3(0,-1,0), new Vector2(1,1)),
                new VertData(new Vector3(0,0,0), new Vector3(0,-1,0), new Vector2(1,0))
            },
            new int[] {2,1,0,0,3,2}
            )
        );
    }

    public struct ShapeData {

        public string Name;
        public MeshData[] Sides;

        public ShapeData (string name, MeshData front, MeshData back, MeshData left, MeshData right, MeshData top, MeshData bottom) {

            Name = name;
            Sides = new MeshData[6];
            Sides[0] = front;
            Sides[1] = back;
            Sides[2] = left;
            Sides[3] = right;
            Sides[4] = top;
            Sides[5] = bottom;

        }

    }

    public struct MeshData {

        public VertData[] Verts;
        public int[] Indices;

        public MeshData (VertData[] verts, int[] indices) {
            Verts = verts;
            Indices = indices;
        }
    }

    public struct VertData {

        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector2 UV { get; private set; }

        public VertData (Vector3 position, Vector3 normal, Vector2 uv) {
            Position = position;
            Normal = normal;
            UV = uv;
        }
    }

}
