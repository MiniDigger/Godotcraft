using Godot;
using Godotcraft.scripts.world;

namespace Godotcraft.test {
public class MeshInstanceV2 : Godot.MeshInstance {
	const float CUBE_SIZE = 0.5f;

	private readonly SurfaceTool tool = new SurfaceTool();

	public override void _Ready() {
		VisualServer.SetDebugGenerateWireframes(true);
		ChunkData chunkData = new ChunkData();
		Mesh = createMesh(chunkData.getSection(0));
	}

	public Mesh createMesh(ChunkSection section) {
		tool.Begin(Mesh.PrimitiveType.Triangles);

		for (var x = 0; x < 16; x++) {
			for (var y = 0; y < 16; y++) {
				for (var z = 0; z < 16; z++) {
					// get data and check if air
					int data = getData(section, x, y, z);
					if (data == 0) {
						continue;
					}

					// culling
					bool renderFront = true;
					if (z < 16 - 1) renderFront = getData(section, x, y, z + 1) == 0;
					bool renderBack = true;
					if (z > 0) renderBack = getData(section, x, y, z - 1) == 0;
					bool renderRight = true;
					if (x < 16 - 1) renderRight = getData(section, x + 1, y, z) == 0;
					bool renderLeft = true;
					if (x > 0) renderLeft = getData(section, x - 1, y, z) == 0;
					bool renderTop = true;
					if (y < 16 - 1) renderTop = getData(section, x, y + 1, z) == 0;
					bool renderBot = true;
					if (y > 0) renderBot = getData(section, x, y - 1, z) == 0;

					// add cube
					createCube(x, y, z, data, renderFront, renderBack, renderRight, renderLeft, renderTop, renderBot);
				}
			}
		}

		tool.Index();

		return tool.Commit();
	}

	private int getData(ChunkSection section, int x, int y, int z) {
		// int data = section.get(ChunkData.index(x, y, z));
		int data = 1;
		if (x % 4 == 1) {
			data = 1;
		}

		if (z % 4 == 1) {
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

	public void createCube(int x, int y, int z, int data, bool renderFront, bool renderBack, bool renderRight, bool renderLeft, bool renderTop,
		bool renderBot) {
		var p1 = new Vector3(x - CUBE_SIZE, y - CUBE_SIZE, z + CUBE_SIZE);
		var p2 = new Vector3(x + CUBE_SIZE, y - CUBE_SIZE, z + CUBE_SIZE);
		var p3 = new Vector3(x + CUBE_SIZE, y + CUBE_SIZE, z + CUBE_SIZE);
		var p4 = new Vector3(x - CUBE_SIZE, y + CUBE_SIZE, z + CUBE_SIZE);
		var p5 = new Vector3(x + CUBE_SIZE, y - CUBE_SIZE, z - CUBE_SIZE);
		var p6 = new Vector3(x - CUBE_SIZE, y - CUBE_SIZE, z - CUBE_SIZE);
		var p7 = new Vector3(x - CUBE_SIZE, y + CUBE_SIZE, z - CUBE_SIZE);
		var p8 = new Vector3(x + CUBE_SIZE, y + CUBE_SIZE, z - CUBE_SIZE);

		if (renderFront) {
			tool.AddNormal(new Vector3(0, 0, 1));
			tool.AddVertex(p1);
			tool.AddVertex(p2);
			tool.AddVertex(p3);

			tool.AddVertex(p1);
			tool.AddVertex(p3);
			tool.AddVertex(p4);
		}

		if (renderBack) {
			tool.AddNormal(new Vector3(0, 0, -1));
			tool.AddVertex(p5);
			tool.AddVertex(p6);
			tool.AddVertex(p7);

			tool.AddVertex(p5);
			tool.AddVertex(p7);
			tool.AddVertex(p8);
		}

		if (renderRight) {
			tool.AddNormal(new Vector3(-1, 0, 0));
			tool.AddVertex(p2);
			tool.AddVertex(p5);
			tool.AddVertex(p8);

			tool.AddVertex(p2);
			tool.AddVertex(p8);
			tool.AddVertex(p3);
		}

		if (renderLeft) {
			tool.AddNormal(new Vector3(1, 0, 0));
			tool.AddVertex(p6);
			tool.AddVertex(p1);
			tool.AddVertex(p4);

			tool.AddVertex(p6);
			tool.AddVertex(p4);
			tool.AddVertex(p7);
		}

		if (renderTop) {
			tool.AddNormal(new Vector3(0, -1, 0));
			tool.AddVertex(p4);
			tool.AddVertex(p3);
			tool.AddVertex(p8);

			tool.AddVertex(p4);
			tool.AddVertex(p8);
			tool.AddVertex(p7);
		}

		if (renderBot) {
			tool.AddNormal(new Vector3(0, 1, 0));
			tool.AddVertex(p6);
			tool.AddVertex(p5);
			tool.AddVertex(p2);

			tool.AddVertex(p6);
			tool.AddVertex(p2);
			tool.AddVertex(p1);
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventKey && Input.IsKeyPressed((int) KeyList.P)) {
			GetViewport().DebugDraw = (Viewport.DebugDrawEnum) (((int) GetViewport().DebugDraw + 1) % 4);
		}
	}
}
}