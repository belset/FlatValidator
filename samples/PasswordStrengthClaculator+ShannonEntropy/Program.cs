using System.Validation;

bool abortProcess = false;
Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) => e.Cancel = abortProcess = true;

while (!abortProcess)
{
    Console.Write("Enter password:");
    var password = Console.ReadLine();
    if (string.IsNullOrEmpty(password)) 
        break;

    var passwordStrength = FlatValidatorFuncs.GetPasswordStrength(password, out var score, out var maxScore);
    var entropy = FlatValidatorFuncs.GetShannonEntropy(password, out var entropyInBits);
    Console.WriteLine($"   strength: {passwordStrength}, " +
                      $"score/maxScore: {score}/{maxScore} = {Math.Round((double)score / maxScore, 2)}, " +
                      $"entropy: {entropyInBits} bits ({entropy})");
}