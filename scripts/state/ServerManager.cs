using System;
using Godot.Collections;

namespace Godotcraft.scripts.state {
public class ServerManager {

	private Dictionary<String, Server> servers = new Dictionary<String, Server>();

	public Server getServer(String name) {
		return servers[name];
	}

	public void addServer(Server server) {
		servers[server.name] = server;
	}

	public void removeServer(String server) {
		servers.Remove(server);
	}
}
}