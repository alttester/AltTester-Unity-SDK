using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityEndTouchCommand : AltUnityCommandWithWait<AltUnityEndTouchParams, string>
    {
        private readonly ICommandHandler _handler;
        int fingerId;
        public AltUnityEndTouchCommand(ICommandHandler handler, AltUnityEndTouchParams cmdParams) : base(cmdParams, handler, true)
        {
            this._handler = handler;
        }
        public override string Execute()
        {
            InputController.EndTouch(CommandParams.fingerId, onFinish);
            return "Ok";
        }
        //TODO: remove this
        protected override void onFinish(Exception err)
        {
            if (err != null)
            {
                _handler.Send(ExecuteAndSerialize<string>(() => throw new AltUnityInnerException(err)));
            }
            else
            {
                _handler.Send(ExecuteAndSerialize(() => "Ok"));
            }
        }
    }
}