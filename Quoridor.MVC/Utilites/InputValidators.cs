using System;
using Quoridor.MVC.Extensions;
using System.Collections.Generic;
using System.Linq;
using Quoridor.MVC.Structures;

namespace Quoridor.MVC.Utilites
{
    static class InputValidators
    {
        public static bool IsPlaceFenceArgumentsValid(this IEnumerable<char> arguments) =>
            arguments is not null
            && arguments.Count() == 3
            && IsHorizontalCoordinateValid(arguments.ElementAt(0))
            && IsVerticalCoordinateValid(arguments.ElementAt(1))
            && arguments.ElementAt(2).IsFenceDirection();

        public static bool IsMovePawnArgumentsValid(this IEnumerable<char> arguments) =>
            arguments is not null
            && arguments.Count() == 2
            && IsHorizontalCoordinateValid(arguments.ElementAt(0))
            && IsVerticalCoordinateValid(arguments.ElementAt(1));

        public static bool IsCommandValid(this string command) =>
            !string.IsNullOrWhiteSpace(command);

        static bool IsHorizontalCoordinateValid(char horizontalCoordinate)
        {
            return Enum.TryParse<HorizontalNotation>(horizontalCoordinate.ToString(), out _);
        }

        static bool IsVerticalCoordinateValid(char verticalCoordinate)
        {
            return int.TryParse(verticalCoordinate.ToString(), out var a) && a != 0;
        }
    }
}
