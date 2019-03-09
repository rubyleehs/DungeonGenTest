using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshGen
{
    public class GridMeshGen : MonoBehaviour
    {
        protected static List<Vector3> vertices;
        protected static List<int> triangles;
        protected static List<Vector2> uvs;

        private static Vector2 uvPerCell;
        private static Vector2 cellSize;

        public static Mesh CreateNewGrid(Vector2Int indexSize, Vector2 _cellSize, Vector2 southWestCorner, ref Transform meshPositioner)
        {
            Mesh mesh = new Mesh();
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uvs = new List<Vector2>();

            uvPerCell = new Vector2(1f / indexSize.x, 1f / indexSize.y);
            cellSize = _cellSize;

            for (int y = 0; y < indexSize.y; y++)
            {
                for (int x = 0; x < indexSize.x; x++)
                {
                    TriangulateCell(new Vector2Int(x, y));
                }
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();

            meshPositioner.position = new Vector3(-cellSize.x * 0.5f, -cellSize.y * 0.5f, meshPositioner.position.z) + (Vector3)southWestCorner;
            return mesh;
        }

        protected static void TriangulateCell(Vector2Int sw) //, MapNode se, MapNode nw, MapNode ne)
        {
            int _vertexIndex = vertices.Count;

            AddQuad((Vector2)sw * cellSize, (sw + Vector2.up) * cellSize, (sw + Vector2.one) * cellSize, (sw + Vector2.right) * cellSize);

            uvs.Add(Vector2.Scale(sw, uvPerCell));
            uvs.Add(Vector2.Scale(sw + Vector2.up, uvPerCell));
            uvs.Add(Vector2.Scale(sw + Vector2.one, uvPerCell));
            uvs.Add(Vector2.Scale(sw + Vector2.right, uvPerCell));
        }

        protected static void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }
    }
}
