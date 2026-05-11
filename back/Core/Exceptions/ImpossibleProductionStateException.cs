using Core.Enums;

namespace Core.Exceptions
{
    public class ImpossibleProductionStateException : Exception
    {
        public ImpossibleProductionStateException(ProductionStates desiredState, string? reason)
            : base(BuildMessage(desiredState, reason))
        {
        }

        private static string BuildMessage(ProductionStates desiredState, string? reason)
        {
            string reasonString = reason == null ? "" : $"Reason:\n{reason}";
            return $"Production couldn't reach the desired state: '{desiredState}'\n{reasonString}";
        }
    }
}
