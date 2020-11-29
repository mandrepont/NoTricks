using System;

namespace NoTricks.CLI {
    internal class ScopedConsoleColor: IDisposable {
        private readonly ConsoleColor _previousForeground;
        private readonly ConsoleColor _previousBackground;

        public ScopedConsoleColor(ConsoleColor foreground, ConsoleColor? background = null) {
            _previousForeground = Console.ForegroundColor;
            _previousBackground = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            if (background != null) {
                Console.BackgroundColor = background.Value;
            }
        }

        public void Dispose() {
            Console.ForegroundColor = _previousForeground;
            Console.BackgroundColor = _previousBackground; 
        }

        public static ScopedConsoleColor Error() => new ScopedConsoleColor(ConsoleColor.Red);
        public static ScopedConsoleColor Success() => new ScopedConsoleColor(ConsoleColor.Green);
        public static ScopedConsoleColor Info() => new ScopedConsoleColor(ConsoleColor.Blue);
    }
}