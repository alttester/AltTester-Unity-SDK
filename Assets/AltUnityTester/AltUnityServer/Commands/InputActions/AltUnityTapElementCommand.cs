using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Assets.AltUnityTester.AltUnityServer.Communication;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTapElementCommand : AltUnityCommand<AltUnityTapElementParams, AltUnityObject>
    {
        private readonly ICommandHandler handler;

        public AltUnityTapElementCommand(ICommandHandler handler, AltUnityTapElementParams cmdParams) : base(cmdParams)
        {
            this.handler = handler;
        }

        public override AltUnityObject Execute()
        {

#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altUnityObject.getScreenPosition().x, CommandParams.altUnityObject.getScreenPosition().y));
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);

            Input.TapElement(gameObject, CommandParams.count, CommandParams.interval, onFinish);

            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject);
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

        private void onFinish(UnityEngine.GameObject gameObject)
        {
            if (CommandParams.wait)
                handler.Send(ExecuteAndSerialize(() => "Finished"));
        }
    }
}
