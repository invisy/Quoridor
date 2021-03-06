using Quoridor.Core.Abstraction.Common;

namespace Quoridor.MVC.Extensions
{
    public static class FenceDirectionExtensions
    {
        public static FenceDirection? ParseFenceDirection(this string stringDirection)
        {
            return stringDirection switch
            {
                "h" => FenceDirection.Horizontal,
                "v" => FenceDirection.Vertical,
                _ => default
            };
        }

        public static bool IsFenceDirection(this string stringDirection)
        {
            return stringDirection switch
            {
                "h" => true,
                "v" => true,
                _ => false
            };
        }
    }
}