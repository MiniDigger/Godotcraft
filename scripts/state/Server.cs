using System;

namespace Godotcraft.scripts.state {
public class Server {

	public String name { get; }
	public int curr { get; }
	public int max { get; }
	public String motd { get; }

	public Server(string name, int curr, int max, string motd) {
		this.name = name;
		this.curr = curr;
		this.max = max;
		this.motd = motd;
	}
}
}