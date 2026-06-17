using System;
using System.Threading.Tasks;

namespace Ersa.Mes.Common.Extensions;

public static class TaskExtend
{
	public static async Task Fun_fdcWithTimeout(this Task i_fdcTask, int i_i32Timeout, Action i_delOnTimeout)
	{
		Task fdcDelayTask = Task.Delay(i_i32Timeout);
		if (await Task.WhenAny(i_fdcTask, fdcDelayTask).ConfigureAwait(continueOnCapturedContext: true) == fdcDelayTask)
		{
			i_delOnTimeout();
		}
		await i_fdcTask.ConfigureAwait(continueOnCapturedContext: true);
	}

	public static async Task<T> Fun_fdcTimeoutAfterAsync<T>(this Task<T> i_fdcTask, int i_i32Timeout)
	{
		Task fdcDelayTask = Task.Delay(i_i32Timeout);
		if (await Task.WhenAny(i_fdcTask, fdcDelayTask).ConfigureAwait(continueOnCapturedContext: true) == fdcDelayTask)
		{
			return await i_fdcTask.ConfigureAwait(continueOnCapturedContext: true);
		}
		throw new TimeoutException();
	}
}
