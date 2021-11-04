using Quoridor.Core.Abstraction.Common;

namespace Quoridor.MVC.Extensions
{
    public static class FenceDirectionExtensions
    {
        public static FenceDirection? ParseFenceDirection(this char stringDirection)
        {
            return stringDirection switch
            {
                "h" => FenceDirection.HORIZONTAL,
                "v" => FenceDirection.VERTICAL,
                _ => default
            };
        }

        public static bool IsFenceDirection(this char stringDirection)
        {
            return stringDirection switch
            {
                'h' => true,
                'v' => true,
                _ => false
            };
        }
    }
}