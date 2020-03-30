using Godot;
using Godotcraft.scripts.network.protocol;
using Godotcraft.scripts.network.protocol.handshake;
using Godotcraft.scripts.network.protocol.status.client;

namespace Godotcraft.scripts.screens {
public class ServerListScreen : Control {
	
	public override void _Ready() {
		Button addButton = GetNode("CenterContainer/VBoxContainer/ButtonBar/Add") as Button;
		Button backButton = GetNode("CenterContainer/VBoxContainer/ButtonBar/Back") as Button;

		addButton?.Connect("pressed", this, nameof(onAdd));
		backButton?.Connect("pressed", this, nameof(onBack));
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
	}

	public void onBack() {
		GetTree().ChangeScene("res://screens/MainMenu.tscn");
	}
}
}