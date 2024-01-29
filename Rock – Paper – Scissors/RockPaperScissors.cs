
const string Rock = "Rock";
const string Paper = "Paper";
const string Scissors = "Scissors";

Console.Write("Choose [r]ock, [p]aper or [s]cissors: ");
string playerMove = Console.ReadLine();

if (playerMove == "r" || playerMove == "rock" || playerMove == "Rock")
{
    playerMove = Rock;
}
else if (playerMove == "p" || playerMove == "paper" || playerMove == "Paper")
{
    playerMove = Paper;
}
else if (playerMove == "s" || playerMove == "scissors" || playerMove == "Scissors")
{
    playerMove = Scissors;
}
else
{
    Console.WriteLine("Invalid Input. Try Again.");
    return;
}

Random random = new Random();
int computerRandomNumber = random.Next(1, 4);
string computerMove = null;

switch (computerRandomNumber)
{
    case 1:
        computerMove = Rock;
        break;
    case 2:
        computerMove = Paper;
        break;
    case 3:
        computerMove = Scissors;
        break;
}

Console.WriteLine($"The computer chose {computerMove}.");

if ((playerMove == Rock && computerMove == Scissors) || (playerMove == Paper && computerMove == Rock) || (playerMove == Scissors && computerMove == Paper))
{
    Console.WriteLine("You win!");
}
else if ((playerMove == Scissors && computerMove == Rock) || (playerMove == Rock && computerMove == Paper) || (playerMove == Paper && computerMove == Scissors))
{
    Console.WriteLine("You lose!");
}
else if ((playerMove == Scissors && computerMove == Scissors) || (playerMove == Rock && computerMove == Rock) || (playerMove == Paper && computerMove == Paper))
{
    Console.WriteLine("Draw!");
}
