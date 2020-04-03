using System;

namespace Godotcraft.scripts.state {
public class Server {

	public String name { get; }
	public String host { get; }
	public ushort port { get; }

	public Server(String name, String host, ushort port) {
		this.name = name;
		this.host = host;
		this.port = port;
	}
}
}