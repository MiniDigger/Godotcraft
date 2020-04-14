using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Godotcraft.scripts.network.protocol.play.server;
using Godotcraft.scripts.renderer;
using Godotcraft.scripts.state;
using Godotcraft.scripts.util;
using Thread = System.Threading.Thread;

namespace Godotcraft.scripts.world {
public class ChunkHandler : Spatial {
	private readonly BlockingCollection<ChunkDataPacket> creationQueue = new BlockingCollection<ChunkDataPacket>();
	private readonly BlockingCollection<Node> spawningQueue = new BlockingCollection<Node>();

	private bool shouldQuit = false;
	private int threadTarget = 10;
	private List<Thread> creationThreads = new List<Thread>();
	
	public override void _Ready() {
		SingletonHandler.instance.chunkHandler = this;

		// initial thread
		startNewThread();
	}

	public void handle(ChunkDataPacket packet) {
		if (packet.chunkData.getSectionCount() == 0) return;
		creationQueue.Add(packet);
		// GD.Print("add new packet, size is now " + creationQueue.Count);
	}

	private bool shouldStartNewThread() {
		return creationThreads.Count >= threadTarget;
	}

	private void startNewThread() {
		Thread creationThread = new Thread(doCreation);
		creationThread.Start();
		creationThreads.Add(creationThread);
	}

	public void doCreation() {
		while (IsProcessing() && !shouldQuit) {
			ChunkDataPacket packet;
			if (creationQueue.TryTake(out packet, 10)) {
				var watch = System.Diagnostics.Stopwatch.StartNew();
				if (packet.chunkData.getSectionCount() == 0) continue;
				if (GetNodeOrNull<Node>("Chunk" + packet.chunkPos) != null) {
					GD.Print("Chunk already existed!");
					continue;
				}
				Node chunk = new Node {Name = "Chunk" + packet.chunkPos};
				for (var i = 0; i < packet.chunkData.getSectionCount(); i++) {
					ChunkSection section = packet.chunkData.getSection(i);
					if(section == null || section.isEmpty()) continue;
					ChunkRenderer renderer = new ChunkRenderer {
						Translation = calcPos(packet.chunkPos, i),
						Name = "Section@" + i
					};
					// Timeout.TimeoutAfter(() => {
						bool created = renderer.createMesh(section);
						// if (created) renderer.createCollision();
						// return created;
					// }, TimeSpan.FromSeconds(1));
					chunk.AddChild(renderer);
				}

				if (!spawningQueue.TryAdd(chunk, 100)) {
					GD.Print("failed");
				}

				// create new threads delayed
				if (shouldStartNewThread()) {
					startNewThread();
				}
				
				watch.Stop();
				// GD.Print("handled packet, size is now " + creationQueue.Count + ", took " + watch.ElapsedMilliseconds);
			}
		}

		GD.Print("out!");
	}

	public override void _Notification(int what) {
		if (what == MainLoop.NotificationWmQuitRequest) {
			GD.Print("request quit");
			shouldQuit = true;
			foreach (var creationThread in creationThreads) {
				creationThread.Interrupt();
				creationThread.Abort();
				GD.Print("Alive? " + creationThread.IsAlive);
			}
		}
	}

	private Vector3 calcPos(Vector2 chunkPos, int section) {
		return new Vector3((int) chunkPos.x << 4, section * 16, (int) chunkPos.y << 4);
	}

	public override void _Process(float delta) {
		while (spawningQueue.Count > 0) {
			AddChild(spawningQueue.Take());
		}
	}
}
}