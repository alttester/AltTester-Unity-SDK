/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

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
