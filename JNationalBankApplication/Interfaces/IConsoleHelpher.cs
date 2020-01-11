namespace JNationalBankApplication.Interfaces
{
    public interface IConsoleHelpher
    {
        string GetUserInput();

        void DisplayText(string text);

        void TextFormatLine();

        void ClearScreen();

        void SetTextColour(string textColour);

        void ResetColour();

        void ReadLine();
    }
}
