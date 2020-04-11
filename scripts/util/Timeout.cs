using System;
using System.Threading.Tasks;
using Godot;

namespace Godotcraft.scripts.util {
public static class Timeout {
	public static TResult TimeoutAfter<TResult>(
		this Func<TResult> func, TimeSpan timeout) {
		var task = Task.Run(func);
		return TimeoutAfterAsync(task, timeout).GetAwaiter().GetResult();
	}

	private static async Task<TResult> TimeoutAfterAsync<TResult>(
		this Task<TResult> task, TimeSpan timeout) {
		var result = await Task.WhenAny(task, Task.Delay(timeout));
		if (result == task) {
			// Task completed within timeout.
			return task.GetAwaiter().GetResult();
		}
		else {
			// Task timed out.
			throw new TimeoutException();
		}
	}
}
}