// namespace Altom.AltUnityDriver.Commands
// {
//     public class AltUnitySyncCommand : AltBaseCommand
//     {
//         public AltUnitySyncCommand(IDriverCommunication commHandler) : base(commHandler)
//         {
//         }
//         public void Execute()
//         {
//             SendCommand("getServerVersion");
//             while (true)
//             {
//                 try
//                 {
//                     Recvall();
//                     break;
//                 }
//                 catch (AltUnityRecvallMessageIdException)
//                 {
//                 }
//             }
//         }
//     }
// }