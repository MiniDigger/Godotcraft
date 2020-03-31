using System;
using Godot;
using Godotcraft.scripts.network.protocol;
using Godotcraft.scripts.network.protocol.handshake;
using Godotcraft.scripts.network.protocol.status.client;

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
	}

	public void onAdd() {
		StreamPeerTCP stream = new StreamPeerTCP();
		Error status = stream.ConnectToHost("localhost", 25565);
		GD.Print("Connected with status: " + status);
		status = Util.writePacket(new HandshakePacket(578, "localhost", 25565, PacketState.STATUS), stream);
		GD.Print("Written with status: " + status);
		status = Util.writePacket(new StatusRequestPacket(), stream);
		GD.Print("Written with status: " + status);
		status = Util.writePacket(new PingPacket(), stream);
		GD.Print("Written with status: " + status);

		Util.readPacket(stream);
		Util.readPacket(stream);

		addServer("Test", "Motd1", "Motd2", 0, 10);
		addServer("Test", "Motd1", "Motd2", 0, 10);
		addServer("Test", "Motd1", "Motd2", 0, 10);
		addServer("Test", "Motd1", "Motd2", 0, 10);
		addServer("Test", "Motd1", "Motd2", 0, 10);
	}

	public void onBack() {
		GetTree().ChangeScene("res://screens/MainMenu.tscn");
	}
}
}