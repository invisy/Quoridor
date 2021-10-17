using System.Collections.Generic;
using System.Linq;

using Quoridor.Core.Abstraction.Common;
using Quoridor.MVC.Extensions;

namespace Quoridor.MVC.Utilites
{
    static class InputValidators
    {
        public static bool IsPlaceFenceArgumentsValid(this IList<string> arguments) =>
            int.TryParse(arguments.ElementAtOrDefault(0), out _)
            && int.TryParse(arguments.ElementAtOrDefault(1), out _)
            && arguments.ElementAtOrDefault(2).IsFenceDirection();

        public static bool IsMovePawnArgumentsValid(this IList<string> arguments) =>
            int.TryParse(arguments.ElementAtOrDefault(0), out _)
            && int.TryParse(arguments.ElementAtOrDefault(1), out _);

        public static bool IsCommandValid(this string command) => 
            !string.IsNullOrWhiteSpace(command);
    }
}
