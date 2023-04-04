using System;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltEndTouchCommand : AltCommandWithWait<AltEndTouchParams, string>
    {
        private readonly ICommandHandler _handler;
        int fingerId;
        public AltEndTouchCommand(ICommandHandler handler, AltEndTouchParams cmdParams) : base(cmdParams, handler, true)
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
                _handler.Send(ExecuteAndSerialize<string>(() => throw new AltInnerException(err)));
            }
            else
            {
                _handler.Send(ExecuteAndSerialize(() => "Ok"));
            }
        }
    }
}