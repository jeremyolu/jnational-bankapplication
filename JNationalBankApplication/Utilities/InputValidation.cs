using JNationalBankApplication.Interfaces;

namespace JNationalBankApplication.Utilities
{
    public class InputValidation : IInputValidation
    {
        public bool ValidateUserInputForInt(string input)
        {
            int value;

            if(int.TryParse(input, out value))
            {
                return true;
            }

            return false;
        }
    }
}
