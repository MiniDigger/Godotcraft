using System;
using Godot;
using Godotcraft.scripts.network.protocol;
using Godotcraft.scripts.network.protocol.handshake;
using Godotcraft.scripts.network.protocol.login.client;
using Godotcraft.scripts.network.protocol.login.server;
using Godotcraft.scripts.network.protocol.play.client;
using Godotcraft.scripts.network.protocol.play.server;
using Godotcraft.scripts.network.protocol.status.client;
using Godotcraft.scripts.network.protocol.status.server;
using Godotcraft.scripts.state;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Array = Godot.Collections.Array;

namespace Godotcraft.scripts.screens {
public class ServerListScreen : Control {
	private VBoxContainer serverList;
	private PopupDialog addServerPopup;
	private LineEdit serverName;
	private LineEdit serverAddress;

	private RandomNumberGenerator random;

	public override void _Ready() {
		Input.SetMouseMode(Input.MouseMode.Visible);

		Button addButton = GetNode("CenterContainer/VBoxContainer/ButtonBar/Add") as Button;
		Button backButton = GetNode("CenterContainer/VBoxContainer/ButtonBar/Back") as Button;
		serverList = GetNode("CenterContainer/VBoxContainer/Servers") as VBoxContainer;

		addServerPopup = GetNode("AddServerPopup") as PopupDialog;
		Button popupAddButton = GetNode("AddServerPopup/HBoxContainer/ButtonBar/Add") as Button;
		Button popupBackButton = GetNode("AddServerPopup/HBoxContainer/ButtonBar/Back") as Button;
		serverName = GetNode("AddServerPopup/HBoxContainer/Name") as LineEdit;
		serverAddress = GetNode("AddServerPopup/HBoxContainer/Address") as LineEdit;

		addButton?.Connect("pressed", this, nameof(onAdd));
		backButton?.Connect("pressed", this, nameof(onBack));
		popupAddButton?.Connect("pressed", this, nameof(onPopupAdd));
		popupBackButton?.Connect("pressed", this, nameof(onPopupBack));

		random = new RandomNumberGenerator();
		random.Randomize();

		// remove visual indicators we have in editor
		while (serverList?.GetChildCount() > 0) {
			Node child = serverList.GetChild(0);
			serverList.RemoveChild(child);
			child.QueueFree();
		}

		// load servers from server manager
		foreach (Server server in SingletonHandler.instance.serverManager.getServers()) {
			addServer(server.name, server.host, server.port);
		}
	}

	private void removeServer(String name) {
		Node node = serverList.GetNode(name);
		if (node != null) {
			serverList.RemoveChild(node);
		}
	}

	public void onPopupAdd() {
		String name = serverName.Text;
		String host = serverAddress.Text;
		ushort port = 25565;
		// add to serverlist
		SingletonHandler.instance.serverManager.addServer(new Server(name, host, port));
		// add to gui
		addServer(name, host, port);
	}

	private void addServer(String name, String host, ushort port) {
		// hide popup
		addServerPopup.Hide();
		// add dummy
		addServer(name, "", "", -1, -1);
		// do server list ping
		SingletonHandler.instance.gameClient.serverListPing(host, port, response => {
			removeServer(name);
			// todo favicon + motd
			addServer(name, "motd1", "motd2", response.players.online, response.players.max);
		});
	}

	private void addServer(String name, String motd1, String motd2, int current, int max) {
		HBoxContainer hBoxContainer = new HBoxContainer {Name = name};
		// TODO server icon
		TextureRect icon = new TextureRect();
		NoiseTexture texture = new NoiseTexture {Width = 64, Height = 64, Noise = new OpenSimplexNoise {Seed = (int) random.Randi()}};
		icon.Texture = texture;

		Label label = new Label {Text = name + " (" + current + "/" + max + ")\n" + motd1 + "\n" + motd2};
		hBoxContainer.AddChild(icon);
		hBoxContainer.AddChild(label);
		serverList.AddChild(hBoxContainer);

		hBoxContainer.Connect("gui_input", this, nameof(serverInput), new Array {name});
	}

	public void serverInput(InputEvent @event, String name) {
		if (@event is InputEventMouseButton mouseEvent) {
			if (mouseEvent.Pressed && mouseEvent.ButtonIndex == (int) ButtonList.Left) {
				joinServer(name);
			}
		}
	}

	public void joinServer(String name) {
		GD.Print("serverClick " + name);
		Server server = SingletonHandler.instance.serverManager.getServer(name);
		if (server == null) {
			GD.Print("unknown server " + name);
			return;
		}

		SingletonHandler.instance.stateHandler.currentServer = server;
		GetTree().ChangeScene("res://screens/GameScreen.tscn");
	}

	public void onBack() {
		GetTree().ChangeScene("res://screens/MainMenu.tscn");
	}

	public void onPopupBack() {
		addServerPopup.Hide();
	}

	public void onAdd() {
		addServerPopup.PopupCentered();
	}
}
}