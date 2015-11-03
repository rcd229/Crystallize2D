public delegate void ProcessRequestHandler(object sender, ProcessRequestEventArgs args);
public delegate void ProcessRequestHandler<I, O>(object sender, ProcessRequestEventArgs<I, O> args);

public delegate IProcess<I, O> GetProcessInstance<I, O>();

public delegate void ProcessExitCallback(object sender, object args);
public delegate void ProcessExitCallback<in O>(object sender, O args);