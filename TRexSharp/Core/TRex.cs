using WindowsInput;
using WindowsInput.Native;
using TRexSharp.Core.Contracts;

namespace TRexSharp.Core
{
    internal class TRex : IEntity
    {
        private readonly IInputSimulator inputSimulator;

        public TRex(IInputSimulator inputSimulator)
        {
            this.inputSimulator = inputSimulator;
        }

        public void ShortJump()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
        }

        public void LongJump()
        {
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
            inputSimulator.Keyboard.Sleep(200);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.SPACE);
        }

        public void HoldDuck()
        {
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.DOWN);
        }

        public void ReleaseDuck()
        {
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.DOWN);
        }

        public bool IsDucking()
        {
            return inputSimulator.InputDeviceState.IsKeyDown(VirtualKeyCode.DOWN);
        }
    }
}