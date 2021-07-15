using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Assets.AltUnityTester.AltUnityServer.Communication;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickElementCommand : AltUnityCommand<AltUnityClickElementParams, AltUnityObject>
    {
        private readonly ICommandHandler handler;

        public AltUnityClickElementCommand(ICommandHandler handler, AltUnityClickElementParams cmdParams) : base(cmdParams)
        {
            this.handler = handler;
        }

        public override AltUnityObject Execute()
        {
            UnityEngine.Debug.Log(CommandParams);

#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altUnityObject.getScreenPosition().x, CommandParams.altUnityObject.getScreenPosition().y));
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);

            Input.ClickElement(gameObject, CommandParams.count, CommandParams.interval, onFinish);

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
