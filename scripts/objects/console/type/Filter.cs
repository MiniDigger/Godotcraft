using System.Collections.Generic;

namespace Godotcraft.scripts.objects.console.type {
public class Filter : BaseType {
	public enum MODE {
		ALLOW,
		DENY
	}

	public List<object> filterList { get; }
	public MODE mode;

	public Filter(List<object> filterList, MODE mode = MODE.ALLOW) : base("Filter") {
		this.filterList = filterList;
		this.mode = mode;
	}

	public override CHECK check(object originalValue) {
		if ((mode == MODE.ALLOW && filterList.Contains(originalValue)) ||
		    (mode == MODE.DENY && !filterList.Contains(originalValue))) {
			return CHECK.OK;
		}

		return CHECK.CANCELED;
	}

	public override void normalized(object originalValue) {
		_normalizedValue = originalValue;
	}
}
}