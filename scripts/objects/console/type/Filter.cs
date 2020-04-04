using System.Collections.Generic;

namespace Godotcraft.scripts.objects.console.type {
public class Filter : BaseType {
	public enum MODE {
		ALLOW,
		DENY
	}

	public List<string> filterList { get; }
	public MODE mode;

	public Filter(List<string> filterList, MODE mode = MODE.ALLOW) : base("Filter", null) {
		this.filterList = filterList;
		this.mode = mode;
	}

	public override CHECK check(string originalValue) {
		if ((mode == MODE.ALLOW && filterList.Contains(originalValue)) ||
		    (mode == MODE.DENY && !filterList.Contains(originalValue))) {
			return CHECK.OK;
		}

		return CHECK.CANCELED;
	}

	public override void normalized(string originalValue) {
		_normalizedValue = originalValue;
	}
}
}