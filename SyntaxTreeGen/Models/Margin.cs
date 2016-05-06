namespace SyntaxTreeGen.Models
{
    /// <summary>
    /// Tracks the current size of the margin to be used in code output
    /// </summary>
    public static class Margin
    {
        private static int _depth;

        /// <summary>
        /// Increase the margin size
        /// </summary>
        public static void Indent()
        {
            _depth++;
        }

        /// <summary>
        /// Decrease the margin size
        /// </summary>
        public static void Outdent()
        {
            _depth--;
        }
        
        /// <summary>
        /// Provides the current margin
        /// </summary>
        /// <returns></returns>
        public static string Tab()
        {
            var i = _depth;
            var x = "";

            while (i > 0)
            {
                x += "  ";
                i--;
            }

            return x;
        }
    }
}
