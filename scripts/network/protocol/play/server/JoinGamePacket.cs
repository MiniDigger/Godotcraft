using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class JoinGamePacket : Packet {
	public int entityId { get; private set; }
	public byte gamemode { get; private set; }
	public int dimension { get; private set; }
	public long seed { get; private set; }
	public byte maxPlayers { get; private set; }
	public String levelType { get; private set; }
	public int viewDistance { get; private set; }
	public bool reducedDebugInfo { get; private set; }
	public bool enableRespawnScreen { get; private set; }

	public JoinGamePacket() : base(PacketType.ToClient.Play.joinGame) { }


	public override void read(DataTypes dataTypes, List<byte> data) {
		entityId = dataTypes.ReadNextInt(data);
		gamemode = dataTypes.ReadNextByte(data);
		dimension = dataTypes.ReadNextInt(data);
		seed = dataTypes.ReadNextLong(data);
		maxPlayers = dataTypes.ReadNextByte(data);
		levelType = dataTypes.ReadNextString(data);
		viewDistance = dataTypes.ReadNextVarInt(data);
		reducedDebugInfo = dataTypes.ReadNextBool(data);
		enableRespawnScreen = dataTypes.ReadNextBool(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}

	public override string ToString() {
		return
			$"{base.ToString()}, {nameof(entityId)}: {entityId}, {nameof(gamemode)}: {gamemode}, {nameof(dimension)}: {dimension}, {nameof(seed)}: {seed}, {nameof(maxPlayers)}: {maxPlayers}, {nameof(levelType)}: {levelType}, {nameof(viewDistance)}: {viewDistance}, {nameof(reducedDebugInfo)}: {reducedDebugInfo}, {nameof(enableRespawnScreen)}: {enableRespawnScreen}";
	}
}
}