using System;

namespace Ersa.Mes.PLC.Modules;

public abstract class Edc_DisposableObject : IDisposable
{
	private readonly object m_objLock = new object();

	private bool m_blnIstDisposed;

	public bool Pro_blnIstDisposed
	{
		get
		{
			lock (m_objLock)
			{
				return m_blnIstDisposed;
			}
		}
		private set
		{
			lock (m_objLock)
			{
				m_blnIstDisposed = value;
			}
		}
	}

	protected Edc_DisposableObject()
	{
		m_blnIstDisposed = false;
	}

	~Edc_DisposableObject()
	{
		Sub_Dispose(i_blnDiposing: false);
	}

	public void Dispose()
	{
		Sub_Dispose(i_blnDiposing: true);
		GC.SuppressFinalize(this);
	}

	protected abstract void Sub_InternalDispose();

	private void Sub_Dispose(bool i_blnDiposing)
	{
		if (!Pro_blnIstDisposed)
		{
			if (i_blnDiposing)
			{
				Sub_InternalDispose();
			}
			Pro_blnIstDisposed = true;
		}
	}
}
