namespace Ersa.Mes.Middleware.Interface;

public interface Inf_MesFunctionContainer
{
	void Sub_AddObject<T>(object i_objObject);

	T Fun_edcGetObject<T>() where T : class;

	void Sub_RemoveObject<T>();
}
