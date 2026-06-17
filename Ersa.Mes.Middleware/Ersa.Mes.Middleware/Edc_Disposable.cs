using System;

namespace Ersa.Mes.Middleware;

public static class Edc_Disposable
{
	private class Edc_Subscription : Edc_DisposableObject
	{
		private readonly Action m_delUnsubcriptionAction;

		public Edc_Subscription(Action i_delUnsubcriptionAction)
		{
			m_delUnsubcriptionAction = i_delUnsubcriptionAction;
		}

		protected override void Sub_InternalDispose()
		{
			if (m_delUnsubcriptionAction != null)
			{
				m_delUnsubcriptionAction();
			}
		}
	}

	public static IDisposable Fun_fdcCreate(Action i_delAction)
	{
		return new Edc_Subscription(i_delAction);
	}

	public static IDisposable Fun_fdcEmpty()
	{
		return new Edc_Subscription(delegate
		{
		});
	}
}
