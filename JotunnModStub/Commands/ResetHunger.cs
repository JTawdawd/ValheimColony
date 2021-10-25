using Jotunn.Entities;

namespace JotunnModStub.Commands
{
    //creating a commmand to clear hunger to test new foods
    public class ResetHunger : ConsoleCommand
    {
        public override string Name => "ResetHunger";

        public override string Help => "Clears hunger";

        public override void Run(string[] args)
        {
            Console.instance.Print("Hunger cleared");
            Player.m_localPlayer.ClearFood();
        }
    }
}
