using System;
using Godot;
using Godotcraft.scripts.network.protocol;
using Godotcraft.scripts.network.protocol.handshake;
using Godotcraft.scripts.network.protocol.status.client;
using Godotcraft.scripts.state;
using Array = Godot.Collections.Array;

namespace Godotcraft.scripts.screens {
public class ServerListScreen : Control {
	private VBoxContainer serverList;
	private RandomNumberGenerator random;

	public override void _Ready() {
		Button addButton = GetNode("CenterContainer/VBoxContainer/ButtonBar/Add") as Button;
		Button backButton = GetNode("CenterContainer/VBoxContainer/ButtonBar/Back") as Button;
		serverList = GetNode("CenterContainer/VBoxContainer/Servers") as VBoxContainer;

		addButton?.Connect("pressed", this, nameof(onAdd));
		backButton?.Connect("pressed", this, nameof(onBack));

		random = new RandomNumberGenerator();
		random.Randomize();

		while (serverList?.GetChildCount() > 0) {
			Node child = serverList.GetChild(0);
			serverList.RemoveChild(child);
			child.QueueFree();
		}
	}

	private void addServer(String name, String motd1, String motd2, int current, int max) {
		HBoxContainer hBoxContainer = new HBoxContainer();
		// TODO server icon
		TextureRect icon = new TextureRect();
		NoiseTexture texture = new NoiseTexture();
		texture.Width = 64;
		texture.Height = 64;
		texture.Noise = new OpenSimplexNoise();
		texture.Noise.Seed = (int) random.Randi();
		icon.Texture = texture;

		Label label = new Label {Text = name + " (" + current + "/" + max + ")\n" + motd1 + "\n" + motd2};
		hBoxContainer.AddChild(icon);
		hBoxContainer.AddChild(label);
		serverList.AddChild(hBoxContainer);

		hBoxContainer.Connect("gui_input", this, nameof(serverInput), new Array {name, hBoxContainer});

		Server server = new Server(name, current, max, motd1 + "\n" + motd2);
		SingletonHandler.instance.serverManager.addServer(server);
	}

	public void onAdd() {
		MinecraftClient minecraftClient = SingletonHandler.instance.client;
		Error status = minecraftClient.connect("localhost", 25565);
		GD.Print("Connected with status: " + status);
		minecraftClient.sendPacket(new HandshakePacket(578, "localhost", 25565, PacketState.STATUS));
		minecraftClient.sendPacket(new StatusRequestPacket());
		minecraftClient.sendPacket(new PingPacket());

		// minecraftClient.addPacketListener(PacketType.ToClient.Status.statusResponse);

		addServer("Test1", "Motd1", "Motd2", 0, 10);
		addServer("Test2", "Motd1", "Motd2", 0, 10);
		addServer("Test3", "Motd1", "Motd2", 0, 10);
		addServer("Test4", "Motd1", "Motd2", 0, 10);
		addServer("Test5", "Motd1", "Motd2", 0, 10);
	}

	public void serverInput(InputEvent @event, String name, HBoxContainer container) {
		if (@event is InputEventMouseButton mouseEvent) {
			if (mouseEvent.Pressed) {
				joinServer(name, container);
			}
		}
	}

	public void joinServer(String name, HBoxContainer container) {
		GD.Print("serverClick " + name);
	}

	public void onBack() {
		GetTree().ChangeScene("res://screens/MainMenu.tscn");
	}
}
}