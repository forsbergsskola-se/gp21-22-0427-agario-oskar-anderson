namespace TNC.Interfaces
{
    public interface IOutputLogger
    {
        /// <summary>
        /// Print the input value to the preferred logging tool.
        /// </summary>
        /// <param name="text"></param>
        public void PrintOutput(string text);
    }
}