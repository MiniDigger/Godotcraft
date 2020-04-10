using Godot;
using Godotcraft.scripts.objects;
using Godotcraft.scripts.world;

namespace Godotcraft.scripts.renderer {
public class ChunkRenderer : Godot.MeshInstance {
	const float CUBE_SIZE = 0.5f;

	private readonly SurfaceTool tool = new SurfaceTool();

	public override void _Ready() {
		VisualServer.SetDebugGenerateWireframes(true);
		ChunkData chunkData = new ChunkData();
		MaterialOverride = new SpatialMaterial {AlbedoTexture = TextureAtlas.instance.atlas, ParamsCullMode = SpatialMaterial.CullMode.Back};
		Mesh = createMesh(chunkData.getSection(0));
		createCollision(Mesh);
	}

	public void createCollision(Mesh mesh) {
		StaticBody staticBody = new StaticBody();
		ConcavePolygonShape shape = new ConcavePolygonShape {Data = mesh.GetFaces()};
		CollisionShape collisionShape = new CollisionShape {Shape = shape};
		staticBody.AddChild(collisionShape);
		AddChild(staticBody);
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
		if (z % 4 == 2) {
			data = 3;
		}

		if (z % 4 == 1) {
			data = 2;
		}

		if (y > 1 || y < 0 || x % 4 == 0 || z % 4 == 0) {
			data = 0;
		}

		return data;
	}

	public void createCube(int x, int y, int z, int data, bool renderFront, bool renderBack, bool renderRight, bool renderLeft, bool renderTop,
		bool renderBot) {
		var p001 = new Vector3(x - CUBE_SIZE, y - CUBE_SIZE, z + CUBE_SIZE);
		var p101 = new Vector3(x + CUBE_SIZE, y - CUBE_SIZE, z + CUBE_SIZE);
		var p111 = new Vector3(x + CUBE_SIZE, y + CUBE_SIZE, z + CUBE_SIZE);
		var p011 = new Vector3(x - CUBE_SIZE, y + CUBE_SIZE, z + CUBE_SIZE);
		var p100 = new Vector3(x + CUBE_SIZE, y - CUBE_SIZE, z - CUBE_SIZE);
		var p000 = new Vector3(x - CUBE_SIZE, y - CUBE_SIZE, z - CUBE_SIZE);
		var p010 = new Vector3(x - CUBE_SIZE, y + CUBE_SIZE, z - CUBE_SIZE);
		var p110 = new Vector3(x + CUBE_SIZE, y + CUBE_SIZE, z - CUBE_SIZE);

		if (renderFront) {
			tool.AddNormal(new Vector3(0, 0, -1));
			addTri(p001, p101, p111, p011, data);
		}

		if (renderBack) {
			tool.AddNormal(new Vector3(0, 0, 1));
			addTri(p100, p000, p010, p110, data);
		}

		if (renderRight) {
			tool.AddNormal(new Vector3(1, 0, 0));
			addTri(p101, p100, p110, p111, data);
		}

		if (renderLeft) {
			tool.AddNormal(new Vector3(-1, 0, 0));
			addTri(p000, p001, p011, p010, data);
		}

		if (renderTop) {
			tool.AddNormal(new Vector3(0, 1, 0));
			addTri(p011, p111, p110, p010, data);
		}

		if (renderBot) {
			tool.AddNormal(new Vector3(0, -1, 0));
			addTri(p000, p100, p101, p001, data);
		}
	}

	private void addTri(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int data) {
		// UVMap uv = UVMap.getMap("aa_test");
		UVMap uv = UVMap.getMap("dirt");
		if (data == 2) {
			uv = UVMap.getMap("bedrock");
		} else if (data == 3) {
			uv = UVMap.getMap("jungle_planks");
		}

		tool.AddUv(uv.uvMap[0]);
		tool.AddVertex(p4);
		tool.AddUv(uv.uvMap[2]);
		tool.AddVertex(p3);
		tool.AddUv(uv.uvMap[3]);
		tool.AddVertex(p2);

		tool.AddUv(uv.uvMap[0]);
		tool.AddVertex(p4);
		tool.AddUv(uv.uvMap[3]);
		tool.AddVertex(p2);
		tool.AddUv(uv.uvMap[1]);
		tool.AddVertex(p1);
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventKey && Input.IsKeyPressed((int) KeyList.P) && !Console.instance.isConsoleShown) {
			GetViewport().DebugDraw = (Viewport.DebugDrawEnum) (((int) GetViewport().DebugDraw + 1) % 4);
		}
	}
}
}