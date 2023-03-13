using CleanTaskExecuter.TaskController.Controller.Iface;

namespace CleanTaskExecuter.TaskController.Controller.Impl;

public class DefaultTaskInstanceResolver : ITaskInstanceResolver
{
	private IServiceProvider? ServiceProvider { get; }

	public DefaultTaskInstanceResolver(IServiceProvider? serviceProvider = null) =>
		ServiceProvider = serviceProvider;

	public object ResolveType(Type type) =>
		(ServiceProvider?.GetService(type) ?? Activator.CreateInstance(type))!;
}