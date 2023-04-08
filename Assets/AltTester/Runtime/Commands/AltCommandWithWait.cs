using System;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public abstract class AltCommandWithWait<TParam, TResult> : AltCommand<TParam, TResult> where TParam : CommandParams
    {
        private readonly bool _wait;
        private readonly ICommandHandler _handler;

        protected AltCommandWithWait(TParam commandParams, ICommandHandler handler, bool wait) : base(commandParams)
        {
            this._wait = wait;
            this._handler = handler;
        }
        protected virtual void onFinish(Exception err)
        {
            if (this._wait)
                if (err != null)
                {
                    _handler.Send(ExecuteAndSerialize<string>(() => throw new AltInnerException(err)));
                }
                else
                {
                    _handler.Send(ExecuteAndSerialize(() => "Finished"));
                }
        }
    }
}
