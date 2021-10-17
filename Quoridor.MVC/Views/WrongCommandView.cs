using Quoridor.MVC.Extensions;
using Quoridor.MVC.Structures;
using System;

namespace Quoridor.MVC.Views
{
    class WrongCommandView
    {
        public void DrawMessage(WrongCommandReason reason)
        {
            Console.WriteLine($"Sorry the input is wrong. Reason: {reason.GetMessage()}");
        }
    }
}
