using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godotcraft.scripts.network.protocol.status.server;
using Newtonsoft.Json;

namespace Godotcraft.scripts.state {
public class ServerManager {
	public static readonly String serverFilePath = "user://servers.json";

	private Dictionary<String, Server> servers = new Dictionary<String, Server>();

	public Server getServer(String name) {
		return servers[name];
	}

	public void addServer(Server server) {
		servers[server.name] = server;
		save();
	}

	public void removeServer(String server) {
		servers.Remove(server);
		save();
	}

	public Dictionary<string, Server>.ValueCollection getServers() {
		return servers.Values;
	}

	public void save() {
		String json = JsonConvert.SerializeObject(getServers().ToList());

		File serverFile = new File();
		serverFile.Open(serverFilePath, File.ModeFlags.Write);
		serverFile.StoreLine(json);
		serverFile.Close();
	}

	public void load() {
		File serverFile = new File();
		if (!serverFile.FileExists(serverFilePath)) {
			GD.Print("Cant read serverFile as it doesnt exist");
			return;
		}

		serverFile.Open(serverFilePath, File.ModeFlags.Read);

		String content = serverFile.GetAsText();

		servers.Clear();
		List<Server> loadedServers = JsonConvert.DeserializeObject<List<Server>>(content);
		foreach (Server loadedServer in loadedServers) {
			servers[loadedServer.name] = loadedServer;
		}
		
		serverFile.Close();
	}
}
}