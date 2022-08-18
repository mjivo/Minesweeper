namespace Minesweeper.IO.Contracts
{
    internal interface IWriter
    {
        public void Write(string input);
        //public void Write(byte[] input);
        public void WriteLine(string input);
    }
}
