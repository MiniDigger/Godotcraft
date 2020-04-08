using System;
using Godot;
using Godot.Collections;
using Godotcraft.scripts.world;

public class MeshInstance : Godot.MeshInstance {
	public MeshInstance() {
		VisualServer.SetDebugGenerateWireframes(true);
	}

	Array<Vector3> arrayQuadVertices = new Array<Vector3>();
	Array<int> arrayQuadIndices = new Array<int>();

	Dictionary<Vector3, int> dictionaryCheckQuadVertices = new Dictionary<Vector3, int>();
	Dictionary<Vector3, Color> colors = new Dictionary<Vector3, Color>();

	const float CUBE_SIZE = 0.5f;
	
	SurfaceTool surfaceTool = new SurfaceTool();

	public override void _Ready() {
		// GetViewport().DebugDraw = Viewport.DebugDrawEnum.Wireframe;

		ChunkData chunkData = new ChunkData();

		makeCubes(chunkData.getSection(0));
	}

	private int getData(ChunkSection section, int x, int y, int z) {
		// int data = section.get(ChunkData.index(x, y, z));
		int data = 1;
		if (x % 4 == 1) {
			data = 1;
		}
		if (y % 4 == 1) {
			data = 2;
		}
		if (y > 1 || y < 0 || x % 4 == 0 || z % 4 == 0) {
			data = 0;
		}

		// switch (x) {
		// 	case 0 when y == 0 && z == 0:
		// 	case 0 when y == 0 && z == 1:
		// 		return 1;
		// 	default:
		// 		return 0;
		// }
		
		return data;
	}

	private int calcNeighbors(ChunkSection section, int x, int y, int z) {
		int self = getData(section, x + 0, y + 0, z + 0);
		if (self == 0) {
			return 0;
		}

		int data = 0;

		int south = getData(section, x + 0, y - 1, z + 0);
		int north = getData(section, x + 0, y + 1, z + 0);

		int down = getData(section, x + 0, y + 0, z - 1);
		int up = getData(section, x + 0, y + 0, z + 1);

		int right = getData(section, x - 1, y + 0, z + 0);
		int left = getData(section, x + 1, y + 0, z + 0);

		if (south == 0) {
			data |= 1 << 1;
		}

		if (north == 0) {
			data |= 1 << 2;
		}

		if (down == 0) {
			data |= 1 << 3;
		}

		if (up == 0) {
			data |= 1 << 4;
		}

		if (right == 0) {
			data |= 1 << 5;
		}

		if (left == 0) {
			data |= 1 << 6;
		}

		return data;
	}

	public void makeCubes(ChunkSection section) {
		surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
		
		for (var x = 0; x < 16; x++) {
			for (var y = 0; y < 16; y++) {
				for (var z = 0; z < 16; z++) {
					int data = getData(section, x, y, z);
					int neighborData = calcNeighbors(section, x, y, z);
					makeCube(data, neighborData, x, y, z);
				}
			}
		}
		
		foreach (var vertex in arrayQuadVertices) {
			surfaceTool.AddColor(colors[vertex]);
			// normals sind doof, das ist nur von oben, von unten fehlt
			surfaceTool.AddNormal(new Vector3(0, -1, 0));
			surfaceTool.AddVertex(vertex);
		}

		foreach (var index in arrayQuadIndices) {
			surfaceTool.AddIndex(index);
		}
		
		// surfaceTool.GenerateNormals();

		Mesh = surfaceTool.Commit();
	}

	public void makeCube(int data, int neighborData, int x, int y, int z) {
		if (data == 0) {
			return;
		}

		var vertNorthTopRight = new Vector3(x - CUBE_SIZE, y + CUBE_SIZE, z + CUBE_SIZE);
		var vertNorthTopLeft = new Vector3(x + CUBE_SIZE, y + CUBE_SIZE, z + CUBE_SIZE);
		var vertNorthBottomLeft = new Vector3(x + CUBE_SIZE, y + CUBE_SIZE, z - CUBE_SIZE);
		var vertNorthBottomRight = new Vector3(x - CUBE_SIZE, y + CUBE_SIZE, z - CUBE_SIZE);

		var vertSouthTopRight = new Vector3(x - CUBE_SIZE, y - CUBE_SIZE, z + CUBE_SIZE);
		var vertSouthTopLeft = new Vector3(x + CUBE_SIZE, y - CUBE_SIZE, z + CUBE_SIZE);
		var vertSouthBottomLeft = new Vector3(x + CUBE_SIZE, y - CUBE_SIZE, z - CUBE_SIZE);
		var vertSouthBottomRight = new Vector3(x - CUBE_SIZE, y - CUBE_SIZE, z - CUBE_SIZE);

		if ((neighborData & (1 << 1)) != 0) addQuad(data, vertSouthTopRight, vertSouthTopLeft, vertSouthBottomLeft, vertSouthBottomRight);
		if ((neighborData & (1 << 2)) != 0) addQuad(data, vertNorthTopRight, vertNorthBottomRight, vertNorthBottomLeft, vertNorthTopLeft);

		if ((neighborData & (1 << 3)) != 0) addQuad(data, vertNorthBottomLeft, vertNorthBottomRight, vertSouthBottomRight, vertSouthBottomLeft);
		if ((neighborData & (1 << 4)) != 0) addQuad(data, vertNorthTopLeft, vertSouthTopLeft, vertSouthTopRight, vertNorthTopRight);

		if ((neighborData & (1 << 5)) != 0) addQuad(data, vertNorthTopRight, vertSouthTopRight, vertSouthBottomRight, vertNorthBottomRight);
		if ((neighborData & (1 << 6)) != 0) addQuad(data, vertNorthTopLeft, vertNorthBottomLeft, vertSouthBottomLeft, vertSouthTopLeft);
	}

	public void addQuad(int data, Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4) {
		var vertexIndexOne = getVertexIndex(data, point1);
		var vertexIndexTwo = getVertexIndex(data, point2);
		var vertexIndexThree = getVertexIndex(data, point3);
		var vertexIndexFour = getVertexIndex(data, point4);

		arrayQuadIndices.Add(vertexIndexOne);
		arrayQuadIndices.Add(vertexIndexTwo);
		arrayQuadIndices.Add(vertexIndexThree);

		arrayQuadIndices.Add(vertexIndexOne);
		arrayQuadIndices.Add(vertexIndexThree);
		arrayQuadIndices.Add(vertexIndexFour);
	}

	public int getVertexIndex(int data, Vector3 vertex) {
		if (dictionaryCheckQuadVertices.ContainsKey(vertex)) {
			return dictionaryCheckQuadVertices[vertex];
		}

		colors[vertex] = data switch {
			1 => new Color(1, 0, 0),
			2 => new Color(0, 1, 0),
			_ => new Color(0, 0, 1)
		};

		arrayQuadVertices.Add(vertex);

		dictionaryCheckQuadVertices[vertex] = arrayQuadVertices.Count - 1;
		return arrayQuadVertices.Count - 1;
	}

	// public override void _Input(InputEvent @event) {
	// 	if (@event is InputEventKey && Input.IsKeyPressed((int) KeyList.P)) {
	// 		GetViewport().DebugDraw = (Viewport.DebugDrawEnum) (((int) GetViewport().DebugDraw + 1) % 4);
	// 	}
	// }
}