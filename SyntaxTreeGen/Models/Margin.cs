namespace SyntaxTreeGen.Models
{
    public static class Margin
    {
        public static int Depth;

        public static void Indent()
        {
            Depth++;
        }

        public static void Outdent()
        {
            Depth--;
        }

        public static string Tab(int i)
        {
            var x = "";

            while (i > 0)
            {
                x += "  ";
                i--;
            }

            return x;
        }
        public static string Tab()
        {
            int i = Depth;
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
