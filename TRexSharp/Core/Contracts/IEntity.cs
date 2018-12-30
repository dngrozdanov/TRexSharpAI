namespace TRexSharp.Core.Contracts
{
    public interface IEntity
    {
        void ShortJump();
        void LongJump();
        void HoldDuck();
        void ReleaseDuck();
        bool IsDucking();
    }
}