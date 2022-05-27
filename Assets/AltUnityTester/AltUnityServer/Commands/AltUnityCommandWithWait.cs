using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    public abstract class AltUnityCommandWithWait<TParam, TResult> : AltUnityCommand<TParam, TResult> where TParam : CommandParams
    {
        private readonly bool _wait;
        private readonly ICommandHandler _handler;

        protected AltUnityCommandWithWait(TParam commandParams, ICommandHandler handler, bool wait) : base(commandParams)
        {
            this._wait = wait;
            this._handler = handler;
        }
        protected virtual void onFinish(Exception err)
        {
            if (this._wait)
                if (err != null)
                {
                    _handler.Send(ExecuteAndSerialize<string>(() => throw new AltUnityInnerException(err)));
                }
                else
                {
                    _handler.Send(ExecuteAndSerialize(() => "Finished"));
                }
        }
    }
}
